# Prove — Full-stack KYC verification

`IMonoProve` wraps Mono's Prove product — a redirect-based KYC flow that
combines BVN/NIN ownership, government-ID + face match, address
verification, and optional bank-account linking.

Uses `ProveSecretKey ?? SecretKey` ([configuration](./configuration.md#per-product-keys)).

> Prove lives under `/v1/` — older than the rest of the API. The library
> hides this; `BuildV1()` rewrites the base URL automatically.

## Flow

```
1. Your app: POST InitiateProve → returns mono_url + session_id
2. Redirect the customer to mono_url
3. Customer completes ID upload + face capture + bank link
4. Mono fires prove.completed (or prove.failed) webhook
5. Your app: GET FetchCustomerDetails(reference) for the full record
```

## 1. Initiate

```csharp
var prove = await _prove.InitiateProve(new InitiateProveModel
{
    Customer = new ProveCustomer
    {
        Name    = "Ada Lovelace",
        Phone   = "+2348012345678",
        Address = "12 Analytical St, Lagos",
        Email   = "ada@example.com",
        Identity = new ProveCustomerIdentity
        {
            Type   = ProveIdentityTypeConstants.Bvn,
            Number = "12345678901",
        },
    },
    Reference   = "kyc-ada-2026-05",   // your idempotency key
    RedirectUrl = "https://yourapp.com/kyc/done",
    KycLevel    = ProveKycLevelConstants.Tier2,
});

return Redirect(prove.Data.MonoUrl);
```

### KYC levels

`KycLevel` picks the verification depth. Higher tiers cost more and take
longer for the user to complete.

| Constant | Value | What it does |
|---|---|---|
| `Tier1` | `tier_1` | BVN + NIN ownership validation only |
| `Tier2` | `tier_2` | Tier 1 + government-ID + facial recognition |
| `Tier3` | `tier_3` | Tier 2 + address + residency confirmation |
| `Custom` | `custom` | Requires the `Identities` array (must include `bvn` or `nin`) |

For a custom flow that just verifies BVN ownership with face match:

```csharp
new InitiateProveModel
{
    Customer    = /* ... */,
    Reference   = "kyc-ada-1",
    RedirectUrl = "...",
    KycLevel    = ProveKycLevelConstants.Custom,
    Identities  = new List<string> { "bvn" },
    BankAccounts = true, // optionally include account linking
}
```

## 2. Handle the webhook

```csharp
public Task HandleProveCompletedEvent(ProveEventModel webhook)
{
    return _kyc.MarkVerifiedAsync(webhook.Reference, webhook.KycLevel);
}

public Task HandleProveFailedEvent(ProveEventModel webhook)
{
    return _kyc.MarkFailedAsync(webhook.Reference, webhook.FailureReason);
}
```

See [webhooks](./webhooks.md#newer-event-types--imonowebhookconsumerextensions).

## 3. Fetch the full verification record

```csharp
var details = await _prove.FetchCustomerDetails(reference);

if (details.Data.Status == "completed")
{
    foreach (var identity in details.Data.Identities)
    {
        // identity.Type ("bvn" / "nin"), identity.Status, identity.FirstName, etc.
    }

    var faceMatch = details.Data.FaceMatch;
    // faceMatch.Match (bool), faceMatch.Confidence (0–1),
    // faceMatch.ManualValidation (true if Mono had to fall back to manual review)

    var address = details.Data.AddressVerification;
    // address.Status, address.Confidence

    foreach (var bank in details.Data.BankAccounts ?? new List<ProveBankAccount>())
    {
        // bank.AccountNumber, BankName, AccountName
    }
}
```

`reference` is whatever you passed to `InitiateProve` — Mono treats it as
the customer key for the entire Prove product.

## 4. List all verified customers

```csharp
var all = await _prove.FetchAllCustomerDetails(new ProveCustomerListQueryOptions
{
    Page  = 1,
    Limit = 50,
    Status = "completed",
    KycLevel = ProveKycLevelConstants.Tier2,
});
```

## Blacklist / whitelist

If you discover fraud post-verification, blacklist the customer:

```csharp
await _prove.BlacklistCustomer(new BlacklistCustomerModel
{
    Reference = "kyc-ada-1",
    Reason    = "Confirmed identity theft per ticket #12345",
    Code      = ProveBlacklistCodeConstants.Code101,
});
```

`Code` is a numeric value in the 101–105 range. Mono hasn't published a
named mapping, so the library exposes them as `Code101` through `Code105`
pass-through constants. Pick whichever your compliance process documents
as meaning what.

Reinstate later if needed:

```csharp
await _prove.WhitelistCustomer(new WhitelistCustomerModel { Reference = "kyc-ada-1" });
```

## Revoke data access

Per NDPR / your customer's deletion request, revoke Mono's permission to
share this customer's data with your business:

```csharp
await _prove.RevokeDataAccess("kyc-ada-1");
```

After this, `FetchCustomerDetails` returns 404 for that reference.

## What happens on the manual-review path

If Prove's face match can't make a determination automatically, Mono routes
to manual review. The customer's status stays in `processing` for hours
until a human Mono operator reviews — then a `prove.completed` /
`prove.failed` webhook fires. `FaceMatch.ManualValidation` will be `true`
in the response if this path was used.

Don't poll aggressively during this state. Wait for the webhook.

## Pricing

Higher KYC tiers cost more per verification. Failed verifications are
generally not billed but consume sandbox quota — confirm the live pricing
with your account manager. Once a customer is verified, fetching
`FetchCustomerDetails` is free.

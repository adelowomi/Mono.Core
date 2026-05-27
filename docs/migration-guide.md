# Migration guide

Upgrading from an earlier version. Find your "from" version below and read
forward to your target. The library follows
[SemVer](https://semver.org/) — every change marked **(breaking)** requires
code changes; everything else is additive and source-compatible.

For the full per-release breakdown see [CHANGELOG.md](../CHANGELOG.md).

## Upgrading from 1.0.x → any 1.x

The biggest jump. Read in order — each section below describes what changed
*entering* that version.

### Migrating to 1.1.0

This release fixed drift that had accumulated since 2024-09. Most of the
breakage is in code that was silently broken before.

#### BVN endpoint paths changed (breaking — but the old paths returned 404)

```csharp
// Before — paths Mono no longer accepts
// POST /lookup/bvn/verify
// POST /lookup/bvn/details

// After — current Mono paths
// POST /lookup/bvn/verify-otp
// POST /lookup/bvn/fetch-bvn
```

If you were calling `VerifyBvnLookUp` or `GetBvnDetails` you weren't getting
real Mono responses anyway. No code change needed — the library now hits
the right paths.

#### BVN scope values lowercased (breaking)

```csharp
// Before
options.Scope = "IDENTITY";

// After
options.Scope = ScopeConstants.Identity; // "identity"
```

#### Lookup HTTP verbs (breaking — but the old verbs were invalid Refit)

Address / passport / TIN / NIN / drivers-license / account-number /
credit-history / mashup were declared `[Get]` with `[Body]`. Refit silently
drops the body on GET, so these endpoints have always sent empty requests.
Now `[Post]` per the docs.

No code change needed — the high-level method signatures are unchanged.

#### Driver's license path

```csharp
// Before: /lookup/driver_license  (underscore)
// After:  /lookup/drivers-license (hyphen)
```

No code change — internal.

#### International passport path

```csharp
// Before: /lookup/passport
// After:  /lookup/intl-passport
```

No code change — internal.

#### Mandate balance enquiry

`BalanceInquiry` `amount` parameter is now optional. Pass `null` for a
real-time balance fetch (NGN 50), pass an amount for a sufficient-funds
check (NGN 10).

```csharp
// Before — required to pass amount even for plain balance
await _directpay.BalanceInquiry(mandateId, "10000");

// After
var balance       = await _directpay.BalanceInquiry(mandateId);          // ₦50 fee, returns balance
var hasFunds      = await _directpay.BalanceInquiry(mandateId, "10000"); // ₦10 fee, returns sufficiency
```

#### CAC deprecations (source-compatible — `[Obsolete]`)

```csharp
[Obsolete] _lookup.GetPreviousAddress(...) // Mono retired
[Obsolete] _lookup.GetChangeOfName(...)    // Mono retired
```

Both still compile, but the underlying Mono endpoints no longer return
real data. Use `GetCacProfile(rcNumber)` from v1.8.0+ instead.

#### Mandate creation — new optional fields

`CreateMandateModel` gained `FeeBearer`, `VerificationMethod`, `Meta`.
Existing usages compile unchanged (all optional).

#### Refit security update

Refit bumped from `7.1.2` to `7.2.22` to fix CVE-2024-51501 (CRLF header
injection). Mostly transparent. If you have your own `RefitSettings`
customizations they should still work.

### Migrating to 1.2.0

Adds `IMonoCustomers`. Pure additive — nothing breaks. Register your
existing `IMonoWebhookConsumer` to receive `account_connected` /
`account_updated` events with customer context.

### Migrating to 1.3.0

Adds `IMonoDisburse`. Pure additive.

### Migrating to 1.4.0

Adds `IMonoWatchlist`. Pure additive.

### Migrating to 1.5.0

Adds `IMonoProve` + `IRefitClientBuilder<T>.BuildV1(string)`. Pure additive.
If you have a custom `IRefitClientBuilder<T>` implementation you'll need to
add `BuildV1`.

### Migrating to 1.6.0

#### Account Match webhook field

`AccountUpdatedEventModel` gained an `AccountMatch` sub-object. Existing
handlers compile unchanged (the field is nullable and only populated when
the linking request opted in).

#### NIN PDF flow

New endpoints `GetNinPdf` / `PollNinJob` for async PDF generation. Doesn't
affect existing `GetNin` callers.

### Migrating to 1.7.0 — **read carefully**

The webhook hardening release. Three things to know.

#### Signature verification — opt in

Add the secret to `MonoInitializationOptions`:

```csharp
services.AddMono(options =>
{
    // ... existing options
    options.WebhookSecret = config["Mono:WebhookSecret"];
});
```

Without it the controller still works but logs a warning per request and
accepts unauthenticated POSTs.

#### `IMonoWebhookConsumer` interface — unchanged

No code change needed. The new event types live on a separate optional
interface (`IMonoWebhookConsumerExtensions`).

#### `MonoEventTypes` constant values — **breaking value change**

If you reference the mandate constants directly:

```csharp
// Before
MonoEventTypes.MandateCreated  // was "created"
MonoEventTypes.MandateReady    // was "ready"
MonoEventTypes.MandateApproved // was "approved"

// After
MonoEventTypes.MandateCreated  // now "mandate.created"
MonoEventTypes.MandateReady    // now "mandate.ready"
MonoEventTypes.MandateApproved // now "mandate.approved"
```

The old values were technically broken — the library's controller
routing compared them against `"mandate"` (not the suffix) and silently
dropped every mandate webhook into `HandleUnknownEvent`. If your own code
hardcoded the old values for your own routing, update to the new ones (or
to `mandate.created` etc. as string literals).

#### Mandate webhooks now actually route

Before v1.7.0, `mandate.created` / `mandate.approved` / `mandate.ready` all
fell through to `HandleUnknownEvent` due to the routing bug. After v1.7.0
they reach `HandleMandateCreatedEvent` / etc. as intended.

**If you implemented mandate handling inside `HandleUnknownEvent`, remove
it.** The mandate-specific handlers will now fire and you'd double-process.

### Migrating to 1.8.0

Adds CAC `GetCacPsc` / `GetCacProfile` / `GetCacStatusReport`. Pure
additive. Use `GetCacProfile` as the replacement for the deprecated
`GetPreviousAddress` / `GetChangeOfName`.

### Migrating to 1.9.0

#### Per-product secret keys

Mono apps are now scoped to one product family. If you have separate
DirectPay / Disburse / Prove apps, configure the per-product overrides:

```csharp
services.AddMono(options =>
{
    options.BaseUrl  = "https://api.withmono.com/v2";
    options.SecretKey = "fallback_key";  // optional if you set all overrides

    options.ConnectSecretKey   = "...";  // IMonoAccounts, IMonoCustomers
    options.LookupSecretKey    = "...";  // IMonoLookUp, IMonoWatchlist
    options.DirectPaySecretKey = "...";  // IMonoDirectPay (NEW)
    options.DisburseSecretKey  = "...";  // IMonoDisburse (NEW)
    options.ProveSecretKey     = "...";  // IMonoProve (NEW)
});
```

Existing single-key setups (`SecretKey` only) work unchanged. The new keys
all fall back to `SecretKey` when unset.

#### DirectPay payment-transactions endpoint fix

`IMonoDirectPay.GetTransactions` was hitting `/v3/payments/transactions`
(which doesn't exist) and returning Mono's HTML 404 page. v1.9.0 routes
to `/v2/payments/transactions` correctly. No code change; calls that were
previously throwing `'<' is an invalid start of a value` JSON exceptions
now succeed.

#### Query-string serialization fix

Refit query-string parameters were serializing C# property names
(`Page=1&Status=settled`) instead of the snake_case Mono expects
(`page=1&status=settled`). Fixed across all 11 query-option classes by
switching from `[JsonPropertyName]` to Refit's `[AliasAs]`. No source-level
change — same property names, just correct wire format.

If Mono had been case-insensitive enough to accept the wrong format,
nothing breaks. If your filter calls were silently being ignored, they now
work.

## Quick reference — by symptom

| Symptom | Likely fix |
|---|---|
| Mandate webhooks don't fire (always go to `HandleUnknownEvent`) | Upgrade to 1.7.0+ |
| `GetTransactions` throws `'<' is an invalid start of a value` | Upgrade to 1.9.0 |
| `"Please use a Lookup app..."` error | Set `LookupSecretKey` separately (1.0.x+) |
| `"Please use a DirectPay app..."` / `"Disburse"` / `"Prove"` | Set the matching per-product key (1.9.0+) |
| BVN flow returns 404 | Upgrade to 1.1.0+ |
| Listing/filter query params seem ignored by Mono | Upgrade to 1.9.0 |
| Refit security warning (CVE-2024-51501) | Upgrade to 1.1.0+ |

# DirectPay — One-time payments, mandates, payouts

`IMonoDirectPay` covers Mono's payments surface:

- **One-time payments** (bank-transfer pull)
- **Mandates** for recurring debits
- **Refunds**
- **Payouts** (your settlements from Mono) + payout transactions
- **Sub-accounts** for split payments

Uses `DirectPaySecretKey ?? SecretKey` ([configuration](./configuration.md#per-product-keys)).

## One-time payment

```csharp
var payment = await _directpay.InitiatePayment(new InitiateOneTimePayment
{
    Amount      = 50_000_00,   // ₦50,000 in kobo
    Type        = "onetime-debit",
    Description = "Order #1234",
    Reference   = "ord-1234",
    RedirectUrl = "https://yourapp.com/checkout/done",
    Customer    = new PaymentCustomer
    {
        Email = "ada@example.com",
        Phone = "+2348012345678",
        Name  = "Ada Lovelace",
    },
});

// payment.Data.PaymentLink is what you redirect the user to.
return Redirect(payment.Data.PaymentLink);
```

After the user completes payment, **always verify before giving value** —
the webhook is asynchronous and can be spoofed if you don't have
[signature verification](./webhooks.md#signature-verification) set up:

```csharp
var verify = await _directpay.VerifyPayment(reference);
if (verify.Data.Status == "successful")
{
    await _orders.MarkPaidAsync(orderId);
}
```

## Refunds

```csharp
var refund = await _directpay.RefundPayment(new RefundPaymentModel
{
    Reference = "ord-1234",
    Source    = RefundSourceConstants.Wallet, // or PendingPayout (default)
});
```

`source` controls where the refund money comes from:
- `wallet` — your dashboard wallet (requires balance)
- `pending_payout` — deducted from your next Mono payout (default)

## Mandates (recurring debits)

Two-step flow: customer authorizes a mandate, then you debit it.

### 1. Initiate authorization

```csharp
var auth = await _directpay.InitiatePayment(new InitiateOneTimePayment
{
    Type         = "recurring-debit",
    Method       = "mandate",
    MandateType  = "emandate",
    DebitType    = "fixed",
    Amount       = 10_000_00,
    AccountNumber = "0123456789",
    BankCode     = "044",
    Reference    = "mandate-ada-1",
    RedirectUrl  = "https://yourapp.com/mandates/done",
    StartDate    = "2026-06-01",
    EndDate      = "2027-06-01",
    Customer     = new PaymentCustomer { Id = customerId },
});

return Redirect(auth.Data.PaymentLink);
```

### 2. Webhook: `mandate.created` → `mandate.approved` → `mandate.ready`

When `mandate.ready` fires the mandate is debitable. Save the mandate id
from `MandateReadyEventModel.Id`.

### 3. Debit it

```csharp
var debit = await _directpay.DebitMandate(new DebitAccountModel
{
    Amount    = 10_000_00,
    Reference = "debit-ada-jun-2026",
    Narration = "June subscription",
}, mandateId);
```

Webhook `mandate.debit.successful` or `mandate.debit.failed` confirms.

### 4. Balance & status checks

```csharp
// Real-time balance (NGN 50 fee) — pass null for the amount
var bal = await _directpay.BalanceInquiry(mandateId);

// Sufficient-funds check (NGN 10 fee) — pass the amount you want to debit
var enough = await _directpay.BalanceInquiry(mandateId, "10000");
```

### 5. Lifecycle

```csharp
await _directpay.PauseMandate(mandateId);
await _directpay.ReinstateMandate(mandateId);
await _directpay.CancelMandate(mandateId); // permanent
```

### Mandate creation via the v3 API

If you want full control over the mandate body (selfie verification,
fee-bearer choice, custom intervals) use `CreateMandate` directly:

```csharp
var mnd = await _directpay.CreateMandate(new CreateMandateModel
{
    DebitType          = DebitTypeConstants.Fixed,
    MandateType        = MandateTypeConstants.EMandate,
    Customer           = customerId,
    AccountNumber      = "0123456789",
    BankCode           = "044",
    Amount             = 10_000_00,
    Reference          = "mandate-ada-1",
    Description        = "Monthly subscription",
    StartDate          = "2026-06-01",
    EndDate            = "2027-06-01",
    FeeBearer          = FeeBearerConstants.Business,
    VerificationMethod = VerificationMethodConstants.SelfieVerification,
});
```

The response's `MonoUrl` (if any) is where the customer completes the
biometric / OTP verification.

## Listing debits

```csharp
var debits = await _directpay.RetrieveAllDebits(mandateId);
var one    = await _directpay.RetrieveDebitByReference(mandateId, "debit-ada-jun-2026");
```

## Listing mandates

```csharp
var mandates = await _directpay.GetMandates(new MandateRequestQueryOptions { Page = 1, Limit = 50 });
var mandate  = await _directpay.GetMandateById(mandateId);
```

## Payouts (your settlements from Mono)

When customers pay you via DirectPay or Direct Debit, Mono settles the
collected funds to your bank account as **payouts**. Inspect them:

```csharp
// List by lifecycle state
var settled = await _directpay.GetPayouts(new PayoutListQueryOptions
{
    Status = PayoutStatusConstants.Settled,
    Page   = 1,
    Limit  = 50,
});

// All payments that made up a specific payout
var txns = await _directpay.GetPayoutTransactions(payoutId);
```

`PayoutStatusConstants`: `pending` → `processing` → `settled` (or `failed`).

## Split payments (sub-accounts)

Create a sub-account once, then route a percentage of each payment to it:

```csharp
var sub = await _directpay.CreateSubAccount(new CreateSubAccountModel
{
    Name          = "Vendor A",
    AccountNumber = "0987654321",
    BankCode      = "057",
    NipCode       = "000014",
});

var all = await _directpay.GetSubAccounts();
```

Then reference `sub.Data.Id` in the `split` object of `InitiateOneTimePayment`.

## Payment transactions

For DirectPay payments across all your customers:

```csharp
var pays = await _directpay.GetTransactions(new PaymentRequestQueryOptions
{
    Page   = 1,
    Start  = DateTime.UtcNow.AddDays(-30),
    End    = DateTime.UtcNow,
    Status = "successful",
});
```

## What's where in the API

The library hides the version split but for debugging:

| Endpoint group | Mono version |
|---|---|
| `/payments/initiate`, `/payments/verify`, `/payments/refund` | v2 |
| `/payments/transactions`, `/payments/payouts`, `/payments/payout/*` | v2 |
| `/payments/mandates/*` (create, list, get, pause, debit, balance, etc.) | v3 |

The library uses both v2 and v3 clients internally — `Build()` for v2,
`BuildV3()` for v3.

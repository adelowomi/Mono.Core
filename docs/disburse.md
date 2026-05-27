# Disburse — Outbound payouts

`IMonoDisburse` handles outbound payments — salary runs, vendor settlements,
marketplace splits, cashback. All endpoints live under `/v3/payments/disburse/...`.

Uses `DisburseSecretKey ?? SecretKey` ([configuration](./configuration.md#per-product-keys)).

## Conceptual model

```
Source Account ─→ Disbursement (batch) ─→ Distributions (recipients)
   (you fund)         (one job)             (one per payee)
```

You register a source account once, then create a disbursement batch
containing one or more distributions. Trigger the batch immediately
(`instant`) or schedule it for later (`scheduled`).

## 1. Register a source account

This is the bank account funding your disbursements. Mono needs to know
about it before you can disburse from it.

```csharp
var src = await _disburse.CreateSourceAccount(new CreateSourceAccountModel
{
    App           = "your_app_id",
    AccountNumber = "0123456789",
    BankCode      = "044",
    Email         = "ops@yourbusiness.com",
});

var sourceAccountId = src.Data.Id;
```

List + fetch:

```csharp
var all = await _disburse.FetchAllSourceAccounts();
var one = await _disburse.FetchSourceAccount(sourceAccountId);
```

Update (e.g. change email):

```csharp
await _disburse.UpdateSourceAccount(new UpdateSourceAccountModel
{
    Id    = sourceAccountId,
    Email = "new-ops@yourbusiness.com",
});
```

## 2. Create a disbursement

### Instant — executes immediately

```csharp
var dsb = await _disburse.CreateInstantDisbursement(new CreateDisbursementModel
{
    Reference   = "payroll-2026-05",
    Account     = sourceAccountId,
    TotalAmount = 750_000_00, // ₦750k in kobo (sum of distributions)
    Description = "May payroll",
    Distribution = new List<DistributionModel>
    {
        new DistributionModel
        {
            Reference      = "payroll-ada",
            RecipientEmail = "ada@example.com",
            Account        = new DisbursementRecipientAccount
            {
                AccountNumber = "0123456789",
                BankCode      = "044",
            },
            Amount    = 250_000_00,
            Narration = "May salary",
        },
        // ... more distributions
    },
});
```

The `CreateInstantDisbursement` wrapper sets `Type = "instant"` for you.

### Scheduled — executes on a future date

```csharp
await _disburse.CreateScheduledDisbursement(new CreateDisbursementModel
{
    Reference     = "payroll-2026-06",
    Account       = sourceAccountId,
    TotalAmount   = 750_000_00,
    Description   = "June payroll (scheduled)",
    ScheduledDate = "2026-06-01T09:00:00Z",
    Distribution  = /* ... */,
});
```

The `CreateScheduledDisbursement` wrapper sets `Type = "scheduled"`.

## 3. Transition a scheduled disbursement

Trigger early or cancel:

```csharp
// Run a scheduled batch now
await _disburse.TransitionDisbursement(batchId, new TransitionDisbursementModel
{
    Action = TransitionActionConstants.Trigger,
});

// Cancel before it fires
await _disburse.TransitionDisbursement(batchId, new TransitionDisbursementModel
{
    Action = TransitionActionConstants.Cancel,
});
```

## 4. Listen for results

Implement `IMonoWebhookConsumerExtensions.HandleDisbursementEvent` to
receive the four lifecycle events:

| Event | When |
|---|---|
| `disbursement.initiated` | Batch accepted, processing not yet started |
| `disbursement.processing` | NIBSS / interbank processing in flight |
| `disbursement.completed` | All distributions settled |
| `disbursement.failed` | At least one distribution failed; check per-distribution status |

```csharp
public Task HandleDisbursementEvent(DisbursementEventModel webhook)
{
    return webhook.Status switch
    {
        "completed" => _payroll.MarkCompleteAsync(webhook.Reference),
        "failed"    => _payroll.MarkFailedAsync(webhook.Reference, webhook.FailureReason),
        _           => _payroll.UpdateStatusAsync(webhook.Reference, webhook.Status),
    };
}
```

See [webhooks](./webhooks.md#newer-event-types--imonowebhookconsumerextensions).

## 5. Inspect

```csharp
// All disbursement batches
var all = await _disburse.FetchAllDisbursements(new DisbursementListQueryOptions
{
    Page   = 1,
    Limit  = 50,
    Status = "completed",
});

// One batch
var one = await _disburse.FetchDisbursement(batchId);

// All distributions in a batch
var dist = await _disburse.FetchAllDistributionsInBatch(batchId);

// One distribution
var single = await _disburse.FetchSingleDistribution(batchId, distributionId);
```

## Managing distributions after batch creation

You can edit a batch's distributions before it executes:

```csharp
// Add more recipients
await _disburse.AddDistributionsToBatch(batchId, new AddDistributionsModel
{
    Distribution = new List<DistributionModel> { /* ... */ },
});

// Edit one (amount typo, wrong account number, etc.)
await _disburse.UpdateDistributionInBatch(batchId, distributionId, new UpdateDistributionModel
{
    Amount = 200_000_00,
});

// Remove one
await _disburse.DeleteDistributionInBatch(batchId, distributionId);
```

After a batch transitions to `processing`, mutations are rejected by Mono.

## Amount semantics

All amounts are in minor units of the source account's currency: kobo for
NGN, pesewa for GHS, cents for KES/ZAR. `TotalAmount` on the batch must
equal the sum of distribution `Amount` values, or Mono will reject the
batch.

## Common errors

| Mono message | Likely cause |
|---|---|
| `"Insufficient funds in source account"` | Top up the source account or fund the linked mandate |
| `"Invalid bank code"` | Use NIP codes, not CBN codes — see `LookUp.GetBanks` |
| `"Distribution count exceeds limit"` | Mono caps batch size; split into multiple batches |
| `"Source account not approved"` | New source accounts need Mono-side approval before first use |

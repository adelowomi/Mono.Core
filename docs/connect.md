# Connect — Accounts & Customers

The Mono Connect product covers bank-account linking, balance / transaction
/ statement access, and customer modeling.

Three library interfaces work together:

| Interface | What it does |
|---|---|
| `IMonoAccounts` | Initiate linking, fetch account data |
| `IMonoCustomers` | Model your end users; tie linked accounts to them |
| `IMonoAuthorization` | Authorize / reauthorize / sync linked accounts |

All three use `ConnectSecretKey ?? SecretKey` ([configuration](./configuration.md#per-product-keys)).

## Account linking flow

```
1. Create a Customer record (optional but recommended)
2. Call InitiateAccountLinking → returns mono_url
3. Redirect user to mono_url; they authenticate with their bank
4. Mono fires the `account_connected` webhook
5. Pull account / income / statement / transaction data via the account id
```

### Step 1 — Create a customer

```csharp
var customer = await _customers.CreateIndividualCustomer(new CreateIndividualCustomerModel
{
    FirstName = "Ada",
    LastName  = "Lovelace",
    Email     = "ada@example.com",
    Phone     = "+2348012345678",
    Identity  = new CustomerIdentity
    {
        Type   = CustomerIdentityTypeConstants.Bvn,
        Number = "12345678901",
    },
});
```

Business customers use `CreateBusinessCustomerModel` with `BusinessName`
instead of `FirstName`/`LastName`.

### Step 2 — Start the linking session

```csharp
var session = await _accounts.InitiateAccountLinking(new AccountLinkingModel
{
    Customer    = new Customer { Name = "Ada Lovelace", Email = "ada@example.com" },
    Scope       = "auth",
    RedirectUrl = "https://yourapp.com/mono/done",
    Meta        = new Meta { Ref = "kyc-2026-05-ada" },
});

// session.Data.MonoUrl is what you redirect the user to
return Redirect(session.Data.MonoUrl);
```

### Step 2.5 (optional) — Account Match

Want to verify the linked account number matches one you already have on
file? Set `Institution.AccountNumber` and `CheckAccountMatch=true`:

```csharp
var session = await _accounts.InitiateAccountLinking(new AccountLinkingModel
{
    Customer = ...,
    Scope    = "auth",
    RedirectUrl = "...",
    Institution = new AccountLinkingInstitution
    {
        Id            = "044",          // bank code or Mono institution id
        AccountNumber = "0123456789",   // the number you expect
    },
    CheckAccountMatch = true,
});
```

Result arrives on the `account_updated` webhook in the `account_match`
field — `status: "matched" | "not_matched"`, plus expected vs linked
account numbers and an optional `reason`. See
[`AccountMatchResult`](../Mono.Core/Services/WebHookService/Models/MonoWebhookModel.cs).

### Step 3 — Handle the webhook

```csharp
public Task HandleAccountCreatedEvent(AccountConnectedEventModel webhook)
{
    // webhook.Id is the account id you'll use for everything else
    return _myApp.RecordLinkedAccountAsync(webhook.Customer, webhook.Id);
}
```

See [webhooks](./webhooks.md) for the full event surface.

### Step 4 — Pull data

```csharp
// Account details (name, number, balance — cached)
var info = await _accounts.GetAccount(accountId);

// Real-time balance (live fetch from the bank; may incur a per-call fee)
var balance = await _accounts.GetAccountBalance(accountId);

// Identity (KYC info attached to the account)
var identity = await _accounts.GetIdentity(accountId);

// Income analysis
var income = await _accounts.GetIncome(accountId);

// Statement (period: "last3months" / "last6months" / "last12months", etc.)
var stmt = await _accounts.GetStatement(accountId, new StatementRequestModels
{
    Period = "last3months",
    Output = "json",
});

// Transactions (with date filters + pagination)
var txns = await _accounts.GetTransactions(accountId, new AccountTransactionsOptionsRequest
{
    Start = "2026-01-01",
    End   = "2026-05-31",
    Paginate = true,
    Limit  = 50,
});
```

## Statement PDF

Set `Output = "pdf"` to get a job id back; poll
`GetPollStatementPdf(accountId, jobId)` until the PDF is ready.

## Customers — full CRUD

```csharp
// Create — see above

// Read one
var customer = await _customers.RetrieveCustomer(customerId);

// List with filters + pagination
var list = await _customers.ListCustomers(new CustomerListQueryOptions
{
    Page      = 1,
    Limit     = 50,
    FirstName = "Ada",
});

// Update (every field is optional)
var updated = await _customers.UpdateCustomer(customerId, new UpdateCustomerModel
{
    Phone   = "+2348099999999",
    Address = "12 Analytical St, Lagos",
});

// Delete
await _customers.DeleteCustomer(customerId);
```

## Transactions by customer

```csharp
var txns = await _customers.GetCustomerTransactions(customerId, new CustomerTransactionsQueryOptions
{
    Period  = "last12months",
    Page    = 1,
    Account = linkedAccountId,  // optional; filters to a single linked account
});
```

## All linked accounts

Mono files this endpoint under Customer even though the path is global
(`/v2/accounts`). Use it to enumerate everything your business has linked:

```csharp
var all = await _customers.FetchAllLinkedAccounts(new LinkedAccountsQueryOptions
{
    Page     = 1,
    Limit    = 50,
    Customer = "cust_5f8d9c",  // optional filter
});
```

## Reauthorization

When an account's data goes stale or a bank revokes the consent, the user
needs to reauthorize. Initiate via `IMonoAuthorization.ReauthorizeAccount` —
returns a fresh `mono_url` to redirect the user to.

## Sync

Force-refresh an account's data without user interaction:

```csharp
await _authorization.SyncAccount(accountId);
```

Mono will pull the latest balance/transactions on the bank side and fire
the `account_updated` webhook when done.

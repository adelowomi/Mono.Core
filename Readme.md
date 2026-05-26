# Mono.Core

Mono.Core is a .NET library that provides services and utilities for Mono accounts management. This library includes extension methods for `IServiceCollection` to easily configure and add Mono services to your application.

## Available Interfaces

<!-- link to all interfaces that have documentation of the methods in them -->

- [IMonoAccounts](#imonoaccounts)
- [IMonoAuthorization](#imonoauthorization)
- [IMonoCustomers](#imonocustomers)
- [IMonoDirectPay](#imonodirectpay)
- [IMonoDisburse](#imonodisburse)
- [IMonoLookUp](#imonolookup)
- [IMonoMiscellaneous](#imonomiscellaneous)
- [IMonoProve](#imonoprove)
- [IMonoWatchlist](#imonowatchlist)

## Configuration

### MonoInitializationOptions

The `MonoInitializationOptions` class is used to configure the Mono services. You can configure the following options:

- `BaseUrl`: The base URL of the Mono API. Default is `https://api.withmono.com`.
- `SecretKey`: The secret key used to authenticate requests to the Mono API.

```csharp
services.AddMono(options =>
{
    options.BaseUrl = "https://api.withmono.com";
    options.SecretKey = "your_secret_key";
});
```

## Installation

To install Mono.Core, you can use the NuGet package manager:

```sh
dotnet add package Mono.Core
```

Usage
Adding Mono Services
To add Mono services to your IServiceCollection, use the AddMono extension method. This method configures the necessary services and dependencies for Mono.

## Usage

### Adding Mono Services

To add all Mono services including accounts, statements, authorization etc. to your `IServiceCollection`, use the `AddMono` extension method. This method configures the necessary services and dependencies for Mono.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Mono.Core;

services.AddMono(options =>
{
    // Configure MonoInitializationOptions here
});
```

## Adding Mono Services Individually

You can also add Mono services individually by calling the `AddMonoAccounts`, `AddMonoStatements`, `AddMonoAuthorization`, etc. extension methods.

```csharp
using Microsoft.Extensions.DependencyInjection;
using Mono.Core;

services.AddMonoAccounts(options =>
{
    // Configure MonoAccountsOptions here
});

services.AddMonoStatements(options =>
{
    // Configure MonoStatementsOptions here
});

services.AddMonoAuthorization(options =>
{
    // Configure MonoAuthorizationOptions here
});
```

### Usage in business logic

Once you have added the Mono services to your `IServiceCollection`, you can inject the services into your business logic classes and use them to interact with the Mono API.

```csharp
using Mono.Core.Accounts;

public class YourService
{
    private readonly IMonoAccountsService _accountsService;

    public YourService(IMonoAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    public async Task GetAccounts()
    {
        var accounts = await _accountsService.GetAccountsAsync();
        // Do something with the accounts
    }
}
```

## Accepting webhooks from Mono

To package includes a predefined controller that you can use to accept webhooks from Mono. To use the controller, add your webhook URL to the Mono dashboard and as follows:

<https://yourdomain.com/api/MonoWebhook/receive>

Then you need to create a class that inherits the IMonoWebhookHandler interface and implement all missing methods.

```csharp
using Mono.Core.Webhooks;

public class YourMonoWebhookHandler : IMonoWebhookConsumer
{
    public async Task HandleAccountCreatedEvent(AccountConnectedEventModel webhook)
    {
        // Handle the webhook
    }

    public async Task HandleAccountUpdatedEvent(AccountUpdatedEventModel webhook)
    {
        // Handle the webhook
    }

    public async Task HandleUnknownEvent(AccountDeletedEventModel webhook)
    {
        // Handle the webhook
    }
}
```

## Interfaces

<!-- interfaces with details of the methods within them -->

Every interface has a corresponding service that implements the interface. The service is responsible for making requests to the Mono API and returning the response.
You can read the mono documentation [here](https://docs.mono.co/api)

### IMonoAccounts

This interface provides methods for managing accounts in Mono.

- `InitiateAccountLinking` This method is to initiate linking an account. Supports Account Match — set `Institution.AccountNumber` and `CheckAccountMatch=true` to have Mono verify the linked account against the expected number, with the result delivered on the `account_updated` webhook.
- `GetAccount` This method provides account information of a specific account .
- `GetAccountBalance` Real-time balance for a connected account (live fetch from the bank; may incur a per-call fee).
- `GetPollStatementPdf` This method is use to retrieve the statement Pdf of an account, when output is set as PDF.
- `GetIncome` This method is use to retrieve income information of a specific account.
- `GetIdentity` This method provides a mini customer identity information.
- `GetStatement` This method is use to get bank statement of a connected financial account.
- `GetTransactions` This method is use to retrive transaction information of a specific account.

### IMonoAuthorization

This interface provides methods for managing authorization in Mono.

- `AuthorizeAccount` This method is use to authorize a specific account.
- `SyncAccount`This method is used to sync specific account manually.
- `ReauthorizeAccount` This method is use to reauthorise a specific previously linked account.

### IMonoCustomers

This interface wraps the Mono Customer API (`/v2/customers`). Use it to model customers in your business and tie linked accounts and payment transactions to them.

- `CreateIndividualCustomer` Creates an individual customer (first name + last name).
- `CreateBusinessCustomer` Creates a business customer (business name + required address/phone).
- `RetrieveCustomer` Fetches a single customer by id.
- `ListCustomers` Lists customers with optional pagination and name/phone/date filters.
- `GetCustomerTransactions` Lists transactions performed by a customer across Mono's payment products. Supports `period`, `page`, and linked-account scoping.
- `FetchAllLinkedAccounts` Lists every bank account linked to your business. Filter by `customer` to scope to one customer.
- `UpdateCustomer` Partially updates a customer; every field on the model is optional.
- `DeleteCustomer` Deletes a customer.

```csharp
using Mono.Core.Customers;

var customer = await _customers.CreateIndividualCustomer(new CreateIndividualCustomerModel
{
    FirstName = "Ada",
    LastName = "Lovelace",
    Email = "ada@example.com",
    Phone = "+2348012345678",
    Identity = new CustomerIdentity
    {
        Type = CustomerIdentityTypeConstants.Bvn,
        Number = "12345678901",
    },
});
```

### IMonoDirectPay

This interface provides methods for managing direct pay in Mono.

- `InitiatePayment` This method is use to initiate a one-time payment
- `VerifyPayment` This method is use to Verify the payment using the reference passed when initiating payment.
- `GetTransactions` This method is use to retrive payment transactions of a specific account.

### IMonoDisburse

This interface wraps the Mono Disburse API (`/v3/payments/disburse/...`). Use it to pay out funds — salary, vendor settlements, marketplace splits, cashback — to one or many recipients from a registered source account.

The flow is: register a source account → create a disbursement batch with one or more distributions → trigger or schedule it.

**Source accounts:**
- `CreateSourceAccount` Registers a funding account.
- `UpdateSourceAccount` Updates a registered source account.
- `FetchAllSourceAccounts` Lists registered source accounts.
- `FetchSourceAccount` Fetches a single source account by id.

**Disbursements (batches):**
- `CreateDisbursement` Creates a batch. Set `type` to `instant` or `scheduled`.
- `CreateInstantDisbursement` / `CreateScheduledDisbursement` Convenience wrappers that set the `type` for you.
- `TransitionDisbursement` Changes a scheduled batch's state — `trigger` to execute now, `cancel` to stop it.
- `FetchAllDisbursements` Lists batches.
- `FetchDisbursement` Fetches a single batch.

**Distributions (recipients within a batch):**
- `AddDistributionsToBatch` Adds one or more recipients to an existing batch.
- `UpdateDistributionInBatch` Edits a recipient (all fields optional).
- `DeleteDistributionInBatch` Removes a recipient from a batch.
- `FetchAllDistributionsInBatch` Lists recipients in a batch.
- `FetchSingleDistribution` Fetches a single recipient by id.

```csharp
using Mono.Core.Disburse;

var disbursement = await _disburse.CreateInstantDisbursement(new CreateDisbursementModel
{
    Reference = "payroll-2026-05",
    Source = DisbursementSourceConstants.Mandate,
    Account = sourceAccountId,
    TotalAmount = 250_000_00,
    Description = "May payroll",
    Distribution = new List<DistributionModel>
    {
        new DistributionModel
        {
            Reference = "payroll-ada",
            RecipientEmail = "ada@example.com",
            Account = new DisbursementRecipientAccount { AccountNumber = "0123456789", BankCode = "044" },
            Amount = 250_000_00,
            Narration = "May salary",
        },
    },
});
```

### IMonoLookUp

This interface provides methods for looking up information in Mono.

- `InitiateBvnLookUp` This method is use to initiate a BVN Consent request.
- `VerifyBvnLookUp` This method is use to verify the BVN request via OTP.
- `GetBvnDetails` This method is use to retrieve BVN Information requested.
- `GetCacLookUp` This method is used to get specific business information is a company.
- `GetCacCompany` This method is use to get official information of an existence business.
- `GetPreviousAddress` *(Deprecated by Mono — retained for source compatibility)*
- `GetChangeOfName` *(Deprecated by Mono — retained for source compatibility)*
- `GetSecretary` This method is used to search for the company secretary.
- `GetDirectors` This method is used to search for the company directors.
- `GetBanks` This method returns NIP supported bank coverage.
- `GetAddress` This method verifies your address via your meter number and house address.
- `GetPassport` This method verifies an international passport document via its passport number and last name.
- `GetNin` This method verifies the national identification number of a user.
- `GetDriverLicense` This method verifies the driver license number of an individual.
- `GetAccountNumber` This method verifies the account and returns the masked BVN attached to the account number supplied.
- `GetCreditHistory` This method enables you to retrieve a user's credit history.
- `GetMashUp` This method allows you to verify the NIN, BVN and date of birth of your user in one API call for KYC.
- `GetNinPdf` Submits a NIN lookup with `output=pdf`. Returns a job id; poll with `PollNinJob`.
- `PollNinJob` Polls a NIN PDF generation job. Returns `processing` until ready, then `completed` with a 7-day-expiry download URL.

### IMonoProve

This interface wraps the Mono Prove API (`/v1/prove/...`) — Mono's full-stack KYC verification (BVN/NIN ownership, government-ID + face match, address verification, optional bank-account linking).

Flow: call `InitiateProve` → redirect the customer to the returned `mono_url` → poll `FetchCustomerDetails` (or wait for the webhook) for verification results.

- `InitiateProve` Starts a verification session. Pick a `KycLevel` (`tier_1`/`tier_2`/`tier_3`/`custom`).
- `FetchCustomerDetails` Pulls the full verification record by reference (identities, face match, address, bank accounts).
- `FetchAllCustomerDetails` Lists every Prove customer with optional filters.
- `BlacklistCustomer` Marks a customer as blacklisted with a reason + code (101-105).
- `WhitelistCustomer` Reinstates a previously blacklisted customer.
- `RevokeDataAccess` Revokes Mono's permission to share this customer's data with your business.

```csharp
using Mono.Core.Prove;

var prove = await _prove.InitiateProve(new InitiateProveModel
{
    Customer = new ProveCustomer
    {
        Name = "Ada Lovelace",
        Phone = "+2348012345678",
        Address = "12 Analytical St, Lagos",
        Email = "ada@example.com",
        Identity = new ProveCustomerIdentity
        {
            Type = ProveIdentityTypeConstants.Bvn,
            Number = "12345678901",
        },
    },
    Reference = "kyc-ada-2026-05",
    RedirectUrl = "https://yourapp.com/kyc/done",
    KycLevel = ProveKycLevelConstants.Tier2,
});

// later, after the customer completes the flow:
var details = await _prove.FetchCustomerDetails("kyc-ada-2026-05");
```

### IMonoWatchlist

This interface wraps the Mono Watchlist Screening API (`/v3/lookup/watchlist/...`) — sanctions, PEP and adverse-media screening with risk scores, batch screening, an audit log per screening, PDF compliance reports, and ongoing monitoring.

- `SubmitIndividualScreening` Screens a single individual.
- `SubmitEntityScreening` Screens a single business / entity.
- `SubmitBatchScreening` Screens multiple subjects in one call.
- `GetScreeningResult` Polls a screening's status, matches and risk score.
- `GetAuditLog` Lifecycle events for a screening.
- `GetScreeningReport` Downloads the PDF compliance report. Returns raw bytes in `Data`.
- `StartMonitoring` Enrolls a subject in continuous monitoring.
- `StopMonitoring` Cancels ongoing monitoring.

```csharp
using Mono.Core.Watchlist;

var screening = await _watchlist.SubmitIndividualScreening(new SubmitIndividualScreeningModel
{
    Name = "Ada Lovelace",
    DateOfBirth = "1815-12-10",
    Gender = "female",
    Bvn = "12345678901",
    Country = "NG",
});

if (screening.Data.RiskLevel == RiskLevelConstants.High)
{
    var report = await _watchlist.GetScreeningReport(screening.Data.Id);
    await File.WriteAllBytesAsync($"screening-{screening.Data.Id}.pdf", report.Data);
}
```

### IMonoMiscellaneous

This interface provides miscellaneous methods for managing Mono.

- `GetCoverage` This method provides bank coverage across supported institutions and product scopes
- `GetCacLookup` This method to retieve cac lookup information.
- `GetCacCompany` This method is use to retrieve shareholder information of a company.
- `UnLinkAccount` This method provide you with the option to unlink their financial account(s).

## Changes in 1.6.0 (May 2026)

Connect additions — fills gaps in existing services rather than adding new product surfaces.

**`IMonoAccounts`:**
- `GetAccountBalance` (GET `/accounts/{id}/balance`) — real-time balance fetch
- `AccountLinkingModel` gains `Institution` and `CheckAccountMatch` for the Feb 2026 Account Match feature; result arrives on the existing `account_updated` webhook
- `AccountUpdatedEventModel` gains `AccountMatch` (status, matched bool, expected vs linked account numbers, reason)

**`IMonoLookUp`:**
- `GetNinPdf` (POST `/lookup/nin` with `output=pdf`) — async NIN lookup returning a job id
- `PollNinJob` (GET `/lookup/nin/{jobId}/job`) — poll status; completed jobs include a 7-day download URL

**Constants:**
- `NinOutputConstants` (`json` / `pdf`)
- `NinJobStatusConstants` (`processing` / `completed` / `failed`)

**Not duplicated:** Get-All-Accounts is already exposed as `IMonoCustomers.FetchAllLinkedAccounts` (v1.2.0). Mono files that endpoint under Customer; the path is `/accounts`.

## Changes in 1.5.0 (May 2026)

Adds the Mono Prove API surface — full-stack KYC verification (BVN/NIN ownership, government-ID + face match, address verification, optional bank-account linking).

**New `IMonoProve` interface — 6 endpoints under `/v1/prove/...`:**
- `InitiateProve` (POST `/prove/initiate`)
- `FetchCustomerDetails` (GET `/prove/customers/{reference}`)
- `FetchAllCustomerDetails` (GET `/prove/customers`)
- `BlacklistCustomer` (POST `/prove/customers/blacklist`)
- `WhitelistCustomer` (POST `/prove/customers/whitelist`)
- `RevokeDataAccess` (DELETE `/prove/customers/{reference}`)

**Infrastructure:**
- `IRefitClientBuilder<T>` gains `BuildV1(string)` — Prove still lives under `/v1/`. Pattern matches the existing `BuildV3` helper (rewrites the `/v2/` in `BaseUrl` to `/v1/`).

**Registration:**
- `AddMono(...)` now also wires up `IMonoProve`
- Standalone `AddMonoProve(...)` extension available

**Constants:**
- `ProveKycLevelConstants` (`tier_1` / `tier_2` / `tier_3` / `custom`)
- `ProveIdentityTypeConstants` (`bvn` / `nin`)
- `ProveBlacklistCodeConstants` (101-105 — passed through verbatim; Mono hasn't published a named mapping)

## Changes in 1.4.0 (May 2026)

Adds the Mono Watchlist Screening API surface (released by Mono in March 2026). Watchlist Screening matches subjects against sanctions, PEP and adverse-media lists, returns a risk score, and can run ongoing monitoring.

**New `IMonoWatchlist` interface — 7 endpoints under `/v3/lookup/watchlist/...`:**
- `SubmitIndividualScreening` (POST `/lookup/watchlist`, `type=individual`)
- `SubmitEntityScreening` (POST `/lookup/watchlist`, `type=entity`)
- `SubmitBatchScreening` (POST `/lookup/watchlist/batch`)
- `GetScreeningResult` (GET `/lookup/watchlist/{id}`)
- `GetAuditLog` (GET `/lookup/watchlist/{id}/audit-log`)
- `GetScreeningReport` (GET `/lookup/watchlist/{id}/report` — binary PDF, returned as `byte[]`)
- `StartMonitoring` (POST `/lookup/watchlist/monitor`)
- `StopMonitoring` (DELETE `/lookup/watchlist/monitor/{id}`)

**Registration:**
- `AddMono(...)` now also wires up `IMonoWatchlist`
- Standalone `AddMonoWatchlist(...)` extension available

**Constants:**
- `WatchlistSubjectTypeConstants` (`individual` / `entity`)
- `ScreeningStatusConstants` (`processing` / `completed` / `failed`)
- `RiskLevelConstants` (`low` / `medium` / `high`)

## Changes in 1.3.0 (May 2026)

Adds the Mono Disburse API surface (added by Mono in September 2025). Disburse handles outbound payments — salary, vendor settlements, marketplace splits, cashback — to one or many recipients from a registered source account.

**New `IMonoDisburse` interface — 13 endpoints under `/v3/payments/disburse/...`:**

*Source accounts:*
- `CreateSourceAccount` (POST `/payments/disburse/source-accounts`)
- `UpdateSourceAccount` (PUT `/payments/disburse/source-accounts`)
- `FetchAllSourceAccounts` (GET `/payments/disburse/source-accounts`)
- `FetchSourceAccount` (GET `/payments/disburse/source-accounts/{id}`)

*Disbursements (batches):*
- `CreateDisbursement` (POST `/payments/disburse/disbursements`)
- `CreateInstantDisbursement` / `CreateScheduledDisbursement` (convenience wrappers)
- `TransitionDisbursement` (POST `/payments/disburse/disbursements/{id}/transition`)
- `FetchAllDisbursements` (GET `/payments/disburse/disbursements`)
- `FetchDisbursement` (GET `/payments/disburse/disbursements/{id}`)

*Distributions:*
- `AddDistributionsToBatch` (POST `/payments/disburse/disbursements/{id}/distributions`)
- `UpdateDistributionInBatch` (PATCH `/payments/disburse/disbursements/{id}/distributions/{distId}`)
- `DeleteDistributionInBatch` (DELETE `/payments/disburse/disbursements/{id}/distributions/{distId}`)
- `FetchAllDistributionsInBatch` (GET `/payments/disburse/disbursements/{id}/distributions`)
- `FetchSingleDistribution` (GET `/payments/disburse/disbursements/{id}/distributions/{distId}`)

**Registration:**
- `AddMono(...)` now also wires up `IMonoDisburse`
- Standalone `AddMonoDisburse(...)` extension available

**Constants:**
- `DisbursementTypeConstants` (`instant` / `scheduled`)
- `DisbursementSourceConstants` (`mandate`)
- `TransitionActionConstants` (`trigger` / `cancel`)

## Changes in 1.2.0 (May 2026)

Adds the Mono Customer API surface. Customers let you model your end users, attach linked bank accounts to them, and pull transaction history per customer.

**New `IMonoCustomers` interface — eight endpoints under `/v2/customers`:**
- `CreateIndividualCustomer` (POST `/customers`, `type=individual`)
- `CreateBusinessCustomer` (POST `/customers`, `type=business`)
- `RetrieveCustomer` (GET `/customers/{id}`)
- `ListCustomers` (GET `/customers`, paginated)
- `GetCustomerTransactions` (GET `/customers/{id}/transactions`)
- `FetchAllLinkedAccounts` (GET `/accounts` — Mono files this under Customer)
- `UpdateCustomer` (PATCH `/customers/{id}`, all fields optional)
- `DeleteCustomer` (DELETE `/customers/{id}`)

**Registration:**
- `AddMono(...)` now also wires up `IMonoCustomers`
- Standalone `AddMonoCustomers(...)` extension available for service-by-service registration

**Constants:**
- `CustomerTypeConstants` (`individual` / `business`)
- `CustomerIdentityTypeConstants` (`bvn` / `nin`)

## Changes in 1.1.0 (May 2026)

Drift-only refresh against the current Mono docs. Future product surfaces (Customers, Disburse, Watchlist, Prove, transaction enrichment) will land in follow-up releases.

**Endpoints fixed:**
- BVN: `/lookup/bvn/verify` → `/lookup/bvn/verify-otp`
- BVN: `/lookup/bvn/details` → `/lookup/bvn/fetch-bvn`
- BVN `scope` values now lowercase (`identity`, `bank_accounts`) to match the API
- Lookups for address, passport, TIN, NIN, drivers-license, account-number, credit-history, mashup are now `POST` (they were declared `[Get]` with `[Body]` — invalid in Refit and inconsistent with the docs)
- Driver's license path: `/lookup/driver_license` → `/lookup/drivers-license`
- International passport path: `/lookup/passport` → `/lookup/intl-passport`
- Mandate balance enquiry: `amount` query parameter is now optional. Present = sufficient-funds check (NGN 10), absent = real-time balance (NGN 50)

**Deprecated (kept for source compatibility, marked `[Obsolete]`):**
- `IMonoLookUp.GetPreviousAddress` — Mono retired the CAC previous-address endpoint
- `IMonoLookUp.GetChangeOfName` — Mono retired the CAC change-of-name endpoint

**Mandate creation:**
- `CreateMandateModel` adds `fee_bearer`, `verification_method`, and `meta` fields per the v3 docs
- New string constants: `MandateTypeConstants` (emandate/sweep), `DebitTypeConstants` (variable/fixed), `FeeBearerConstants` (business/customer), `VerificationMethodConstants` (transfer_verification/selfie_verification)

**Security / dependencies:**
- Refit bumped to `7.2.22` to address [CVE-2024-51501](https://github.com/advisories/GHSA-3hxg-fxwm-8gf7) (CRLF header injection)
- Removed unused `Moq` and `Newtonsoft.Json.Bson` from the runtime package; `Moq` moved to the test project where it belongs

## Contributing

Contributions are welcome! For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)

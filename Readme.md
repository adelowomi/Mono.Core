# Mono.Core

Mono.Core is a .NET library that provides services and utilities for Mono accounts management. This library includes extension methods for `IServiceCollection` to easily configure and add Mono services to your application.

## Available Interfaces

<!-- link to all interfaces that have documentation of the methods in them -->

- [IMonoAccounts](#imonoaccounts)
- [IMonoAuthorization](#imonoauthorization)
- [IMonoDirectPay](#imonodirectpay)
- [IMonoLookUp](#imonolookup)
- [IMonoMiscellaneous](#imonomiscellaneous)

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

- `InitiateAccountLinking` This method is to initiate linking an account.
- `GetAccount` This method provides account information of a specific account .
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

### IMonoDirectPay

This interface provides methods for managing direct pay in Mono.

- `InitiatePayment` This method is use to initiate a one-time payment
- `VerifyPayment` This method is use to Verify the payment using the reference passed when initiating payment.
- `GetTransactions` This method is use to retrive payment transactions of a specific account.

### IMonoLookUp

This interface provides methods for looking up information in Mono.

- `InitiateBvnLookUp` This method is use to initiate a BVN Consent request.
- `VerifyBvnLookUp` This method is use to verify the BVN request via OTP.
- `GetBvnDetails` This method is use to retrieve BVN Information requested.
- `GetCacLookUp` This method is used to get specific business information is a company.
- `GetCacCompany` This method is use to get official information of an existence business.
- `GetPreviousAddress` This method is use to check previous company address.
- `GetChangeOfName` This method is use to check the history of a business name change.
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

### IMonoMiscellaneous

This interface provides miscellaneous methods for managing Mono.

- `GetCoverage` This method provides bank coverage across supported institutions and product scopes
- `GetCacLookup` This method to retieve cac lookup information.
- `GetCacCompany` This method is use to retrieve shareholder information of a company.
- `UnLinkAccount` This method provide you with the option to unlink their financial account(s).

## Contributing

Contributions are welcome! For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)

# Mono.Core

Mono.Core is a .NET library that provides services and utilities for Mono accounts management. This library includes extension methods for `IServiceCollection` to easily configure and add Mono services to your application.

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

https://yourdomain.com/api/MonoWebhook/receive

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



## Contributing

Contributions are welcome! For major changes, please open an issue first to discuss what you would like to change.
Please make sure to update tests as appropriate.

## License

[MIT](https://choosealicense.com/licenses/mit/)
```


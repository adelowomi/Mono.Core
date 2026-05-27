# Getting started

A 10-minute walkthrough that gets a typical ASP.NET Core app talking to Mono.

## 1. Install

```sh
dotnet add package Mono.Core
```

Targets `netstandard2.0` so it works with .NET Framework 4.6.1+, .NET Core
2.0+, .NET 5+, and Mono/Xamarin.

## 2. Get your secret key

1. Sign in at [app.mono.co](https://app.mono.co/apps)
2. Create an app for the product family you need (Connect, DirectPay,
   Disburse, Lookup, or Prove)
3. Copy the secret key (starts with `live_sk_` or `test_sk_`)

> Mono apps are scoped to one product. If you need more than one product,
> create one app per product and configure separate keys — see
> [Configuration](./configuration.md#per-product-keys).

## 3. Register the services

In `Program.cs` (or wherever you wire up DI):

```csharp
using Mono.Core;

builder.Services.AddMono(options =>
{
    options.BaseUrl = "https://api.withmono.com/v2";
    options.SecretKey = builder.Configuration["Mono:SecretKey"];
    options.WebhookSecret = builder.Configuration["Mono:WebhookSecret"];
});
```

`AddMono` registers every product interface (`IMonoAccounts`,
`IMonoCustomers`, `IMonoDirectPay`, `IMonoDisburse`, `IMonoLookUp`,
`IMonoWatchlist`, `IMonoProve`, etc.). If you only need one product use the
service-specific extension instead — e.g. `AddMonoCustomers(...)`,
`AddMonoLookUp(...)`.

## 4. Make your first call

Inject the interface and call it. Every method returns
`MonoStandardResponse<T>` — see [Error handling](./error-handling.md).

```csharp
public class OnboardingService
{
    private readonly IMonoCustomers _customers;

    public OnboardingService(IMonoCustomers customers) => _customers = customers;

    public async Task<string> CreateCustomerAsync(string firstName, string lastName, string bvn)
    {
        var response = await _customers.CreateIndividualCustomer(new CreateIndividualCustomerModel
        {
            FirstName = firstName,
            LastName = lastName,
            Email = $"{firstName.ToLower()}@example.com",
            Phone = "+2348012345678",
            Identity = new CustomerIdentity
            {
                Type = CustomerIdentityTypeConstants.Bvn,
                Number = bvn,
            },
        });

        if (!response.Success)
        {
            throw new InvalidOperationException(response.Message);
        }

        return response.Data.Id;
    }
}
```

## 5. (Recommended) Accept webhooks

Mono webhooks ship account updates, mandate state changes, disbursement
results, watchlist matches, KYC results, and more. The library includes a
preconfigured controller at `POST /api/MonoWebhook/receive` with built-in
`mono-webhook-secret` verification.

Implement the consumer interface:

```csharp
public class MyWebhookHandler : IMonoWebhookConsumer
{
    public Task HandleAccountCreatedEvent(AccountConnectedEventModel webhook) => Task.CompletedTask;
    public Task HandleAccountUpdatedEvent(AccountUpdatedEventModel webhook) => Task.CompletedTask;
    public Task HandleMandateCreatedEvent(MandateCreatedEventModel webhook) => Task.CompletedTask;
    public Task HandleMandateApprovedEvent(MandateApprovedEventModel webhook) => Task.CompletedTask;
    public Task HandleMandateReadyEvent(MandateReadyEventModel webhook) => Task.CompletedTask;
    public Task HandleUnknownEvent(string json) => Task.CompletedTask;
}
```

Register it before `AddMono(...)`:

```csharp
builder.Services.AddScoped<IMonoWebhookConsumer, MyWebhookHandler>();
builder.Services.AddMono(options => { /* ... */ });
```

Then point Mono's dashboard at `https://yourdomain.com/api/MonoWebhook/receive`.

Full webhook guide including signature verification, all event types and the
optional `IMonoWebhookConsumerExtensions` for newer events:
[webhooks](./webhooks.md).

## Where to go from here

| Need | Doc |
|---|---|
| Configure base URLs, per-product keys, webhook secret | [configuration](./configuration.md) |
| Understand `MonoStandardResponse<T>` and errors | [error-handling](./error-handling.md) |
| Bank-account linking, statements, balance, transactions | [connect](./connect.md) |
| One-time payments, mandates, refunds, payouts | [direct-pay](./direct-pay.md) |
| Outbound payouts to one or many recipients | [disburse](./disburse.md) |
| BVN, NIN, CAC, TIN, drivers-license, mashup | [lookup](./lookup.md) |
| Sanctions / PEP / adverse-media screening | [watchlist](./watchlist.md) |
| Full-stack KYC verification (tier 1/2/3) | [prove](./prove.md) |
| Upgrading from an earlier version | [migration-guide](./migration-guide.md) |

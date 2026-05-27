# Mono.Core

A .NET client library for the [Mono](https://mono.co) API — Connect,
DirectPay, Disburse, Lookup (BVN / NIN / CAC / TIN / watchlist), Prove KYC,
and a webhook controller with signature verification.

[![NuGet](https://img.shields.io/nuget/v/Mono.Core.svg)](https://www.nuget.org/packages/Mono.Core/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Targets `netstandard2.0` — works on .NET Framework 4.6.1+, .NET Core 2.0+,
.NET 5+, and Mono/Xamarin.

## Install

```sh
dotnet add package Mono.Core
```

## Configure

```csharp
using Mono.Core;

services.AddMono(options =>
{
    options.BaseUrl = "https://api.withmono.com/v2";
    options.SecretKey = "your_secret_key";       // from app.mono.co/apps
    options.WebhookSecret = "your_webhook_secret"; // recommended
});
```

Need product-scoped keys (DirectPay + Disburse + Prove are separate apps
on Mono's dashboard)? See [docs/configuration.md](./docs/configuration.md).

## Use

Inject any interface, every method returns `MonoStandardResponse<T>`:

```csharp
public class OnboardingService
{
    private readonly IMonoCustomers _customers;

    public OnboardingService(IMonoCustomers customers) => _customers = customers;

    public async Task<string> CreateAsync(string bvn)
    {
        var response = await _customers.CreateIndividualCustomer(new CreateIndividualCustomerModel
        {
            FirstName = "Ada",
            LastName = "Lovelace",
            Email = "ada@example.com",
            Phone = "+2348012345678",
            Identity = new CustomerIdentity { Type = "bvn", Number = bvn },
        });

        if (!response.Success) throw new InvalidOperationException(response.Message);
        return response.Data.Id;
    }
}
```

## Documentation

| Guide | What it covers |
|---|---|
| [Getting started](./docs/getting-started.md) | 10-minute walkthrough — install → call → webhook |
| [Configuration](./docs/configuration.md) | Per-product keys, base URLs, webhook secret, DI registration |
| [Error handling](./docs/error-handling.md) | `MonoStandardResponse<T>`, when things fail, what to log |
| [Webhooks](./docs/webhooks.md) | Signature verification, full event reference, idempotency |
| [Connect](./docs/connect.md) | `IMonoAccounts` + `IMonoCustomers` (account linking, balance, statements, transactions) |
| [DirectPay](./docs/direct-pay.md) | One-time payments, mandates, refunds, payouts, sub-accounts |
| [Disburse](./docs/disburse.md) | Outbound payouts to one or many recipients |
| [Lookup](./docs/lookup.md) | BVN, NIN, CAC, TIN, drivers-license, mashup, credit history |
| [Watchlist](./docs/watchlist.md) | Sanctions / PEP / adverse-media screening + monitoring |
| [Prove](./docs/prove.md) | Full-stack KYC verification (tier 1/2/3) |
| [Migration guide](./docs/migration-guide.md) | Upgrading from an earlier version |
| [Changelog](./CHANGELOG.md) | Per-release notes |

## Interfaces at a glance

| Service | Purpose | Mono product |
|---|---|---|
| `IMonoAccounts` | Account linking, balance, statements, transactions | Connect |
| `IMonoCustomers` | Customer modeling, list/CRUD, linked-account enumeration | Connect |
| `IMonoAuthorization` | Authorize / reauthorize / sync linked accounts | Connect |
| `IMonoDirectPay` | One-time payments, mandates, refunds, payouts, sub-accounts | DirectPay |
| `IMonoDisburse` | Outbound payouts (source accounts → batches → distributions) | Disburse |
| `IMonoLookUp` | BVN, NIN, CAC, TIN, drivers-license, address, mashup, credit history | Lookup |
| `IMonoWatchlist` | Sanctions / PEP screening, ongoing monitoring | Lookup |
| `IMonoProve` | Full-stack KYC verification | Prove |
| `IMonoMiscellaneous` | Coverage, unlink account | misc |

## Contributing

Issues + PRs welcome. For substantial changes, open an issue first so we
can align on the approach. Keep PRs focused — one product surface or one
bug per PR makes review tractable.

Run the test suite locally:

```sh
dotnet test
```

The suite has four layers — unit, JSON contract, Refit HTTP wire, and
optional sandbox integration. The sandbox layer is gated behind environment
variables and skips automatically when they're not set:

```sh
export MONO_SANDBOX_KEY="test_sk_..."
export MONO_SANDBOX_LOOKUP_KEY="test_sk_..."  # optional, for Lookup endpoints
export MONO_SANDBOX_ACCOUNT_ID="..."           # optional, for account-data tests
dotnet test
```

## License

[MIT](https://choosealicense.com/licenses/mit/)

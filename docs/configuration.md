# Configuration

`MonoInitializationOptions` is configured via the `AddMono(...)` extension (or
any of the per-service `AddMonoXxx(...)` extensions). All settings live on
this single class.

## Quick reference

```csharp
services.AddMono(options =>
{
    options.BaseUrl = "https://api.withmono.com/v2";   // required
    options.SecretKey = "fallback_key";                 // required (or use a per-product key for everything you call)

    // Per-product keys (all optional; each falls back to SecretKey)
    options.ConnectSecretKey   = "connect_app_key";
    options.LookupSecretKey    = "lookup_app_key";
    options.DirectPaySecretKey = "directpay_app_key";
    options.DisburseSecretKey  = "disburse_app_key";
    options.ProveSecretKey     = "prove_app_key";

    // Webhook signature verification (recommended in production)
    options.WebhookSecret = "your_webhook_secret";
});
```

## `BaseUrl`

Always set this to the **v2** root — `https://api.withmono.com/v2`. The
library auto-rewrites the `v2` segment to `v3` or `v1` per endpoint, since
Mono's API surface is split across all three versions (BVN is on v2, NIN/CAC
on v3, Prove on v1).

For sandbox testing, use the same URL — Mono switches between sandbox and
live mode based on whether your secret key starts with `test_sk_` or
`live_sk_`.

## `SecretKey`

The default. Every service that doesn't have a more specific override falls
back to this. The minimum required setting.

If your Mono app is multi-product (rare on Mono's current dashboard) you can
just set `SecretKey` and skip every override.

## Per-product keys

Mono's dashboard scopes every app to one product family. If you call a Mono
endpoint with a key that belongs to the wrong product, Mono returns:

> *"Please use a Lookup app, go to your dashboard and create an app with
> Lookup product."*

(or `Connect`, `DirectPay`, etc. depending on the endpoint). To support this,
each library service routes through a product-specific key:

| Service | Key used |
|---|---|
| `IMonoAccounts` | `ConnectSecretKey ?? SecretKey` |
| `IMonoCustomers` | `ConnectSecretKey ?? SecretKey` |
| `IMonoLookUp` | `LookupSecretKey ?? SecretKey` |
| `IMonoWatchlist` | `LookupSecretKey ?? SecretKey` (same family) |
| `IMonoDirectPay` | `DirectPaySecretKey ?? SecretKey` |
| `IMonoDisburse` | `DisburseSecretKey ?? SecretKey` |
| `IMonoProve` | `ProveSecretKey ?? SecretKey` |
| `IMonoAuthorization` | `SecretKey` (no dedicated override; shared with Connect family) |
| `IMonoMiscellaneous` | `SecretKey` |

**Recommended setup for production:**

```csharp
services.AddMono(options =>
{
    options.BaseUrl = "https://api.withmono.com/v2";
    options.ConnectSecretKey   = config["Mono:Connect"];
    options.LookupSecretKey    = config["Mono:Lookup"];
    options.DirectPaySecretKey = config["Mono:DirectPay"];
    // ... set whichever products you actually use
    // Don't set SecretKey — that way any call to an unconfigured product
    // fails with a null-key error during a smoke test, not silently in prod.
});
```

## `WebhookSecret`

Shared secret you configure on the Mono dashboard. When set, the library's
webhook controller rejects any inbound POST whose `mono-webhook-secret`
header doesn't match (constant-time comparison; returns HTTP 401).

When unset, the controller logs a warning and processes the request anyway —
opt-in to preserve backward compatibility, but **you should set this in
production**. Without it, anyone who knows your webhook URL can POST fake
events.

Full webhook guide: [webhooks](./webhooks.md).

## Configuration sources

The options class is a plain POCO. Bind it from any `IConfiguration` source:

```csharp
// appsettings.json
{
    "Mono": {
        "BaseUrl": "https://api.withmono.com/v2",
        "ConnectSecretKey": "...",
        "LookupSecretKey": "...",
        "WebhookSecret": "..."
    }
}

// Program.cs
services.AddMono(options => builder.Configuration.GetSection("Mono").Bind(options));
```

Or pull from environment variables, Azure Key Vault, AWS Secrets Manager —
whatever your standard secrets pipeline uses. Keys should not live in source
control.

## Service registration

`AddMono(...)` registers everything. If you only need a subset, use the
per-service extensions — each one configures the same `MonoInitializationOptions`
but only wires up its own service:

```csharp
services.AddMonoAccounts(options => { /* ... */ });
services.AddMonoLookUp(options => { /* ... */ });
services.AddMonoCustomers(options => { /* ... */ });
services.AddMonoDirectPay(options => { /* ... */ });
services.AddMonoDisburse(options => { /* ... */ });
services.AddMonoWatchlist(options => { /* ... */ });
services.AddMonoProve(options => { /* ... */ });
services.AddMonoAuthorization(options => { /* ... */ });
services.AddMonoMiscellaneous(options => { /* ... */ });
```

Calling `Configure<MonoInitializationOptions>(...)` more than once **does
not** merge — the last call wins. If you need different keys for different
services in the same process, register the library more than once in
separate scopes or use named options (advanced; not directly supported,
needs a custom `IRefitClientBuilder<T>`).

# Webhooks

The library ships an opinionated webhook receiver at
`POST /api/MonoWebhook/receive`. Implement one interface, set the secret,
point Mono's dashboard at the URL, done.

## Set up

### 1. Implement `IMonoWebhookConsumer`

This is the "core" event surface — account linking, mandate
created/approved/ready, plus a catch-all for unknown events.

```csharp
public class MyWebhookHandler : IMonoWebhookConsumer
{
    private readonly IMyAppService _app;
    public MyWebhookHandler(IMyAppService app) => _app = app;

    public Task HandleAccountCreatedEvent(AccountConnectedEventModel webhook)
        => _app.LinkAccountAsync(webhook.Id, webhook.Customer);

    public Task HandleAccountUpdatedEvent(AccountUpdatedEventModel webhook)
        => _app.UpdateAccountAsync(webhook.Account.Id, webhook.Account.Balance);

    public Task HandleMandateCreatedEvent(MandateCreatedEventModel webhook)
        => _app.MandateCreatedAsync(webhook.Id);

    public Task HandleMandateApprovedEvent(MandateApprovedEventModel webhook)
        => _app.MandateApprovedAsync(webhook.Id);

    public Task HandleMandateReadyEvent(MandateReadyEventModel webhook)
        => _app.MandateReadyAsync(webhook.Id);

    public Task HandleUnknownEvent(string json)
    {
        // Log it; this is also where new event types arrive if you haven't
        // implemented IMonoWebhookConsumerExtensions yet.
        _log.LogWarning("Unhandled Mono webhook: {Json}", json);
        return Task.CompletedTask;
    }
}
```

### 2. Register it and the library

```csharp
services.AddScoped<IMonoWebhookConsumer, MyWebhookHandler>();
services.AddMono(options =>
{
    options.SecretKey = "...";
    options.WebhookSecret = "..."; // strongly recommended
});
```

The order doesn't matter — both must be registered for the controller to
resolve at request time.

### 3. Tell Mono where to send events

In the Mono dashboard, set your webhook URL to:

```
https://yourdomain.com/api/MonoWebhook/receive
```

The route prefix is `api` because the controller declares
`[Route("api/[controller]")]`. If your app uses a different convention you'll
need to add a routing rule or subclass the controller — see [Customisation](#customisation).

## Signature verification

The library checks the `mono-webhook-secret` header against
`MonoInitializationOptions.WebhookSecret` using a constant-time string
comparison. The comparison is constant-time because `WebhookSecret` is a
real secret (high entropy, attacker-controlled headers, timing-attack
surface).

### Behavior matrix

| `WebhookSecret` set? | Header present? | Header matches? | Result |
|---|---|---|---|
| ✅ | ✅ | ✅ | 200, handler invoked |
| ✅ | ✅ | ❌ | **401 Unauthorized**, handler not invoked |
| ✅ | ❌ | — | **401 Unauthorized**, handler not invoked |
| ❌ | any | any | 200, handler invoked, warning logged |

**The "unset" behavior is opt-in for backward compatibility.** When the
controller boots without `WebhookSecret`, it logs:

> *MonoInitializationOptions.WebhookSecret is not configured; webhook
> signature verification is disabled. Set the WebhookSecret to enforce
> verification of the mono-webhook-secret header.*

This warning is loud enough that production operators should notice it. If
you want to fail closed instead of warning, subclass the controller (see
[Customisation](#customisation)).

## Newer event types — `IMonoWebhookConsumerExtensions`

Mono shipped a bunch of events after the original `IMonoWebhookConsumer`
interface was set: mandate debit success/failure, account income,
creditworthiness, disbursement lifecycle, watchlist matches, Prove
verification results. To preserve backward compatibility, these live on a
separate optional interface:

```csharp
public class MyWebhookHandler : IMonoWebhookConsumer, IMonoWebhookConsumerExtensions
{
    // existing methods unchanged ...

    public Task HandleMandateDebitSuccessfulEvent(MandateDebitEventModel webhook) => /* ... */;
    public Task HandleMandateDebitFailedEvent(MandateDebitEventModel webhook) => /* ... */;
    public Task HandleAccountIncomeEvent(AccountIncomeEventModel webhook) => /* ... */;
    public Task HandleCreditworthinessEvent(CreditworthinessEventModel webhook) => /* ... */;
    public Task HandleDisbursementEvent(DisbursementEventModel webhook) => /* ... */;
    public Task HandleWatchlistMatchEvent(WatchlistMatchEventModel webhook) => /* ... */;
    public Task HandleProveCompletedEvent(ProveEventModel webhook) => /* ... */;
    public Task HandleProveFailedEvent(ProveEventModel webhook) => /* ... */;
}
```

Implement only the ones you care about; the rest can be `Task.CompletedTask`.

Events your consumer doesn't implement fall through to
`HandleUnknownEvent(string json)` — exactly the same behavior as before this
interface existed.

## Full event reference

Constants live on `MonoEventTypes`. Each value is the full event string Mono
sends minus the `mono.events.` prefix.

### Core (`IMonoWebhookConsumer`)

| Constant | Event string | Payload model |
|---|---|---|
| `AccountConnected` | `account_connected` | `AccountConnectedEventModel` |
| `AccountUpdated` | `account_updated` | `AccountUpdatedEventModel` |
| `MandateCreated` | `mandate.created` | `MandateCreatedEventModel` |
| `MandateApproved` | `mandate.approved` | `MandateApprovedEventModel` |
| `MandateReady` | `mandate.ready` | `MandateReadyEventModel` |
| `WebhookTestEvent` | `webhook_test` | (handled internally — logged only) |

### Newer (`IMonoWebhookConsumerExtensions`)

| Constant | Event string | Payload model |
|---|---|---|
| `MandateDebitSuccessful` | `mandate.debit.successful` | `MandateDebitEventModel` |
| `MandateDebitFailed` | `mandate.debit.failed` | `MandateDebitEventModel` |
| `AccountIncome` | `account_income` | `AccountIncomeEventModel` |
| `Creditworthiness` | `account_creditworthiness` | `CreditworthinessEventModel` |
| `DisbursementInitiated`/`Processing`/`Completed`/`Failed` | `disbursement.*` | `DisbursementEventModel` |
| `WatchlistMatchFound` | `watchlist.match_found` | `WatchlistMatchEventModel` |
| `WatchlistMonitoringUpdate` | `watchlist.monitoring_update` | `WatchlistMatchEventModel` |
| `ProveCompleted` | `prove.completed` | `ProveEventModel` |
| `ProveFailed` | `prove.failed` | `ProveEventModel` |

## Envelope fields

Every payload arrives wrapped in `MonoWebhookModel<T>`:

```csharp
public class MonoWebhookModel<T>
{
    public string Event { get; set; }       // "mono.events.account_connected"
    public string EventId { get; set; }     // stable per-event id — use for idempotency
    public DateTime? Timestamp { get; set; }
    public string App { get; set; }
    public string Business { get; set; }
    public T Data { get; set; }              // typed payload
}
```

The controller uses `EventId` for nothing — your handler should, if you care
about exactly-once processing. Mono retries failed webhooks; the same event
can arrive multiple times.

## Idempotency

The library does not deduplicate by itself. Best practice:

```csharp
public async Task HandleAccountCreatedEvent(AccountConnectedEventModel webhook)
{
    // Stash event_id in a unique-constraint table or a Redis SET first
    if (await _idempotency.AlreadyProcessedAsync(eventId)) return;
    await _app.LinkAccountAsync(webhook.Id);
    await _idempotency.MarkProcessedAsync(eventId);
}
```

The controller returns 200 unconditionally on success. If your handler
throws, the request fails with 500 and Mono retries — that's a feature.
Make handlers idempotent.

## Customisation

The `MonoWebhookController` is a normal ASP.NET Core controller. If you need
a different route, signature behavior, or anything else, subclass it:

```csharp
[ApiController]
[Route("hooks/[controller]")]
public class CustomMonoWebhookController : MonoWebhookController
{
    public CustomMonoWebhookController(
        IMonoWebhookConsumer consumer,
        ILogger<MonoWebhookController> logger,
        IOptions<MonoInitializationOptions> options)
        : base(consumer, logger, options) { }
}
```

(Override `ReceiveWebhookEvent` if you want different status codes, custom
authentication, etc.)

## Routing internals

For curiosity / debugging: the controller strips the `mono.events.` prefix
from the incoming `event` field and matches the remainder against
`MonoEventTypes` constants. Anything that doesn't match a known constant
falls through to `HandleUnknownEvent(string json)`.

Pre-v1.7.0 this was buggy — `Split('.')[2]` always returned `"mandate"` for
mandate events and compared against `"created"`, so every mandate webhook
fell through. Fixed; the test suite has a specific regression case for it.

## Testing your handler

Unit-test by calling the controller directly with a typed `GenericWebHookModel`:

```csharp
[Fact]
public async Task HandlesAccountConnected()
{
    var consumer = new Mock<IMonoWebhookConsumer>();
    var options = Options.Create(new MonoInitializationOptions());
    var controller = new MonoWebhookController(consumer.Object, NullLogger<MonoWebhookController>.Instance, options);
    controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

    var result = await controller.ReceiveWebhookEvent(new GenericWebHookModel
    {
        Event = "mono.events.account_connected",
        Data = new AccountConnectedEventModel { Id = "acc_1", Customer = "cust_1" },
    });

    Assert.IsType<OkResult>(result);
    consumer.Verify(c => c.HandleAccountCreatedEvent(It.IsAny<AccountConnectedEventModel>()), Times.Once);
}
```

For integration, POST raw JSON to your local app with the right header set —
the library's own test suite has examples in `Mono.Core.Tests/Controllers/WebhookRawJsonTests.cs`.

# Error handling

Every public method on every `IMono*` interface returns a
`MonoStandardResponse<T>`. This page explains what's in it and how to handle
failures.

## The envelope

```csharp
public class MonoStandardResponse<T>
{
    public bool Success { get; set; }
    public DateTime Timestamp { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
    public T Data { get; set; }
    public Exception InAppErrors { get; set; }
    public MonoErrors[] MonoErrors { get; set; }
}

public class MonoErrors
{
    public string Field { get; set; }
    public string Message { get; set; }
}
```

| Field | Populated when | What it means |
|---|---|---|
| `Success` | always | `true` if Mono returned a 2xx; `false` otherwise |
| `Status` | always | Echo of Mono's `status` field (e.g. `"successful"`, `"failed"`) |
| `Message` | failures | Human-readable error message |
| `Data` | most successes | The typed response body |
| `MonoErrors` | validation failures | Field-level error array |
| `InAppErrors` | network / parse failures | The `.NET` exception that occurred |
| `Timestamp` | always | When the response was processed |

## The happy path

```csharp
var response = await _customers.RetrieveCustomer("cust_1");
if (response.Success)
{
    Console.WriteLine($"Found: {response.Data.FirstName} {response.Data.LastName}");
}
```

## Handling errors

There are three kinds of failure to handle, in order of frequency:

### 1. Validation / business errors (Mono returned a 4xx)

Mono's response is parsed and `Success` is `false`. `Message` describes the
problem; `MonoErrors` may have field-level detail:

```csharp
var response = await _customers.CreateIndividualCustomer(model);
if (!response.Success)
{
    _log.LogWarning("Mono rejected the customer: {Status} {Message}",
        response.Status, response.Message);

    foreach (var err in response.MonoErrors ?? Array.Empty<MonoErrors>())
    {
        _log.LogWarning("  - {Field}: {Message}", err.Field, err.Message);
    }

    return BadRequest(response.Message);
}
```

Common cases:
- `"Customer already exists"` — duplicate identity (BVN/RC number)
- `"Please use a Lookup app, go to your dashboard..."` — secret key is for
  the wrong product family ([configuration](./configuration.md#per-product-keys))
- `"Invalid Customer Id"` — the `{id}` you passed doesn't exist
- `"Insufficient funds"` (mandate debit) — exactly what it says

### 2. Server errors (Mono returned a 5xx)

Same shape — `Success=false`, `Message` populated. Retry with exponential
backoff or give up:

```csharp
var attempts = 0;
MonoStandardResponse<CustomerResponse> response;
do
{
    response = await _customers.RetrieveCustomer(id);
    if (response.Success) break;
    if (response.Status?.StartsWith("5") != true) break; // not a 5xx; don't retry
    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempts)));
} while (++attempts < 3);
```

### 3. Network / parse failures (no response from Mono)

If the HTTP call itself fails — DNS, timeout, TLS handshake — the underlying
exception flows up through `HandleResponse()`. The library catches the
common case where Mono returned non-JSON (e.g. an HTML 404 page from a CDN)
and turns it into a `MonoStandardResponse<T>.Error(message)`, but a hard
network failure throws.

Wrap your call sites in a `try/catch` if you need to survive transient
network problems:

```csharp
try
{
    var response = await _customers.RetrieveCustomer(id);
    // ... handle Success/!Success
}
catch (HttpRequestException ex)
{
    _log.LogError(ex, "Mono unreachable");
    return ServiceUnavailable();
}
catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
{
    _log.LogError(ex, "Mono timed out");
    return ServiceUnavailable();
}
```

## Binary downloads (PDF endpoints)

`IMonoWatchlist.GetScreeningReport` and
`IMonoLookUp.GetCacStatusReport` return `MonoStandardResponse<byte[]>` —
same envelope, but `Data` contains the raw PDF bytes:

```csharp
var report = await _watchlist.GetScreeningReport(screeningId);
if (!report.Success)
{
    _log.LogError("Report fetch failed: {Message}", report.Message);
    return;
}
await File.WriteAllBytesAsync("report.pdf", report.Data);
```

On a non-2xx response the library reads the body as JSON and parses it into
the standard envelope; if the body isn't JSON (rare), it falls back to a
generic error message with the raw response text.

## A reusable helper

Most apps want a one-liner that throws on failure. Build your own:

```csharp
public static T Unwrap<T>(this MonoStandardResponse<T> response,
    [CallerMemberName] string caller = null)
{
    if (response.Success) return response.Data;
    throw new MonoApiException(
        $"{caller} failed: {response.Status} — {response.Message}",
        response);
}
```

Then call sites become:

```csharp
var customer = (await _customers.RetrieveCustomer(id)).Unwrap();
```

The library doesn't ship this helper because the right error-handling
strategy depends on the calling app (controllers want `BadRequest`,
background jobs want retries, webhooks want idempotent handling).

## What to log

Useful structured fields per call:
- `response.Status` — Mono's status string
- `response.Message` — error reason
- `response.MonoErrors` — field-level breakdown (validation)
- the request reference / customer id / etc.

Don't log the secret key, ever. Don't log `response.Data` for PII-bearing
endpoints (Identity, BVN details, NIN, Prove customers).

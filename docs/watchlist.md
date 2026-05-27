# Watchlist Screening

`IMonoWatchlist` screens individuals and entities against sanctions, PEP
lists, and adverse-media records. Supports single + batch screening, ongoing
monitoring, audit logs, and PDF compliance reports.

Uses `LookupSecretKey ?? SecretKey` ([configuration](./configuration.md#per-product-keys)) —
Watchlist is part of the Lookup product family on Mono's dashboard.

## Typical compliance flow

1. Customer onboards → you run `SubmitIndividualScreening` / `SubmitBatchScreening`
2. Poll `GetScreeningResult` (or wait for the webhook) for matches + risk score
3. If risk is non-trivial, download `GetScreeningReport` for compliance records
4. Enroll in `StartMonitoring` so you're alerted when the subject hits a new list

## Submit a screening

### Individual

```csharp
var scr = await _watchlist.SubmitIndividualScreening(new SubmitIndividualScreeningModel
{
    Name        = "Ada Lovelace",
    DateOfBirth = "1815-12-10",
    Gender      = "female",
    Bvn         = "12345678901",
    Country     = "NG",
});
```

### Entity / business

```csharp
var scr = await _watchlist.SubmitEntityScreening(new SubmitEntityScreeningModel
{
    Name    = "ACME LIMITED",
    Address = "12 Acme Way, Lagos",
    Country = "NG",
});
```

### Batch (mixed individuals + entities)

```csharp
var batch = await _watchlist.SubmitBatchScreening(new BatchScreeningModel
{
    Entries = new List<WatchlistScreeningSubject>
    {
        new WatchlistScreeningSubject
        {
            Type = WatchlistSubjectTypeConstants.Individual,
            Name = "Ada Lovelace", Country = "NG",
            DateOfBirth = "1815-12-10", Bvn = "12345678901",
        },
        new WatchlistScreeningSubject
        {
            Type = WatchlistSubjectTypeConstants.Entity,
            Name = "ACME LIMITED", Country = "NG",
            Address = "12 Acme Way, Lagos",
        },
    },
});
```

`WatchlistScreeningSubject` is a flat shape — populate only the fields that
apply for each entry's `Type`.

## Poll the result

Screening is async. Poll until `Status == "completed"`:

```csharp
var result = await _watchlist.GetScreeningResult(scr.Data.Id);
if (result.Data.Status == ScreeningStatusConstants.Completed)
{
    if (result.Data.RiskLevel == RiskLevelConstants.High)
    {
        await _compliance.EscalateAsync(result.Data);
    }

    foreach (var match in result.Data.Matches ?? Array.Empty<WatchlistMatch>())
    {
        // match.MatchScore (0–1), MatchLevel ("strong"/"weak"), Categories, etc.
    }
}
```

Or implement the webhook handler and wait for
`watchlist.match_found` / `watchlist.monitoring_update`:

```csharp
public Task HandleWatchlistMatchEvent(WatchlistMatchEventModel webhook)
{
    if (webhook.RiskLevel == RiskLevelConstants.High)
        return _compliance.EscalateAsync(webhook.ScreeningId);
    return Task.CompletedTask;
}
```

See [webhooks](./webhooks.md#newer-event-types--imonowebhookconsumerextensions).

## Audit log

Every screening has a lifecycle audit log — useful for compliance reviews:

```csharp
var audit = await _watchlist.GetAuditLog(screeningId);
foreach (var entry in audit.Data.Entries)
{
    // entry.Event ("created", "completed", "match_acknowledged"...)
    // entry.Actor, entry.CreatedAt
}
```

## PDF compliance report

Returns raw PDF bytes wrapped in `MonoStandardResponse<byte[]>`:

```csharp
var report = await _watchlist.GetScreeningReport(screeningId);
if (report.Success)
{
    await File.WriteAllBytesAsync($"screening-{screeningId}.pdf", report.Data);
}
```

On a non-2xx the library reads Mono's JSON error envelope so the standard
`Success` / `Message` / `MonoErrors` shape still applies — see
[error handling](./error-handling.md#binary-downloads-pdf-endpoints).

## Ongoing monitoring

Enroll a subject in continuous re-screening. Mono fires
`watchlist.monitoring_update` whenever a new match appears (or an existing
match changes risk level).

```csharp
var mon = await _watchlist.StartMonitoring(new StartMonitoringModel
{
    Type        = WatchlistSubjectTypeConstants.Individual,
    Name        = "Ada Lovelace",
    Country     = "NG",
    DateOfBirth = "1815-12-10",
    Bvn         = "12345678901",
});

// later — cancel
await _watchlist.StopMonitoring(mon.Data.Id);
```

## Risk score interpretation

The `risk_score` is a numeric 0–100 value; `risk_level` is the bucketed
version using `RiskLevelConstants`:

| Level | Score range (Mono's defaults) |
|---|---|
| `low` | 0 – 30 |
| `medium` | 31 – 70 |
| `high` | 71 – 100 |

Threshold boundaries can shift between sandbox and live — verify with your
account manager before hard-coding decisions on raw scores. Bucket on
`RiskLevel` instead where possible.

## Pricing

Each submission is billed; monitoring carries an ongoing per-subject fee.
Failed screenings (e.g. malformed input) are still billed — make your input
validation strict.

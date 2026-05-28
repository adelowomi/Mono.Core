# Changelog

All notable changes to **Mono.Core** are documented here. The format follows
[Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project follows
[Semantic Versioning](https://semver.org/).

## [1.9.3] — 2026-05

### Fixed

- **`IMonoAccounts.GetTransactions` now correctly deserialises Mono's
  flat transactions response.** Mono returns the endpoint as
  `{ status, message, timestamp, data: [ ... ] }` — `data` is the
  transaction array directly. The Refit signature previously expected
  `MonoStandardResponse<TransactionResponseModel>` (data nested inside
  an object with its own `paging` + `data` fields), so System.Text.Json
  hit a type mismatch (`data: array → object`) and silently returned
  `null` Content. After `1.9.2` made `HandleResponse` defensive this
  surfaced as `"Mono returned an empty response body"` instead of a
  `NullReferenceException`, but transactions still didn't flow through.

  The Refit interface now declares
  `MonoStandardResponse<List<Transaction>>`, matching the actual wire
  shape. `AccountService.GetTransactions` adapts the deserialised
  envelope back into `MonoStandardResponse<TransactionResponseModel>`
  so existing consumers (e.g. `response.Data.Transactions`) keep
  working unchanged.

  `TransactionResponseModel.Paging` is now populated from Mono's
  top-level `meta` field (verified against
  [docs.mono.co/api/bank-data/transactions](https://docs.mono.co/api/bank-data/transactions)
  and a live API capture). Mono returns `meta` in both `paginate=true`
  and `paginate=false` modes; previously every consumer saw it as null
  because the wrapper looked for a nested `paging` field that Mono
  doesn't send.

### Added

- **`MonoStandardResponse<T>.Meta`** — top-level pagination envelope
  (`total`, `page`, `previous`, `next`). Mono surfaces this consistently
  across list endpoints (transactions, payouts, etc.); the envelope
  type now captures it so the wire shape is honest. Backward-compatible:
  consumers that didn't read pagination metadata don't notice the
  addition.

## [1.9.2] — 2026-05

### Fixed

- **`RefitExtensions.HandleResponse<T>` no longer throws `NullReferenceException`
  on non-typical Mono responses.** Three latent null derefs:
  - `response.Error.Content` could be null (empty body, proxy 5xx); now
    guarded.
  - `JsonSerializer.Deserialize<...>` can legitimately return null (e.g.
    body is the literal string `"null"` or non-JSON); now caught and
    surfaced as a typed `Error` envelope instead of NRE'ing.
  - `response.Content` could be null on a `200 No Content`; now guarded
    with an explicit "empty response body" error message.

  Consumers always get a `MonoStandardResponse<T>` back. Any unparseable
  error body now surfaces both the status code and (when present) the
  parsed envelope's message, making upstream debugging cheaper.

## [1.9.1] — 2026-05

### Fixed

- **`IMonoAuthorization.AuthorizeAccount` now POSTs to `/accounts/auth`
  (plural).** The Refit attribute was `[Post("/account/auth")]` (singular),
  which doesn't exist on Mono's API. Every link-exchange call surfaced as
  HTTP 404 → consumers re-skinned it as "code expired", masking the real
  cause. Every other route in the same interface used the plural form;
  this was an isolated typo. No API surface change for consumers — calling
  code stays identical.

## [1.9.0] — 2026-05

DirectPay money-operations additions — closes the last gaps from the May 2026
Mono docs audit.

**`IMonoDirectPay` adds 5 endpoints (all v2):**
- `RefundPayment` (POST `/payments/refund`) — body: `reference` + optional
  `source` (`wallet` or `pending_payout`)
- `GetPayouts` (GET `/payments/payouts`) — list payouts; filter by `status`
  (`pending` / `processing` / `settled` / `failed`)
- `GetPayoutTransactions` (GET `/payments/payout/{payoutId}/transactions`)
- `CreateSubAccount` (POST `/payments/payout/sub-accounts`) — for split-payment
  configurations
- `GetSubAccounts` (GET `/payments/payout/sub-accounts`)

**Constants:** `PayoutStatusConstants`, `RefundSourceConstants`.

**Path naming wrinkle:** Mono uses `payouts` (plural) for the list endpoint
but `payout` (singular) for everything else. Mirrored verbatim.

## [1.8.0] — 2026-05

CAC additions — fills the gap left by the v1.1.0 deprecations of
`GetPreviousAddress` and `GetChangeOfName`.

**`IMonoLookUp`:**
- `GetCacPsc` (GET `/lookup/cac/company/{id}/psc`) — Persons with Significant
  Control
- `GetCacProfile` (GET `/lookup/cac/profile/{rcNumber}`) — aggregate profile
  (business + shareholders + directors + secretaries + PSC). Takes **RC
  number**, not the numeric business id used by the other CAC endpoints.
- `GetCacStatusReport` (GET `/lookup/cac/company/{id}/status-report`) — binary
  PDF, returned as `byte[]`

**Models added:** `CacPscEntry`, `CacProfileResponse`.

## [1.7.0] — 2026-05

Webhook hardening — signature verification, mandate routing bug fix, new event
surface.

**Signature verification (security):**
- New `MonoInitializationOptions.WebhookSecret` — when set, the controller
  constant-time-compares the `mono-webhook-secret` header and returns 401 on
  mismatch
- If unset, the controller logs a warning and processes the request anyway
  (opt-in for backward compatibility; **set it in production**)

**Routing fix:**
- The controller's previous routing for mandate events was broken —
  `event.Split('.')[2]` always returned `"mandate"`, then compared against
  constants like `"created"`, so the switch never matched and every mandate
  webhook fell into `HandleUnknownEvent`. Now strips the `mono.events.` prefix
  and matches the full suffix.
- Removed the mutable `_eventType` instance field

**`MonoEventTypes` constants — breaking value change for mandate events:**

| Constant | Old value (broken) | New value |
|---|---|---|
| `MandateCreated` | `"created"` | `"mandate.created"` |
| `MandateReady` | `"ready"` | `"mandate.ready"` |
| `MandateApproved` | `"approved"` | `"mandate.approved"` |

The old values never matched anything in the controller routing, so this is a
bug fix; users who hardcoded the old values for their own routing will need to
update.

**New event types:**
- `mandate.debit.successful` / `mandate.debit.failed`
- `account_income`
- `account_creditworthiness`
- `disbursement.initiated` / `processing` / `completed` / `failed`
- `watchlist.match_found` / `watchlist.monitoring_update`
- `prove.completed` / `prove.failed`

**New optional `IMonoWebhookConsumerExtensions` interface** — implement
alongside `IMonoWebhookConsumer` to receive the new events as strongly-typed
payloads. Consumers that don't implement it receive these events via
`HandleUnknownEvent` (same behavior as before).

**Standard event fields added to `MonoWebhookModel<T>`:** `event_id`,
`timestamp`, `app`, `business`.

## [1.6.0] — 2026-05

Connect additions — fills gaps in existing services without introducing new
product surfaces.

**`IMonoAccounts`:**
- `GetAccountBalance` (GET `/accounts/{id}/balance`) — real-time balance fetch
- `AccountLinkingModel` gains `Institution` and `CheckAccountMatch` for the
  Feb 2026 Account Match feature; result arrives on the existing
  `account_updated` webhook
- `AccountUpdatedEventModel` gains `AccountMatch` (status, matched bool,
  expected vs linked account numbers, reason)

**`IMonoLookUp`:**
- `GetNinPdf` (POST `/lookup/nin` with `output=pdf`) — async NIN lookup
  returning a job id
- `PollNinJob` (GET `/lookup/nin/{jobId}/job`) — poll status; completed jobs
  include a 7-day download URL

**Constants:** `NinOutputConstants`, `NinJobStatusConstants`.

## [1.5.0] — 2026-05

Adds the Mono Prove API surface — full-stack KYC verification.

**New `IMonoProve` interface — 6 endpoints under `/v1/prove/...`:**
- `InitiateProve` (POST `/prove/initiate`)
- `FetchCustomerDetails` (GET `/prove/customers/{reference}`)
- `FetchAllCustomerDetails` (GET `/prove/customers`)
- `BlacklistCustomer` (POST `/prove/customers/blacklist`)
- `WhitelistCustomer` (POST `/prove/customers/whitelist`)
- `RevokeDataAccess` (DELETE `/prove/customers/{reference}`)

**Infrastructure:** `IRefitClientBuilder<T>` gains `BuildV1(string)` — Prove
still lives under `/v1/`. Pattern matches the existing `BuildV3` helper.

**Constants:** `ProveKycLevelConstants`, `ProveIdentityTypeConstants`,
`ProveBlacklistCodeConstants` (101-105 pass-through).

## [1.4.0] — 2026-05

Adds the Mono Watchlist Screening API surface (released by Mono in March 2026).

**New `IMonoWatchlist` interface — 7 endpoints under `/v3/lookup/watchlist/...`:**
- `SubmitIndividualScreening` (POST `/lookup/watchlist`, `type=individual`)
- `SubmitEntityScreening` (POST `/lookup/watchlist`, `type=entity`)
- `SubmitBatchScreening` (POST `/lookup/watchlist/batch`)
- `GetScreeningResult` (GET `/lookup/watchlist/{id}`)
- `GetAuditLog` (GET `/lookup/watchlist/{id}/audit-log`)
- `GetScreeningReport` (GET `/lookup/watchlist/{id}/report` — binary PDF,
  returned as `byte[]`)
- `StartMonitoring` (POST `/lookup/watchlist/monitor`)
- `StopMonitoring` (DELETE `/lookup/watchlist/monitor/{id}`)

**Constants:** `WatchlistSubjectTypeConstants`, `ScreeningStatusConstants`,
`RiskLevelConstants`.

## [1.3.0] — 2026-05

Adds the Mono Disburse API surface (added by Mono in September 2025).

**New `IMonoDisburse` interface — 13 endpoints under
`/v3/payments/disburse/...`:**

*Source accounts:* `CreateSourceAccount`, `UpdateSourceAccount`,
`FetchAllSourceAccounts`, `FetchSourceAccount`.

*Disbursements (batches):* `CreateDisbursement`, `CreateInstantDisbursement` /
`CreateScheduledDisbursement` (convenience wrappers), `TransitionDisbursement`,
`FetchAllDisbursements`, `FetchDisbursement`.

*Distributions:* `AddDistributionsToBatch`, `UpdateDistributionInBatch`,
`DeleteDistributionInBatch`, `FetchAllDistributionsInBatch`,
`FetchSingleDistribution`.

**Constants:** `DisbursementTypeConstants`, `DisbursementSourceConstants`,
`TransitionActionConstants`.

## [1.2.0] — 2026-05

Adds the Mono Customer API surface (added by Mono in March 2024).

**New `IMonoCustomers` interface — eight endpoints under `/v2/customers`:**
- `CreateIndividualCustomer` (POST `/customers`, `type=individual`)
- `CreateBusinessCustomer` (POST `/customers`, `type=business`)
- `RetrieveCustomer` (GET `/customers/{id}`)
- `ListCustomers` (GET `/customers`, paginated)
- `GetCustomerTransactions` (GET `/customers/{id}/transactions`)
- `FetchAllLinkedAccounts` (GET `/accounts` — Mono files this under Customer)
- `UpdateCustomer` (PATCH `/customers/{id}`, all fields optional)
- `DeleteCustomer` (DELETE `/customers/{id}`)

**Constants:** `CustomerTypeConstants`, `CustomerIdentityTypeConstants`.

## [1.1.0] — 2026-05

Drift-only refresh against the current Mono docs.

**Endpoints fixed:**
- BVN: `/lookup/bvn/verify` → `/lookup/bvn/verify-otp`
- BVN: `/lookup/bvn/details` → `/lookup/bvn/fetch-bvn`
- BVN `scope` values now lowercase (`identity`, `bank_accounts`) to match
  the API
- Lookups for address, passport, TIN, NIN, drivers-license, account-number,
  credit-history, mashup are now `POST` (they were declared `[Get]` with
  `[Body]` — invalid in Refit and inconsistent with the docs)
- Driver's license path: `/lookup/driver_license` → `/lookup/drivers-license`
- International passport path: `/lookup/passport` → `/lookup/intl-passport`
- Mandate balance enquiry: `amount` query parameter is now optional. Present
  = sufficient-funds check (NGN 10), absent = real-time balance (NGN 50)

**Deprecated (kept for source compatibility, marked `[Obsolete]`):**
- `IMonoLookUp.GetPreviousAddress` — Mono retired the CAC previous-address
  endpoint
- `IMonoLookUp.GetChangeOfName` — Mono retired the CAC change-of-name endpoint

**Mandate creation:**
- `CreateMandateModel` adds `fee_bearer`, `verification_method`, and `meta`
  fields per the v3 docs
- New string constants: `MandateTypeConstants` (emandate/sweep),
  `DebitTypeConstants` (variable/fixed), `FeeBearerConstants`
  (business/customer), `VerificationMethodConstants`
  (transfer_verification/selfie_verification)

**Security / dependencies:**
- Refit bumped to `7.2.22` to address
  [CVE-2024-51501](https://github.com/advisories/GHSA-3hxg-fxwm-8gf7) (CRLF
  header injection)
- Removed unused `Moq` and `Newtonsoft.Json.Bson` from the runtime package;
  `Moq` moved to the test project where it belongs

[1.9.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.9.0
[1.8.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.8.0
[1.7.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.7.0
[1.6.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.6.0
[1.5.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.5.0
[1.4.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.4.0
[1.3.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.3.0
[1.2.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.2.0
[1.1.0]: https://github.com/adelowomi/Mono.Core/releases/tag/v1.1.0

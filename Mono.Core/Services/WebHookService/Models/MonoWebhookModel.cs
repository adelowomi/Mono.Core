using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Webhooks
{
    public class Account
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("institution")]
        public Institution Institution { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; set; }

        [JsonPropertyName("liveMode")]
        public bool LiveMode { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class AccountUpdatedEventModel
    {
        [JsonPropertyName("webhook_id")]
        public string WebhookId { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("account")]
        public Account Account { get; set; }

        /// <summary>
        /// Account Match verification result (Feb 2026). Populated only when
        /// the linking request set <c>check_account_match=true</c>.
        /// </summary>
        [JsonPropertyName("account_match")]
        public AccountMatchResult AccountMatch { get; set; }
    }

    public class AccountMatchResult
    {
        /// <summary>"matched" or "not_matched".</summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("matched")]
        public bool? Matched { get; set; }

        /// <summary>Account number the customer was asked to match against.</summary>
        [JsonPropertyName("expected_account_number")]
        public string ExpectedAccountNumber { get; set; }

        /// <summary>Account number that was actually linked.</summary>
        [JsonPropertyName("linked_account_number")]
        public string LinkedAccountNumber { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }

    public class Institution
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("bankCode")]
        public string BankCode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("data_status")]
        public string DataStatus { get; set; }
        [JsonPropertyName("auth_method")]
        public string AuthMethod { get; set; }
    }

    public class MonoWebhookModel<T>
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("Data")]
        public T Data { get; set; }

        /// <summary>Stable per-event identifier; safe to use for idempotency keys.</summary>
        [JsonPropertyName("event_id")]
        public string EventId { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }
    }

    public class AccountConnectedEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("customer")]
        public string Customer { get; set; }
    }

    /// <summary>
    /// Event-type constants. Each value is the full event string Mono sends
    /// minus the <c>mono.events.</c> prefix — e.g. <c>account_connected</c>
    /// or <c>mandate.debit.successful</c>.
    /// </summary>
    public class MonoEventTypes
    {
        public const string WebhookTestEvent = "webhook_test";

        // Connect / accounts
        public const string AccountConnected = "account_connected";
        public const string AccountUpdated = "account_updated";
        public const string AccountIncome = "account_income";
        public const string Creditworthiness = "account_creditworthiness";

        // Direct debit / mandates (fixed in v1.7.0 — previous values
        // "created"/"ready"/"approved" were never matched by the controller
        // routing because the event string is "mono.events.mandate.created").
        public const string MandateCreated = "mandate.created";
        public const string MandateApproved = "mandate.approved";
        public const string MandateReady = "mandate.ready";
        public const string MandateDebitSuccessful = "mandate.debit.successful";
        public const string MandateDebitFailed = "mandate.debit.failed";

        // Disburse (Sept 2025)
        public const string DisbursementInitiated = "disbursement.initiated";
        public const string DisbursementProcessing = "disbursement.processing";
        public const string DisbursementCompleted = "disbursement.completed";
        public const string DisbursementFailed = "disbursement.failed";

        // Watchlist screening (March 2026)
        public const string WatchlistMatchFound = "watchlist.match_found";
        public const string WatchlistMonitoringUpdate = "watchlist.monitoring_update";

        // Prove (KYC)
        public const string ProveCompleted = "prove.completed";
        public const string ProveFailed = "prove.failed";

        public const string Unknown = "unknown";
    }

    public class MandateCreatedEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("ready_to_debit")]
        public bool? ReadyToDebit { get; set; }

        [JsonPropertyName("nibss_code")]
        public string NibssCode { get; set; }

        [JsonPropertyName("approved")]
        public bool? Approved { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank")]
        public string Bank { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }
    }
    public class MandateApprovedEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("ready_to_debit")]
        public bool? ReadyToDebit { get; set; }

        [JsonPropertyName("nibss_code")]
        public string NibssCode { get; set; }

        [JsonPropertyName("approved")]
        public bool? Approved { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank")]
        public string Bank { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }
    }
    public class MandateReadyEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("ready_to_debit")]
        public bool ReadyToDebit { get; set; }

        [JsonPropertyName("nibss_code")]
        public string NibssCode { get; set; }

        [JsonPropertyName("approved")]
        public bool Approved { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank")]
        public string Bank { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("live_mode")]
        public bool LiveMode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }
    }

    // ============ Events added in v1.7.0 ============

    /// <summary>
    /// Payload for <c>mono.events.mandate.debit.successful</c> and
    /// <c>mono.events.mandate.debit.failed</c>.
    /// </summary>
    public class MandateDebitEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("mandate")]
        public string Mandate { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; }

        [JsonPropertyName("nibss_code")]
        public string NibssCode { get; set; }

        [JsonPropertyName("failure_reason")]
        public string FailureReason { get; set; }

        [JsonPropertyName("processed_at")]
        public DateTime? ProcessedAt { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }
    }

    /// <summary>Payload for <c>mono.events.account_income</c>.</summary>
    public class AccountIncomeEventModel
    {
        [JsonPropertyName("account")]
        public string Account { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("monthly_income")]
        public double? MonthlyIncome { get; set; }

        [JsonPropertyName("annual_income")]
        public double? AnnualIncome { get; set; }

        [JsonPropertyName("total_income")]
        public double? TotalIncome { get; set; }

        [JsonPropertyName("income_sources")]
        public System.Collections.Generic.List<string> IncomeSources { get; set; }
    }

    /// <summary>Payload for <c>mono.events.account_creditworthiness</c>.</summary>
    public class CreditworthinessEventModel
    {
        [JsonPropertyName("account")]
        public string Account { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("score")]
        public double? Score { get; set; }

        [JsonPropertyName("rating")]
        public string Rating { get; set; }

        [JsonPropertyName("recommendation")]
        public string Recommendation { get; set; }
    }

    /// <summary>
    /// Payload for any <c>mono.events.disbursement.*</c> event. Inspect
    /// <see cref="Mono.Core.Webhooks.MonoWebhookModel{T}.Event"/> for the
    /// specific lifecycle state (initiated / processing / completed / failed).
    /// </summary>
    public class DisbursementEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("account")]
        public string Account { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("total_amount")]
        public long? TotalAmount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("distribution_count")]
        public long? DistributionCount { get; set; }

        [JsonPropertyName("processed_at")]
        public DateTime? ProcessedAt { get; set; }

        [JsonPropertyName("failure_reason")]
        public string FailureReason { get; set; }
    }

    /// <summary>
    /// Payload for <c>mono.events.watchlist.match_found</c> and
    /// <c>mono.events.watchlist.monitoring_update</c>.
    /// </summary>
    public class WatchlistMatchEventModel
    {
        [JsonPropertyName("screening_id")]
        public string ScreeningId { get; set; }

        [JsonPropertyName("subject_name")]
        public string SubjectName { get; set; }

        [JsonPropertyName("subject_type")]
        public string SubjectType { get; set; }

        [JsonPropertyName("match_count")]
        public int? MatchCount { get; set; }

        [JsonPropertyName("risk_score")]
        public double? RiskScore { get; set; }

        [JsonPropertyName("risk_level")]
        public string RiskLevel { get; set; }

        [JsonPropertyName("monitoring")]
        public bool? Monitoring { get; set; }
    }

    /// <summary>
    /// Payload for <c>mono.events.prove.completed</c> and
    /// <c>mono.events.prove.failed</c>.
    /// </summary>
    public class ProveEventModel
    {
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("kyc_level")]
        public string KycLevel { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("failure_reason")]
        public string FailureReason { get; set; }
    }
}
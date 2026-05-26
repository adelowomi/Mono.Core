using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Refit;

namespace Mono.Core.Disburse
{
    // ============ Source accounts ============

    public class CreateSourceAccountModel
    {
        [Required]
        [JsonPropertyName("app")]
        public string App { get; set; }

        [Required]
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [Required]
        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class UpdateSourceAccountModel
    {
        [Required]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class SourceAccountResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("balance")]
        public long? Balance { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class SourceAccountListResponse
    {
        [JsonPropertyName("source_accounts")]
        public List<SourceAccountResponse> SourceAccounts { get; set; }

        [JsonPropertyName("meta")]
        public DisbursePaginationMeta Meta { get; set; }
    }

    public class SourceAccountListQueryOptions
    {
        // [AliasAs] applies for Refit query-string serialization;
        // [JsonPropertyName] would be ignored on query params.
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("status")] public string Status { get; set; }
    }

    // ============ Disbursements (batches) ============

    public class DisbursementRecipientAccount
    {
        [Required]
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [Required]
        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }
    }

    public class DistributionModel
    {
        [Required]
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [Required]
        [JsonPropertyName("recipient_email")]
        public string RecipientEmail { get; set; }

        [Required]
        [JsonPropertyName("account")]
        public DisbursementRecipientAccount Account { get; set; }

        [Required]
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [Required]
        [JsonPropertyName("narration")]
        public string Narration { get; set; }
    }

    public class CreateDisbursementModel
    {
        [Required]
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        /// <summary>Funding source. Currently "mandate".</summary>
        [Required]
        [JsonPropertyName("source")]
        public string Source { get; set; } = DisbursementSourceConstants.Mandate;

        /// <summary>Source account id (from <see cref="SourceAccountResponse.Id"/>).</summary>
        [Required]
        [JsonPropertyName("account")]
        public string Account { get; set; }

        /// <summary>"instant" or "scheduled".</summary>
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [Required]
        [JsonPropertyName("total_amount")]
        public long TotalAmount { get; set; }

        [Required]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>Scheduled execution date (ISO-8601). Required when <see cref="Type"/> is "scheduled".</summary>
        [JsonPropertyName("scheduled_date")]
        public string ScheduledDate { get; set; }

        [Required]
        [JsonPropertyName("distribution")]
        public List<DistributionModel> Distribution { get; set; }
    }

    public class TransitionDisbursementModel
    {
        /// <summary>"trigger" or "cancel".</summary>
        [Required]
        [JsonPropertyName("action")]
        public string Action { get; set; }
    }

    public class DisbursementResponse
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
        public long TotalAmount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("scheduled_date")]
        public DateTime? ScheduledDate { get; set; }

        [JsonPropertyName("processed_at")]
        public DateTime? ProcessedAt { get; set; }

        [JsonPropertyName("distribution_count")]
        public long? DistributionCount { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class DisbursementListResponse
    {
        [JsonPropertyName("disbursements")]
        public List<DisbursementResponse> Disbursements { get; set; }

        [JsonPropertyName("meta")]
        public DisbursePaginationMeta Meta { get; set; }
    }

    public class DisbursementListQueryOptions
    {
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("status")] public string Status { get; set; }
        [AliasAs("type")] public string Type { get; set; }
        [AliasAs("start")] public string Start { get; set; }
        [AliasAs("end")] public string End { get; set; }
    }

    // ============ Distributions inside a batch ============

    public class AddDistributionsModel
    {
        [Required]
        [JsonPropertyName("distribution")]
        public List<DistributionModel> Distribution { get; set; }
    }

    public class UpdateDistributionModel
    {
        [JsonPropertyName("recipient_email")]
        public string RecipientEmail { get; set; }

        [JsonPropertyName("account")]
        public DisbursementRecipientAccount Account { get; set; }

        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; }
    }

    public class DistributionResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("disbursement")]
        public string Disbursement { get; set; }

        [JsonPropertyName("recipient_email")]
        public string RecipientEmail { get; set; }

        [JsonPropertyName("account")]
        public DisbursementRecipientAccount Account { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("failure_reason")]
        public string FailureReason { get; set; }

        [JsonPropertyName("processed_at")]
        public DateTime? ProcessedAt { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class DistributionListResponse
    {
        [JsonPropertyName("distributions")]
        public List<DistributionResponse> Distributions { get; set; }

        [JsonPropertyName("meta")]
        public DisbursePaginationMeta Meta { get; set; }
    }

    public class DistributionListQueryOptions
    {
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("status")] public string Status { get; set; }
    }

    public class DisbursePaginationMeta
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("page")]
        public long Page { get; set; }

        [JsonPropertyName("previous")]
        public string Previous { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }
    }

    // ============ Constants ============

    public class DisbursementTypeConstants
    {
        public const string Instant = "instant";
        public const string Scheduled = "scheduled";
    }

    public class DisbursementSourceConstants
    {
        public const string Mandate = "mandate";
    }

    public class TransitionActionConstants
    {
        public const string Trigger = "trigger";
        public const string Cancel = "cancel";
    }
}

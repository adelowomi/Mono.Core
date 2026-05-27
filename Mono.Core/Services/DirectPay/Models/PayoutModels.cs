using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Refit;

namespace Mono.Core.Services.DirectPay.Models
{
    public class PayoutListQueryOptions
    {
        /// <summary>One of <see cref="PayoutStatusConstants"/>.</summary>
        [AliasAs("status")] public string Status { get; set; }
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("start")] public string Start { get; set; }
        [AliasAs("end")] public string End { get; set; }
    }

    public class PayoutResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>Amount in minor units (kobo for NGN, pesewa for GHS, cents for KES/ZAR).</summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("settlement_account")]
        public string SettlementAccount { get; set; }

        [JsonPropertyName("settled_at")]
        public DateTime? SettledAt { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class PayoutListResponse
    {
        [JsonPropertyName("payouts")]
        public List<PayoutResponse> Payouts { get; set; }

        [JsonPropertyName("meta")]
        public PayoutPaginationMeta Meta { get; set; }
    }

    public class PayoutTransactionsQueryOptions
    {
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("status")] public string Status { get; set; }
        [AliasAs("start")] public string Start { get; set; }
        [AliasAs("end")] public string End { get; set; }
    }

    public class PayoutTransaction
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("payout")]
        public string Payout { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class PayoutTransactionsResponse
    {
        [JsonPropertyName("transactions")]
        public List<PayoutTransaction> Transactions { get; set; }

        [JsonPropertyName("meta")]
        public PayoutPaginationMeta Meta { get; set; }
    }

    public class PayoutPaginationMeta
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

    public class PayoutStatusConstants
    {
        public const string Pending = "pending";
        public const string Processing = "processing";
        public const string Settled = "settled";
        public const string Failed = "failed";
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class RefundPaymentModel
    {
        /// <summary>Reference of the original payment to refund.</summary>
        [Required]
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        /// <summary>"wallet" or "pending_payout" (default if omitted).</summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }
    }

    public class RefundPaymentResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }

    public class RefundSourceConstants
    {
        public const string Wallet = "wallet";
        public const string PendingPayout = "pending_payout";
    }
}

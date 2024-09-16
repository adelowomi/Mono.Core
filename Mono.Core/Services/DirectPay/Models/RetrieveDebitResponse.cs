using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class RetrieveDebitResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }

        [JsonPropertyName("fee")]
        public double? Fee { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("object_id")]
        public string ObjectId { get; set; }

        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }

        [JsonPropertyName("refunded")]
        public bool? Refunded { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("__v")]
        public int? V { get; set; }
    }
}

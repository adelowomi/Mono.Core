using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class BalanceEnquiryResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("has_sufficient_balance")]
        public bool? HasSufficientBalance { get; set; }

        [JsonPropertyName("account_balance")]
        public double? AccountBalance { get; set; }
    }
}

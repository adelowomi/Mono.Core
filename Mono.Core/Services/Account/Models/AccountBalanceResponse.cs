using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    /// <summary>
    /// Response shape for the real-time balance endpoint (GET /accounts/{id}/balance).
    /// </summary>
    public class AccountBalanceResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>Balance in minor units (kobo for NGN, pesewa for GHS, cents for KES/ZAR).</summary>
        [JsonPropertyName("balance")]
        public long? Balance { get; set; }

        /// <summary>Funds available for withdrawal (subset of <see cref="Balance"/>).</summary>
        [JsonPropertyName("available_balance")]
        public long? AvailableBalance { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime? LastUpdated { get; set; }
    }
}

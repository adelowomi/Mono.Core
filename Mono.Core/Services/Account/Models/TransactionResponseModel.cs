using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    public class TransactionResponseModel
    {
        [JsonPropertyName("paging")]
        public MonoStandardPaginatedResponse Paging { get; set; }

        [JsonPropertyName("data")]
        public List<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("balance")]
        public long Balance { get; set; }
    }
}

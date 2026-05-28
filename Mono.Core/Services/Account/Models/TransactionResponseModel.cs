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
        // Mono returns the transaction id as `id` on the wire (verified against
        // docs.mono.co/api/bank-data/transactions and a live sandbox capture).
        // The previous attribute name "_id" came from MongoDB-internal
        // convention and never matched what Mono actually serialises — so
        // consumers always saw a null Id. Idempotent dedup paths that key on
        // this field were silently no-oping.
        [JsonPropertyName("id")]
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

        /// <summary>
        /// Currency the transaction settled in (e.g. "NGN"). Optional on the
        /// wire — Mono omits it for some legacy datasources.
        /// </summary>
        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Mono's first-pass categorisation hint (e.g. "bank_charges",
        /// "unknown"). Null when Mono hasn't categorised yet.
        /// </summary>
        [JsonPropertyName("category")]
        public string Category { get; set; }
    }
}

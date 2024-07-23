using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    public class StatementResponseModel
    {
        [JsonPropertyName("meta")]
        public ResponseMeta Meta { get; set; }

        [JsonPropertyName("data")]
        public List<Statement> StatementList { get; set; }
    }

    public class Statement
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

    public class ResponseMeta
    {
        [JsonPropertyName("count")]
        public long Count { get; set; }
    }

    public enum OutputType
    {
        Pdf = 1,
        Json
    }

    public static class TransactionType
    {
        public const string Credit = "credit";
        public const string Debit = "debit";
    }
}

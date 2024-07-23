using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    public class StatementPdfResponseModel
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
}

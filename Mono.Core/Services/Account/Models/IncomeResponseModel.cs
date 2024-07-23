using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    public class IncomeResponseModel
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("employer")]
        public string Employer { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }
    }
}

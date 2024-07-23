using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    public class Institution
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}

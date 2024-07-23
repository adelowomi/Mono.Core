using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    public class InformationResponseModel
    {
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("account")]
        public Account Account { get; set; }
    }

    

    
}

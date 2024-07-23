using System.Text.Json.Serialization;

namespace Mono.Core
{
    public class AuthorizationAccountResponseModel
    {
        [JsonPropertyName("id")]
        public string AccountId { get; set; }
    }
}

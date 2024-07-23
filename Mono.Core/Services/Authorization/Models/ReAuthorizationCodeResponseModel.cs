using System.Text.Json.Serialization;

namespace Mono.Core
{
    public class ReAuthorizationCodeResponseModel
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}

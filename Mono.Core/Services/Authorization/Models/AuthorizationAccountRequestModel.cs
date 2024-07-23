using System.Text.Json.Serialization;

namespace Mono.Core
{
    public class AuthorizationAccountRequestModel
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}

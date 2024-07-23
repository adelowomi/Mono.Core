using System.Text.Json.Serialization;

namespace Mono.Core
{
    public class ManualDataSyncResponseModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}

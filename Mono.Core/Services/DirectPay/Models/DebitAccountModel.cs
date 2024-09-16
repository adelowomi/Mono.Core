using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class DebitAccountModel
    {
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; }
    }
}

using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class VerifyMandateOtpModel
    {
        [JsonPropertyName("session")]
        public string Session { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("otp")]
        public string Otp { get; set; }
    }


    public class SetOtpMethodResponse
    {
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("session")]
        public string Session { get; set; }
    }

}

using System;
using System.Text.Json.Serialization;

namespace Mono.Core.DirectPay
{
    public class Customer
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("identity")]
        public Identity Identity { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Identity
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }
    }
    public class InitiateOneTimePayment
    {
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }
    }
}

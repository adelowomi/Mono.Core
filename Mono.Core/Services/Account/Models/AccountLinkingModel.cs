using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    public class AccountLinkingModel
    {
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }
    }

    public class Customer
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("ref")]
        public string Ref { get; set; }
    }

    public class AccountLinkingResponseModel
    {
        [JsonPropertyName("mono_url")]
        public string MonoUrl { get; set; }
        [JsonPropertyName("customer")]
        public string Customer { get; set; }
        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}




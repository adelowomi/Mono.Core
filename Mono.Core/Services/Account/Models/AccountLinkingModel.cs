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

        /// <summary>
        /// Optional. Pin the linking flow to a specific institution + account
        /// (e.g. for Account Match verification). When provided alongside
        /// <see cref="CheckAccountMatch"/> = true, Mono confirms the linked
        /// account number matches <see cref="AccountLinkingInstitution.AccountNumber"/>
        /// and reports the result on the <c>account_match</c> field of the
        /// <c>mono.events.account_updated</c> webhook.
        /// </summary>
        [JsonPropertyName("institution")]
        public AccountLinkingInstitution Institution { get; set; }

        /// <summary>
        /// Optional. Set true to enable Account Match (Feb 2026). Verifies the
        /// account number the customer links matches the one supplied in
        /// <see cref="Institution"/>; result is delivered on the
        /// <c>mono.events.account_updated</c> webhook.
        /// </summary>
        [JsonPropertyName("check_account_match")]
        public bool? CheckAccountMatch { get; set; }
    }

    public class AccountLinkingInstitution
    {
        /// <summary>Mono institution id or bank code. Required for Account Match.</summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>Customer's account number — Mono compares the linked account to this value.</summary>
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        /// <summary>Optional auth method override (e.g. <c>internet_banking</c>).</summary>
        [JsonPropertyName("auth_method")]
        public string AuthMethod { get; set; }
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




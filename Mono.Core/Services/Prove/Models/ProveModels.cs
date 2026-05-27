using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Refit;

namespace Mono.Core.Prove
{
    // ============ Initiate Prove ============

    public class ProveCustomerIdentity
    {
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [Required]
        [JsonPropertyName("number")]
        public string Number { get; set; }
    }

    public class ProveCustomer
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [Required]
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("identity")]
        public ProveCustomerIdentity Identity { get; set; }
    }

    public class InitiateProveModel
    {
        [Required]
        [JsonPropertyName("customer")]
        public ProveCustomer Customer { get; set; }

        [Required]
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [Required]
        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }

        /// <summary>"tier_1", "tier_2", "tier_3", or "custom".</summary>
        [Required]
        [JsonPropertyName("kyc_level")]
        public string KycLevel { get; set; }

        /// <summary>Required when <see cref="KycLevel"/> is "custom". Must include at least "bvn" or "nin".</summary>
        [JsonPropertyName("identities")]
        public List<string> Identities { get; set; }

        /// <summary>Include bank account verification in the flow.</summary>
        [JsonPropertyName("bank_accounts")]
        public bool? BankAccounts { get; set; }
    }

    public class InitiateProveResponse
    {
        [JsonPropertyName("mono_url")]
        public string MonoUrl { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }

    // ============ Customer details ============

    public class ProveIdentityResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; }

        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }
    }

    public class ProveFaceMatchResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("match")]
        public bool? Match { get; set; }

        [JsonPropertyName("confidence")]
        public double? Confidence { get; set; }

        [JsonPropertyName("manual_validation")]
        public bool? ManualValidation { get; set; }
    }

    public class ProveAddressVerification
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("confidence")]
        public double? Confidence { get; set; }
    }

    public class ProveBankAccount
    {
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("bank_name")]
        public string BankName { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }
    }

    public class ProveCustomerResponse
    {
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("kyc_level")]
        public string KycLevel { get; set; }

        [JsonPropertyName("customer")]
        public ProveCustomer Customer { get; set; }

        [JsonPropertyName("identities")]
        public List<ProveIdentityResult> Identities { get; set; }

        [JsonPropertyName("face_match")]
        public ProveFaceMatchResult FaceMatch { get; set; }

        [JsonPropertyName("address_verification")]
        public ProveAddressVerification AddressVerification { get; set; }

        [JsonPropertyName("bank_accounts")]
        public List<ProveBankAccount> BankAccounts { get; set; }

        [JsonPropertyName("blacklisted")]
        public bool? Blacklisted { get; set; }

        [JsonPropertyName("blacklist_reason")]
        public string BlacklistReason { get; set; }

        [JsonPropertyName("blacklist_code")]
        public int? BlacklistCode { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class ProveCustomerListResponse
    {
        [JsonPropertyName("customers")]
        public List<ProveCustomerResponse> Customers { get; set; }

        [JsonPropertyName("meta")]
        public ProvePaginationMeta Meta { get; set; }
    }

    public class ProveCustomerListQueryOptions
    {
        // Refit query strings use [AliasAs], not [JsonPropertyName].
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("status")] public string Status { get; set; }
        [AliasAs("kyc_level")] public string KycLevel { get; set; }
        [AliasAs("blacklisted")] public bool? Blacklisted { get; set; }
        [AliasAs("start")] public string Start { get; set; }
        [AliasAs("end")] public string End { get; set; }
    }

    public class ProvePaginationMeta
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("page")]
        public long Page { get; set; }

        [JsonPropertyName("previous")]
        public string Previous { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }
    }

    // ============ Blacklist / Whitelist ============

    public class BlacklistCustomerModel
    {
        [Required]
        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [Required]
        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        /// <summary>Blacklist reason code in the range 101-105 (per Mono Prove docs).</summary>
        [Required]
        [JsonPropertyName("code")]
        public int Code { get; set; }
    }

    public class WhitelistCustomerModel
    {
        [Required]
        [JsonPropertyName("reference")]
        public string Reference { get; set; }
    }

    // ============ Constants ============

    public class ProveKycLevelConstants
    {
        public const string Tier1 = "tier_1";
        public const string Tier2 = "tier_2";
        public const string Tier3 = "tier_3";
        public const string Custom = "custom";
    }

    public class ProveIdentityTypeConstants
    {
        public const string Bvn = "bvn";
        public const string Nin = "nin";
    }

    public class ProveBlacklistCodeConstants
    {
        // Mono Prove docs specify codes 101-105 without naming them; values
        // are exposed as-is so callers can pass whatever Mono returns for
        // their specific reason. Add named constants here once Mono publishes
        // the mapping.
        public const int Code101 = 101;
        public const int Code102 = 102;
        public const int Code103 = 103;
        public const int Code104 = 104;
        public const int Code105 = 105;
    }
}

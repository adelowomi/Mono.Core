using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Refit;

namespace Mono.Core.Services.DirectPay.Models
{
    public class CreateSubAccountModel
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [Required]
        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        /// <summary>NIP code of the destination bank.</summary>
        [Required]
        [JsonPropertyName("nip_code")]
        public string NipCode { get; set; }
    }

    public class SubAccountResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("nip_code")]
        public string NipCode { get; set; }

        [JsonPropertyName("balance")]
        public long? Balance { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class SubAccountListResponse
    {
        [JsonPropertyName("sub_accounts")]
        public List<SubAccountResponse> SubAccounts { get; set; }

        [JsonPropertyName("meta")]
        public PayoutPaginationMeta Meta { get; set; }
    }

    public class SubAccountListQueryOptions
    {
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("status")] public string Status { get; set; }
    }
}

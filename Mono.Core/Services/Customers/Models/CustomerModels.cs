using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Refit;

namespace Mono.Core.Customers
{
    public class CustomerIdentity
    {
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [Required]
        [JsonPropertyName("number")]
        public string Number { get; set; }
    }

    public class CreateIndividualCustomerModel
    {
        [Required]
        [JsonPropertyName("identity")]
        public CustomerIdentity Identity { get; set; }

        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = CustomerTypeConstants.Individual;

        [Required]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [Required]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }
    }

    public class CreateBusinessCustomerModel
    {
        [Required]
        [JsonPropertyName("identity")]
        public CustomerIdentity Identity { get; set; }

        [Required]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = CustomerTypeConstants.Business;

        [Required]
        [JsonPropertyName("business_name")]
        public string BusinessName { get; set; }

        [Required]
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [Required]
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
    }

    public class UpdateCustomerModel
    {
        [JsonPropertyName("identity")]
        public CustomerIdentity Identity { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("business_name")]
        public string BusinessName { get; set; }
    }

    // Query option classes use Refit's [AliasAs] (not [JsonPropertyName])
    // because Refit's query-string serializer reads C# property names directly
    // — it doesn't honor System.Text.Json attributes.
    public class CustomerListQueryOptions
    {
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("phone")] public string Phone { get; set; }
        [AliasAs("first_name")] public string FirstName { get; set; }
        [AliasAs("last_name")] public string LastName { get; set; }
        [AliasAs("start")] public string Start { get; set; }
        [AliasAs("end")] public string End { get; set; }
    }

    public class CustomerTransactionsQueryOptions
    {
        [AliasAs("period")] public string Period { get; set; }
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("account")] public string Account { get; set; }
    }

    public class LinkedAccountsQueryOptions
    {
        [AliasAs("page")] public int? Page { get; set; }
        [AliasAs("limit")] public int? Limit { get; set; }
        [AliasAs("customer")] public string Customer { get; set; }
        [AliasAs("institution")] public string Institution { get; set; }
    }

    public class CustomerResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("identity")]
        public CustomerIdentity Identity { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("business_name")]
        public string BusinessName { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class CustomerListResponse
    {
        [JsonPropertyName("customers")]
        public List<CustomerResponse> Customers { get; set; }

        [JsonPropertyName("meta")]
        public CustomerListMeta Meta { get; set; }
    }

    public class CustomerListMeta
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

    public class CustomerTransaction
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; }

        [JsonPropertyName("account")]
        public string Account { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class CustomerTransactionsResponse
    {
        [JsonPropertyName("transactions")]
        public List<CustomerTransaction> Transactions { get; set; }

        [JsonPropertyName("meta")]
        public CustomerListMeta Meta { get; set; }
    }

    public class LinkedAccountSummary
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("balance")]
        public long? Balance { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("institution")]
        public LinkedAccountInstitution Institution { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class LinkedAccountInstitution
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class LinkedAccountsResponse
    {
        [JsonPropertyName("accounts")]
        public List<LinkedAccountSummary> Accounts { get; set; }

        [JsonPropertyName("meta")]
        public CustomerListMeta Meta { get; set; }
    }

    public class CustomerTypeConstants
    {
        public const string Individual = "individual";
        public const string Business = "business";
    }

    public class CustomerIdentityTypeConstants
    {
        public const string Bvn = "bvn";
        public const string Nin = "nin";
    }
}

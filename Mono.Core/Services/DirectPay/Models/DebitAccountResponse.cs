using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class AccountDebited
    {
        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank_name")]
        public string BankName { get; set; }
    }

    public class Beneficiary
    {
        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank_name")]
        public string BankName { get; set; }
    }

    public class DebitAccountResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("mandate")]
        public string Mandate { get; set; }

        [JsonPropertyName("reference_number")]
        public string ReferenceNumber { get; set; }

        [JsonPropertyName("account_debited")]
        public AccountDebited AccountDebited { get; set; }

        [JsonPropertyName("beneficiary")]
        public Beneficiary Beneficiary { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
    }
}
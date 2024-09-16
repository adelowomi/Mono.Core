using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class Institution
    {
        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("nip_code")]
        public string NipCode { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class MonoMandate
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("approved")]
        public bool? Approved { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("institution")]
        public Institution Institution { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("narration")]
        public string Narration { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }
    }

    public class MandateRequestQueryOptions
    {
        [JsonPropertyName("page")]
        public int? Page { get; set; }

        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

    }
}

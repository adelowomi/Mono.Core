using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class Method
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class OtpDestinations
    {
        [JsonPropertyName("session")]
        public string Session { get; set; }

        [JsonPropertyName("methods")]
        public List<Method> Methods { get; set; }
    }

    public class TokenizedMandateResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank")]
        public string Bank { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("otp_destinations")]
        public OtpDestinations OtpDestinations { get; set; }
        [JsonPropertyName("transfer_destinations")]
        public List<TransferDestination> TransferDestinations { get; set; }
    }

    public class TransferDestination
    {
        [JsonPropertyName("bank_name")]
        public string BankName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
}
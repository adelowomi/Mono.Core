using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Services.DirectPay.Models
{
    public class CreateMandateModel
    {
        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("fee_bearer")]
        public string FeeBearer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("verification_method")]
        public string VerificationMethod { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public string EndDate { get; set; }

        [JsonPropertyName("meta")]
        public object Meta { get; set; }
    }

    public class FeeBearerConstants
    {
        public const string Business = "business";
        public const string Customer = "customer";
    }

    public class MandateTypeConstants
    {
        public const string EMandate = "emandate";
        public const string Sweep = "sweep";
    }

    public class DebitTypeConstants
    {
        public const string Variable = "variable";
        public const string Fixed = "fixed";
    }

    public class VerificationMethodConstants
    {
        public const string TransferVerification = "transfer_verification";
        public const string SelfieVerification = "selfie_verification";
    }
}



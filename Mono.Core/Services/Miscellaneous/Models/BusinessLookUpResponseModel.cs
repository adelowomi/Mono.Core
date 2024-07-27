using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Miscellaneous
{
    public class BusinessLookUpResponseModel
    {
        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("lga")]
        public string Lga { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("rcNumber")] 
        public string RcNumber { get; set; }

        [JsonPropertyName("classificationId")]
        public long ClassificationId { get; set; }

        [JsonPropertyName("branchAddress")]
        public string BranchAddress { get; set; }

        [JsonPropertyName("registrationDate")]
        public string RegistrationDate { get; set; }

        [JsonPropertyName("companyStatus")]
        public string CompanyStatus { get; set; }

        [JsonPropertyName("approvedName")]
        public string ApprovedName { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("code")] 
        public string Code { get; set; }
    }
}

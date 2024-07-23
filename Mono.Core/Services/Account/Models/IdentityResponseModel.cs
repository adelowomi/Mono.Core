using System.Text.Json.Serialization;

namespace Mono.Core.Accounts
{
    public class IdentityResponseModel
    {
        [JsonPropertyName("fullName")]
        public string FullName { get; set; }
    
        [JsonPropertyName("email")]
        public string Email { get; set; }
    
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
    
        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    
        [JsonPropertyName("dob")]
        public string Dob { get; set; }
    
        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }
    
        [JsonPropertyName("maritalStatus")]
        public object MaritalStatus { get; set; }
    
        [JsonPropertyName("addressLine1")]
        public string AddressLine1 { get; set; }
    
        [JsonPropertyName("addressLine2")]
        public string AddressLine2 { get; set; }
    }
}

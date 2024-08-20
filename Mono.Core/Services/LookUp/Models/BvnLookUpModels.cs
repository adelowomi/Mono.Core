using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mono.Core.LookUp
{
    public class InitiateBvnLookUpModel
    {
        [Required]
        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; } = ScopeConstants.Identity;
    }

    public class ScopeConstants
    {
        public const string Identity = "IDENTITY";
        public const string BankAccounts = "BANK_ACCOUNTS";
    }

    public class Methods
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("hint")]
        public string Hint { get; set; }
    }

    public class InitiateBvnLookUpResponseModel
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("methods")]
        public List<Methods> Methods { get; set; }
    }

    public class VerifyBvnLookUpOtpModel
    {
        [Required]
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [Required]
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [Required]
        [JsonPropertyName("otp")]
        public string Otp { get; set; }

        [Required]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }
    }

    public class BvnDetailsModel
    {
        [JsonPropertyName("otp")]
        public string Otp { get; set; }
    }

    public class BvnDetailsResponse
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; }

        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("phone_number_2")]
        public object PhoneNumber2 { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("state_of_origin")]
        public string StateOfOrigin { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("nin")]
        public string Nin { get; set; }

        [JsonPropertyName("registration_date")]
        public string RegistrationDate { get; set; }

        [JsonPropertyName("lga_of_origin")]
        public string LgaOfOrigin { get; set; }

        [JsonPropertyName("lga_of_Residence")]
        public string LgaOfResidence { get; set; }

        [JsonPropertyName("marital_status")]
        public string MaritalStatus { get; set; }

        [JsonPropertyName("watch_listed")]
        public bool WatchListed { get; set; }

        [JsonPropertyName("photoId")]
        public string PhotoId { get; set; }
    }
}


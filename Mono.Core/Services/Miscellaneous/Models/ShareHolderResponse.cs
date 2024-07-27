using System;
using System.Text.Json.Serialization;

namespace Mono.Core
{
    public class ShareHolderResponseModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        [JsonPropertyName("otherName")]
        public string OtherName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("formerNationality")]
        public string FormerNationality { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("occupation")]
        public string Occupation { get; set; }

        [JsonPropertyName("formerName")]
        public string FormerName { get; set; }

        [JsonPropertyName("corporationName")]
        public string CorporationName { get; set; }

        [JsonPropertyName("rcNumber")]
        public string RcNumber { get; set; }

        [JsonPropertyName("corporationCompany")]
        public CorporationCompany CorporationCompany { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("accreditationnumber")]
        public string Accreditationnumber { get; set; }

        [JsonPropertyName("formType")]
        public string FormType { get; set; }

        [JsonPropertyName("numSharesAlloted")]
        public string NumSharesAlloted { get; set; }

        [JsonPropertyName("typeOfShares")]
        public string TypeOfShares { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("dateOfAppointment")]
        public string DateOfAppointment { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("formerSurname")]
        public string FormerSurname { get; set; }

        [JsonPropertyName("formerFirstName")]
        public string FormerFirstName { get; set; }

        [JsonPropertyName("formerOtherName")]
        public string FormerOtherName { get; set; }

        [JsonPropertyName("identityNumber")]
        public string IdentityNumber { get; set; }

        [JsonPropertyName("otherDirectorshipDetails")]
        public string OtherDirectorshipDetails { get; set; }

        [JsonPropertyName("affiliateTypeFk")]
        public AffiliateTypeFk AffiliateTypeFk { get; set; }

        [JsonPropertyName("countryFk")]
        public CountryFk CountryFk { get; set; }

        [JsonPropertyName("lga")]
        public string Lga { get; set; }

        [JsonPropertyName("isCorporate")]
        public string IsCorporate { get; set; }

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("streetNumber")]
        public string StreetNumber { get; set; }

        [JsonPropertyName("isChairman")]
        public string IsChairman { get; set; }

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("formerNameType")]
        public string FormerNameType { get; set; }

        [JsonPropertyName("affiliatesResidentialAddress")]
        public string AffiliatesResidentialAddress { get; set; }

        [JsonPropertyName("affiliatesPscInformation")]
        public string AffiliatesPscInformation { get; set; }

        [JsonPropertyName("isPublicUser")]
        public string IsPublicUser { get; set; }
    }

    public class CorporationCompany
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("approvedName")]
        public string ApprovedName { get; set; }

        [JsonPropertyName("stringives")]
        public string stringives { get; set; }

        [JsonPropertyName("registrationApproved")]
        public string RegistrationApproved { get; set; }

        [JsonPropertyName("rcNumber")]
        public string RcNumber { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("branchAddress")]
        public string BranchAddress { get; set; }

        [JsonPropertyName("registrationSubmitted")]
        public string RegistrationSubmitted { get; set; }

        [JsonPropertyName("businessCommencementDate")]
        public string BusinessCommencementDate { get; set; }

        [JsonPropertyName("reservationSerialNo")]
        public string ReservationSerialNo { get; set; }

        [JsonPropertyName("active")]
        public string Active { get; set; }

        [JsonPropertyName("registrationSerialNo")]
        public string RegistrationSerialNo { get; set; }

        [JsonPropertyName("registrationApprovedByRg")]
        public string RegistrationApprovedByRg { get; set; }

        [JsonPropertyName("trackingId")]
        public string TrackingId { get; set; }

        [JsonPropertyName("dateOfReservation")]
        public string DateOfReservation { get; set; }

        [JsonPropertyName("needsProficiencyDocs")]
        public string NeedsProficiencyDocs { get; set; }

        [JsonPropertyName("availabilityCode")]
        public string AvailabilityCode { get; set; }

        [JsonPropertyName("nameAvailability")]
        public string NameAvailability { get; set; }

        [JsonPropertyName("forwardedToAo")]
        public string ForwardedToAo { get; set; }

        [JsonPropertyName("forwardedToRgs")]
        public string ForwardedToRgs { get; set; }

        [JsonPropertyName("classificationFk")]
        public string ClassificationFk { get; set; }

        [JsonPropertyName("natureOfBusinessFk")]
        public string NatureOfBusinessFk { get; set; }

        [JsonPropertyName("companyTypeFk")]
        public string CompanyTypeFk { get; set; }

        [JsonPropertyName("registrationDate")]
        public string RegistrationDate { get; set; }

        [JsonPropertyName("review_status")]
        public string ReviewStatus { get; set; }

        [JsonPropertyName("isOldRecord")]
        public string IsOldRecord { get; set; }

        [JsonPropertyName("regPortalUserFk")]
        public string RegPortalUserFk { get; set; }

        [JsonPropertyName("approvedNameForSearch")]
        public string ApprovedNameForSearch { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("companyStatus")]
        public string CompanyStatus { get; set; }

        [JsonPropertyName("isForChangeOfName")]
        public string IsForChangeOfName { get; set; }

        [JsonPropertyName("isChangeOfNameSubmited")]
        public string IsChangeOfNameSubmited { get; set; }

        [JsonPropertyName("dateCreated")]
        public string DateCreated { get; set; }

        [JsonPropertyName("isSubmittedForPostInc")]
        public string IsSubmittedForPostInc { get; set; }

        [JsonPropertyName("queryCode")]
        public string QueryCode { get; set; }

        [JsonPropertyName("queryHistoryFk")]
        public string QueryHistoryFk { get; set; }

        [JsonPropertyName("reasonForDisapproval")]
        public string ReasonForDisapproval { get; set; }

        [JsonPropertyName("dateOfApproval")]
        public string DateOfApproval { get; set; }

        [JsonPropertyName("date_of_query")]
        public string DateOfQuery { get; set; }

        [JsonPropertyName("approver")]
        public string Approver { get; set; }

        [JsonPropertyName("updatingOffice")]
        public string UpdatingOffice { get; set; }

        [JsonPropertyName("hasBeenUpdated")]
        public string HasBeenUpdated { get; set; }

        [JsonPropertyName("dateOfUpdate")]
        public string DateOfUpdate { get; set; }

        [JsonPropertyName("approvedForUpdate")]
        public string ApprovedForUpdate { get; set; }

        [JsonPropertyName("dateOfUpdateApproval")]
        public string DateOfUpdateApproval { get; set; }

        [JsonPropertyName("approvedForUpdateByFk")]
        public string ApprovedForUpdateByFk { get; set; }

        [JsonPropertyName("updatedByFk")]
        public string UpdatedByFk { get; set; }

        [JsonPropertyName("registrationSubmissionDate")]
        public string RegistrationSubmissionDate { get; set; }

        [JsonPropertyName("financialYearEnd")]
        public string FinancialYearEnd { get; set; }

        [JsonPropertyName("streetNumber")]
        public string StreetNumber { get; set; }

        [JsonPropertyName("lga")]
        public string Lga { get; set; }

        [JsonPropertyName("delisting_status")]
        public string DelistingStatus { get; set; }

        [JsonPropertyName("head_office_address")]
        public string HeadOfficeAddress { get; set; }

        [JsonPropertyName("companyHeadOffice")]
        public string CompanyHeadOffice { get; set; }

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("progressLevel")]
        public string ProgressLevel { get; set; }

        [JsonPropertyName("batch")]
        public string Batch { get; set; }

        [JsonPropertyName("queried")]
        public string Queried { get; set; }

        [JsonPropertyName("firsTin")]
        public string FirsTin { get; set; }

        [JsonPropertyName("natureOfBusiness")]
        public string NatureOfBusiness { get; set; }

        [JsonPropertyName("formerName")]
        public string FormerName { get; set; }

        [JsonPropertyName("resolved")]
        public string Resolved { get; set; }

        [JsonPropertyName("durationInQueue")]
        public string DurationInQueue { get; set; }

        [JsonPropertyName("timeTakeTobeProcessed")]
        public string TimeTakeTobeProcessed { get; set; }

        [JsonPropertyName("rrr")]
        public string Rrr { get; set; }

        [JsonPropertyName("paymentDate")]
        public string PaymentDate { get; set; }

        [JsonPropertyName("enteredBy")]
        public string EnteredBy { get; set; }

        [JsonPropertyName("activeForPostInc")]
        public string ActiveForPostInc { get; set; }

        [JsonPropertyName("shareCapital")]
        public string ShareCapital { get; set; }

        [JsonPropertyName("shareCapitalInWords")]
        public string ShareCapitalInWords { get; set; }

        [JsonPropertyName("dividedInto")]
        public string DividedInto { get; set; }

        [JsonPropertyName("ofEach")]
        public string OfEach { get; set; }

        [JsonPropertyName("consentCode")]
        public string ConsentCode { get; set; }

        [JsonPropertyName("jtbTINStatus")]
        public string JtbTINStatus { get; set; }

        [JsonPropertyName("fullAddress")]
        public string FullAddress { get; set; }
    }

    public  class AffiliateTypeFk
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
    
    public class CountryFk
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    
    }
}

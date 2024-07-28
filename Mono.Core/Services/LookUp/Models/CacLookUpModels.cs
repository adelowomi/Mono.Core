using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mono.Core.LookUp
{
    public class CacLookUpModels
    {

    }

    public class BusinessDetails
    {
        [JsonPropertyName("nature_of_business_name")]
        public object NatureOfBusinessName { get; set; }

        [JsonPropertyName("classification_id")]
        public int ClassificationId { get; set; }

        [JsonPropertyName("delisting_status")]
        public object DelistingStatus { get; set; }

        [JsonPropertyName("company_type_name")]
        public object CompanyTypeName { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("classification")]
        public object Classification { get; set; }

        [JsonPropertyName("business_commencement_date")]
        public DateTime BusinessCommencementDate { get; set; }

        [JsonPropertyName("approved_name")]
        public string ApprovedName { get; set; }

        [JsonPropertyName("branch_address")]
        public string BranchAddress { get; set; }

        [JsonPropertyName("registration_approved")]
        public bool RegistrationApproved { get; set; }

        [JsonPropertyName("head_office_address")]
        public object HeadOfficeAddress { get; set; }

        [JsonPropertyName("objectives")]
        public object Objectives { get; set; }

        [JsonPropertyName("registration_date")]
        public DateTime RegistrationDate { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("lga")]
        public string Lga { get; set; }

        [JsonPropertyName("rc_number")]
        public string RcNumber { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }

    public class AffiliateTypeFk
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class CountryFk
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }
    }

    public class ProcessTypeFk
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("type")]
        public object Type { get; set; }

        [JsonPropertyName("product_id")]
        public string ProductId { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }
    }

    public class OfficialDetails
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        [JsonPropertyName("other_name")]
        public string OtherName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("former_nationality")]
        public string FormerNationality { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("occupation")]
        public string Occupation { get; set; }

        [JsonPropertyName("former_name")]
        public string FormerName { get; set; }

        [JsonPropertyName("corporation_name")]
        public string CorporationName { get; set; }

        [JsonPropertyName("rc_number")]
        public string RcNumber { get; set; }

        [JsonPropertyName("corporation_company")]
        public object CorporationCompany { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("pobox")]
        public object Pobox { get; set; }

        [JsonPropertyName("accreditationnumber")]
        public string Accreditationnumber { get; set; }

        [JsonPropertyName("is_lawyer")]
        public object IsLawyer { get; set; }

        [JsonPropertyName("last_visit")]
        public int LastVisit { get; set; }

        [JsonPropertyName("form_type")]
        public string FormType { get; set; }

        [JsonPropertyName("is_presenter")]
        public object IsPresenter { get; set; }

        [JsonPropertyName("is_chairman")]
        public object IsChairman { get; set; }

        [JsonPropertyName("num_shares_alloted")]
        public int NumSharesAlloted { get; set; }

        [JsonPropertyName("type_of_shares")]
        public string TypeOfShares { get; set; }

        [JsonPropertyName("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("date_of_termination")]
        public object DateOfTermination { get; set; }

        [JsonPropertyName("date_of_appointment")]
        public object DateOfAppointment { get; set; }

        [JsonPropertyName("date_of_change_of_address")]
        public object DateOfChangeOfAddress { get; set; }

        [JsonPropertyName("former_address")]
        public object FormerAddress { get; set; }

        [JsonPropertyName("former_postal")]
        public object FormerPostal { get; set; }

        [JsonPropertyName("former_surname")]
        public string FormerSurname { get; set; }

        [JsonPropertyName("former_first_name")]
        public string FormerFirstName { get; set; }

        [JsonPropertyName("former_other_name")]
        public string FormerOtherName { get; set; }

        [JsonPropertyName("date_of_status_change")]
        public object DateOfStatusChange { get; set; }

        [JsonPropertyName("identity_number")]
        public string IdentityNumber { get; set; }

        [JsonPropertyName("identity_issue_state")]
        public object IdentityIssueState { get; set; }

        [JsonPropertyName("other_directorship_details")]
        public string OtherDirectorshipDetails { get; set; }

        [JsonPropertyName("portal_user_fk")]
        public object PortalUserFk { get; set; }

        [JsonPropertyName("affiliates_fk")]
        public object AffiliatesFk { get; set; }

        [JsonPropertyName("process_type_fk")]
        public ProcessTypeFk ProcessTypeFk { get; set; }

        [JsonPropertyName("company")]
        public object Company { get; set; }

        [JsonPropertyName("same_person_as_fk")]
        public object SamePersonAsFk { get; set; }

        [JsonPropertyName("nature_of_app_or_discharge")]
        public object NatureOfAppOrDischarge { get; set; }

        [JsonPropertyName("is_designated")]
        public object IsDesignated { get; set; }

        [JsonPropertyName("end_of_appointment")]
        public object EndOfAppointment { get; set; }

        [JsonPropertyName("appointed_by")]
        public object AppointedBy { get; set; }

        [JsonPropertyName("date_of_deed_of_discharge")]
        public object DateOfDeedOfDischarge { get; set; }

        [JsonPropertyName("date_of_resolution")]
        public object DateOfResolution { get; set; }

        [JsonPropertyName("country_fk")]
        public CountryFk CountryFk { get; set; }

        [JsonPropertyName("country_of_residence")]
        public object CountryOfResidence { get; set; }

        [JsonPropertyName("is_carried_over_from_name_avai")]
        public object IsCarriedOverFromNameAvai { get; set; }

        [JsonPropertyName("lga")]
        public object Lga { get; set; }

        [JsonPropertyName("corporation_registration_date")]
        public object CorporationRegistrationDate { get; set; }

        [JsonPropertyName("is_company_deleted")]
        public object IsCompanyDeleted { get; set; }

        [JsonPropertyName("government_organisation_name")]
        public object GovernmentOrganisationName { get; set; }

        [JsonPropertyName("foreign_organisation_name")]
        public object ForeignOrganisationName { get; set; }

        [JsonPropertyName("company_street_address")]
        public object CompanyStreetAddress { get; set; }

        [JsonPropertyName("company_state")]
        public object CompanyState { get; set; }

        [JsonPropertyName("company_city")]
        public object CompanyCity { get; set; }

        [JsonPropertyName("is_corporate")]
        public object IsCorporate { get; set; }

        [JsonPropertyName("county_of_incorporation_fk")]
        public object CountyOfIncorporationFk { get; set; }

        [JsonPropertyName("nationality")]
        public object Nationality { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("postcode")]
        public string Postcode { get; set; }

        [JsonPropertyName("street_number")]
        public string StreetNumber { get; set; }

        [JsonPropertyName("affiliates_residential_address")]
        public object AffiliatesResidentialAddress { get; set; }

        [JsonPropertyName("affiliates_psc_information")]
        public int AffiliatesPscInformation { get; set; }

        [JsonPropertyName("legal_owners_of_interests")]
        public List<object> LegalOwnersOfInterests { get; set; }

        [JsonPropertyName("legal_owners_of_voting_rights")]
        public List<object> LegalOwnersOfVotingRights { get; set; }

        [JsonPropertyName("stock_exchange_soes")]
        public List<object> StockExchangeSoes { get; set; }

        [JsonPropertyName("approved_for_notice_of_psc")]
        public object ApprovedForNoticeOfPsc { get; set; }

        [JsonPropertyName("company_address2")]
        public string CompanyAddress2 { get; set; }

        [JsonPropertyName("full_address2")]
        public string FullAddress2 { get; set; }

        [JsonPropertyName("affiliate_type_fk")]
        public AffiliateTypeFk AffiliateTypeFk { get; set; }
    }

    public class PreviousAddressResponse
    {
        [JsonPropertyName("approved_name")]
        public string ApprovedName { get; set; }

        [JsonPropertyName("previous_address")]
        public string PreviousAddress { get; set; }

        [JsonPropertyName("street_name")]
        public string StreetName { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("submission_date")]
        public DateTime SubmissionDate { get; set; }

        [JsonPropertyName("approval_date")]
        public DateTime ApprovalDate { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }
    }

    public class ChangeOfNameResponse
    {
        [JsonPropertyName("persist_master_id")]
        public int PersistMasterId { get; set; }

        [JsonPropertyName("new_name")]
        public string NewName { get; set; }

        [JsonPropertyName("former_name")]
        public string FormerName { get; set; }

        [JsonPropertyName("approval_date")]
        public DateTime ApprovalDate { get; set; }
    }

    public class Bank
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("nip_code")]
        public string NipCode { get; set; }
    }

    public class BanksResponse
    {
        [JsonPropertyName("banks")]
        public List<Bank> Banks { get; set; }
    }

    public class AddressLookUpRequestModel
    {
        [JsonPropertyName("meter_number")]
        public string MeterNumber { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public class AddressLookUpResponseModel
    {
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        [JsonPropertyName("house_address")]
        public string HouseAddress { get; set; }

        [JsonPropertyName("house_owner")]
        public string HouseOwner { get; set; }

        [JsonPropertyName("confidence_level")]
        public int ConfidenceLevel { get; set; }

        [JsonPropertyName("disco_code")]
        public string DiscoCode { get; set; }
    }

    public class InternationalPassportRequestModel
    {
        [JsonPropertyName("passport_number")]
        public string PassportNumber { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
    }
    public class InternationalPassportResponse
    {
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("reference_id")]
        public string ReferenceId { get; set; }

        [JsonPropertyName("issued_date")]
        public string IssuedDate { get; set; }

        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonPropertyName("document_type")]
        public string DocumentType { get; set; }

        [JsonPropertyName("issued_at")]
        public string IssuedAt { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; }

        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }
    }
    public class TinRequestModel
    {
        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("channel")]
        public string Channel { get; set; }
    }
    public class TinResponseModel
    {
        [JsonPropertyName("taxpayer_name")]
        public string TaxpayerName { get; set; }

        [JsonPropertyName("cac_reg_number")]
        public string CacRegNumber { get; set; }

        [JsonPropertyName("firstin")]
        public string Firstin { get; set; }

        [JsonPropertyName("jittin")]
        public string Jittin { get; set; }

        [JsonPropertyName("tax_office")]
        public string TaxOffice { get; set; }

        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }

    public class NinRequestModel
    {
        [JsonPropertyName("nin")]
        public string Nin { get; set; }
    }
    public class NinResponseModel
    {
        [JsonPropertyName("birthcountry")]
        public string Birthcountry { get; set; }

        [JsonPropertyName("birthdate")]
        public string Birthdate { get; set; }

        [JsonPropertyName("birthlga")]
        public string Birthlga { get; set; }

        [JsonPropertyName("birthstate")]
        public string Birthstate { get; set; }

        [JsonPropertyName("educationallevel")]
        public string Educationallevel { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("employmentstatus")]
        public string Employmentstatus { get; set; }

        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("heigth")]
        public string Heigth { get; set; }

        [JsonPropertyName("maritalstatus")]
        public string Maritalstatus { get; set; }

        [JsonPropertyName("middlename")]
        public string Middlename { get; set; }

        [JsonPropertyName("nin")]
        public string Nin { get; set; }

        [JsonPropertyName("nok_address1")]
        public string NokAddress1 { get; set; }

        [JsonPropertyName("nok_address2")]
        public string NokAddress2 { get; set; }

        [JsonPropertyName("nok_firstname")]
        public string NokFirstname { get; set; }

        [JsonPropertyName("nok_lga")]
        public string NokLga { get; set; }

        [JsonPropertyName("nok_middlename")]
        public string NokMiddlename { get; set; }

        [JsonPropertyName("nok_postalcode")]
        public string NokPostalcode { get; set; }

        [JsonPropertyName("nok_state")]
        public string NokState { get; set; }

        [JsonPropertyName("nok_surname")]
        public string NokSurname { get; set; }

        [JsonPropertyName("nok_town")]
        public string NokTown { get; set; }

        [JsonPropertyName("ospokenlang")]
        public string Ospokenlang { get; set; }

        [JsonPropertyName("pfirstname")]
        public string Pfirstname { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }

        [JsonPropertyName("pmiddlename")]
        public string Pmiddlename { get; set; }

        [JsonPropertyName("profession")]
        public string Profession { get; set; }

        [JsonPropertyName("psurname")]
        public string Psurname { get; set; }

        [JsonPropertyName("religion")]
        public string Religion { get; set; }

        [JsonPropertyName("residence_address")]
        public string ResidenceAddress { get; set; }

        [JsonPropertyName("residence_lga")]
        public string ResidenceLga { get; set; }

        [JsonPropertyName("residence_state")]
        public string ResidenceState { get; set; }

        [JsonPropertyName("residence_town")]
        public string ResidenceTown { get; set; }

        [JsonPropertyName("residencestatus")]
        public string Residencestatus { get; set; }

        [JsonPropertyName("self_origin_lga")]
        public string SelfOriginLga { get; set; }

        [JsonPropertyName("self_origin_place")]
        public string SelfOriginPlace { get; set; }

        [JsonPropertyName("self_origin_state")]
        public string SelfOriginState { get; set; }

        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("spoken_language")]
        public string SpokenLanguage { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("telephoneno")]
        public string Telephoneno { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("userid")]
        public string Userid { get; set; }

        [JsonPropertyName("vnin")]
        public string Vnin { get; set; }

        [JsonPropertyName("central_iD")]
        public string CentralID { get; set; }

        [JsonPropertyName("tracking_id")]
        public string TrackingId { get; set; }
    }
    public class DriversLicenseRequestModel
    {
        [JsonPropertyName("license_number")]
        public string LicenseNumber { get; set; }

        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
    }
    public class DriversLicenseResponse
    {
        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("photo")]
        public string Photo { get; set; }

        [JsonPropertyName("license_no")]
        public string LicenseNo { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; }

        [JsonPropertyName("issued_date")]
        public string IssuedDate { get; set; }

        [JsonPropertyName("expiry_date")]
        public string ExpiryDate { get; set; }

        [JsonPropertyName("state_ofIssue")]
        public string StateOfIssue { get; set; }

        [JsonPropertyName("birth_date")]
        public string BirthDate { get; set; }
    }
    public class AccountRequestModel
    {
        [JsonPropertyName("nip_code")]
        public string NipCode { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }
    }

    public class AccountResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("bank")]
        public Bank Bank { get; set; }
    }

    public class CreditHistoryModel
    {
        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }
    }

    public class AddressHistory
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("date_reported")]
        public string DateReported { get; set; }
    }

    public class CreditHistory
    {
        [JsonPropertyName("institution")]
        public string Institution { get; set; }

        [JsonPropertyName("history")]
        public List<History> History { get; set; }
    }

    public class History
    {
        [JsonPropertyName("date_opened")]
        public string DateOpened { get; set; }

        [JsonPropertyName("opening_balance")]
        public int OpeningBalance { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("performance_status")]
        public string PerformanceStatus { get; set; }

        [JsonPropertyName("tenor")]
        public int Tenor { get; set; }

        [JsonPropertyName("closed_date")]
        public string ClosedDate { get; set; }

        [JsonPropertyName("loan_status")]
        public string LoanStatus { get; set; }

        [JsonPropertyName("repayment_frequency")]
        public string RepaymentFrequency { get; set; }

        [JsonPropertyName("repayment_amount")]
        public int RepaymentAmount { get; set; }

        [JsonPropertyName("repayment_schedule")]
        public List<RepaymentSchedule> RepaymentSchedule { get; set; }
    }

    public class Identification
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("no")]
        public string No { get; set; }
    }

    public class Profile
    {
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("address_history")]
        public List<AddressHistory> AddressHistory { get; set; }

        [JsonPropertyName("email_addresses")]
        public List<string> EmailAddresses { get; set; }

        [JsonPropertyName("phone_numbers")]
        public List<string> PhoneNumbers { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("identifications")]
        public List<Identification> Identifications { get; set; }
    }

    public class RepaymentSchedule
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }

    public class CreditHistoryResponse
    {
        [JsonPropertyName("providers")]
        public List<string> Providers { get; set; }

        [JsonPropertyName("profile")]
        public Profile Profile { get; set; }

        [JsonPropertyName("credit_history")]
        public List<CreditHistory> CreditHistory { get; set; }
    }

    public class CreditHistoryRequestModel
    {
        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }
    }

    public class CreditHistoryProvidersConstants
    {
        public const string CRC = "CRC";
        public const string XDS = "XDS";
        public const string ALL = "ALL";
    }

    public class MashUpRequestModel
    {
        [JsonPropertyName("nin")]
        public string Nin { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }
    }

    public class Biometrics
    {
        [JsonPropertyName("photo")]
        public string Photo { get; set; }
    }

    public class IdentificationNumbers
    {
        [JsonPropertyName("nin")]
        public string Nin { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }
    }

    public class PersonalInformation
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("middle_name")]
        public string MiddleName { get; set; }

        [JsonPropertyName("surname")]
        public string Surname { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("dob")]
        public string Dob { get; set; }

        [JsonPropertyName("birth_date")]
        public string BirthDate { get; set; }

        [JsonPropertyName("birth_country")]
        public string BirthCountry { get; set; }

        [JsonPropertyName("birth_state")]
        public string BirthState { get; set; }

        [JsonPropertyName("birth_lga")]
        public string BirthLga { get; set; }

        [JsonPropertyName("marital_status")]
        public string MaritalStatus { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("telephone_no")]
        public string TelephoneNo { get; set; }

        [JsonPropertyName("occupation")]
        public string Occupation { get; set; }

        [JsonPropertyName("lga_of_origin")]
        public string LgaOfOrigin { get; set; }

        [JsonPropertyName("state_of_origin")]
        public string StateOfOrigin { get; set; }

        [JsonPropertyName("watch_listed")]
        public bool WatchListed { get; set; }
    }

    public class ResidenceInformation
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("town")]
        public string Town { get; set; }

        [JsonPropertyName("lga")]
        public string Lga { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("residence_status")]
        public string ResidenceStatus { get; set; }
    }

    public class MashUpResponse
    {
        [JsonPropertyName("personal_information")]
        public PersonalInformation PersonalInformation { get; set; }

        [JsonPropertyName("identification_numbers")]
        public IdentificationNumbers IdentificationNumbers { get; set; }

        [JsonPropertyName("residence_information")]
        public ResidenceInformation ResidenceInformation { get; set; }

        [JsonPropertyName("biometrics")]
        public Biometrics Biometrics { get; set; }
    }



}

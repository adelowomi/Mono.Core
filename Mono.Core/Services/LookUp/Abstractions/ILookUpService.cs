using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Mono.Core.LookUp
{
    public interface ILookUpService
    {
        #region Bvn LookUp
        // /lookup/bvn/initiate
        [Post("/lookup/bvn/initiate")]
        Task<IApiResponse<MonoStandardResponse<InitiateBvnLookUpResponseModel>>> InitiateBvnLookUp([Body] InitiateBvnLookUpModel bvnInitiateRequestModel, CancellationToken cancellationToken = default);

        // /lookup/bvn/verify-otp (renamed from /verify per Mono BVN iGree docs)
        [Post("/lookup/bvn/verify-otp")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> VerifyBvnLookUp([Body] VerifyBvnLookUpOtpModel bvnVerifyRequestModel, [Header("x-session-id")] string sessionId, CancellationToken cancellationToken = default);

        // /lookup/bvn/fetch-bvn (renamed from /details per Mono BVN iGree docs)
        [Post("/lookup/bvn/fetch-bvn")]
        Task<IApiResponse<MonoStandardResponse<BvnDetailsResponse>>> GetBvnDetails([Body] BvnDetailsModel bvnDetailsModel,[Header("x-session-id")] string sessionId, CancellationToken cancellationToken = default);
        #endregion

        #region cac LookUp NB: These endpoints are in the v3 and need to be implemented in the v3 of the API
        // /lookup/cac
        [Get("/lookup/cac")]
        Task<IApiResponse<MonoStandardResponse<List<BusinessDetails>>>> GetCacLookUp([Query] string search, CancellationToken cancellationToken = default);

        // /lookup/cac/company/2909515  // share holder details
        [Get("/lookup/cac/company/{businessId}")]
        Task<IApiResponse<MonoStandardResponse<List<OfficialDetails>>>> GetCacCompany(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/company/266914/previous-address // previous address
        [Obsolete("Deprecated by Mono. Use the CAC Profile endpoint when it is implemented in a future release.")]
        [Get("/lookup/cac/company/{businessId}/previous-address")]
        Task<IApiResponse<MonoStandardResponse<PreviousAddressResponse>>> GetPreviousAddress(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/company/322175/change-of-name
        [Obsolete("Deprecated by Mono. Use the CAC Profile endpoint when it is implemented in a future release.")]
        [Get("/lookup/cac/company/{businessId}/change-of-name")]
        Task<IApiResponse<MonoStandardResponse<ChangeOfNameResponse>>> GetChangeOfName(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/company/322175/secretary
        [Get("/lookup/cac/company/{businessId}/secretary")]
        Task<IApiResponse<MonoStandardResponse<List<OfficialDetails>>>> GetSecretary(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/company/322175/directors
        [Get("/lookup/cac/company/{businessId}/directors")]
        Task<IApiResponse<MonoStandardResponse<List<OfficialDetails>>>> GetDirectors(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/company/{businessId}/psc — Persons with Significant Control
        [Get("/lookup/cac/company/{businessId}/psc")]
        Task<IApiResponse<MonoStandardResponse<List<CacPscEntry>>>> GetCacPsc(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/profile/{rcNumber} — aggregate company profile
        [Get("/lookup/cac/profile/{rcNumber}")]
        Task<IApiResponse<MonoStandardResponse<CacProfileResponse>>> GetCacProfile(string rcNumber, CancellationToken cancellationToken = default);

        // /lookup/cac/company/{businessId}/status-report — binary PDF download
        [Get("/lookup/cac/company/{businessId}/status-report")]
        Task<HttpResponseMessage> GetCacStatusReport(string businessId, CancellationToken cancellationToken = default);

        #endregion

        #region Others LookUp
        // lookup/banks
        [Get("/lookup/banks")]
        Task<IApiResponse<MonoStandardResponse<BanksResponse>>> GetBanks(CancellationToken cancellationToken = default);

        // /lookup/address
        [Post("/lookup/address")]
        Task<IApiResponse<MonoStandardResponse<AddressLookUpResponseModel>>> GetAddress([Body] AddressLookUpRequestModel addressLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/intl-passport
        [Post("/lookup/intl-passport")]
        Task<IApiResponse<MonoStandardResponse<InternationalPassportResponse>>> GetPassport([Body] InternationalPassportRequestModel passportLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/tin
        [Post("/lookup/tin")]
        Task<IApiResponse<MonoStandardResponse<TinResponseModel>>> GetTin([Body] TinRequestModel tinLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/nin
        [Post("/lookup/nin")]
        Task<IApiResponse<MonoStandardResponse<NinResponseModel>>> GetNin([Body] NinRequestModel ninLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/nin with output=pdf — async, returns a job id to poll
        [Post("/lookup/nin")]
        Task<IApiResponse<MonoStandardResponse<NinPdfJobInitiationResponse>>> GetNinPdf([Body] NinPdfRequestModel ninPdfRequestModel, CancellationToken cancellationToken = default);

        // /lookup/nin/{jobId}/job — poll the NIN PDF job
        [Get("/lookup/nin/{jobId}/job")]
        Task<IApiResponse<MonoStandardResponse<NinPollJobResponse>>> PollNinJob(string jobId, CancellationToken cancellationToken = default);

        // /lookup/drivers-license (renamed from driver_license per current Mono lookup docs)
        [Post("/lookup/drivers-license")]
        Task<IApiResponse<MonoStandardResponse<DriversLicenseResponse>>> GetDriverLicense([Body] DriversLicenseRequestModel driverLicenseLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/account-number
        [Post("/lookup/account-number")]
        Task<IApiResponse<MonoStandardResponse<AccountResponse>>> GetAccountNumber([Body] AccountRequestModel accountNumberLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/credit-history/{provider} — provider in path: crc | xds | all
        [Post("/lookup/credit-history/{provider}")]
        Task<IApiResponse<MonoStandardResponse<CreditHistoryResponse>>> GetCreditHistory(string provider, [Body] CreditHistoryRequestModel creditHistoryLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/mashup
        [Post("/lookup/mashup")]
        Task<IApiResponse<MonoStandardResponse<MashUpResponse>>> GetMashUp([Body] MashUpRequestModel mashUpLookUpRequestModel, CancellationToken cancellationToken = default);

        #endregion

    }
}

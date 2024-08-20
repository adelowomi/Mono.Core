using System;
using System.Collections.Generic;
using System.IO;
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

        // /lookup/bvn/verify
        [Post("/lookup/bvn/verify")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> VerifyBvnLookUp([Body] VerifyBvnLookUpOtpModel bvnVerifyRequestModel, [Header("x-session-id")] string sessionId, CancellationToken cancellationToken = default);

        // /lookup/bvn/details
        [Post("/lookup/bvn/details")]
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
        [Get("/lookup/cac/company/{businessId}/previous-address")]
        Task<IApiResponse<MonoStandardResponse<PreviousAddressResponse>>> GetPreviousAddress(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/company/322175/change-of-name
        [Get("/lookup/cac/company/{businessId}/change-of-name")]
        Task<IApiResponse<MonoStandardResponse<ChangeOfNameResponse>>> GetChangeOfName(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/company/322175/secretary
        [Get("/lookup/cac/company/{businessId}/secretary")]
        Task<IApiResponse<MonoStandardResponse<List<OfficialDetails>>>> GetSecretary(string businessId, CancellationToken cancellationToken = default);

        // /lookup/cac/company/322175/directors
        [Get("/lookup/cac/company/{businessId}/directors")]
        Task<IApiResponse<MonoStandardResponse<List<OfficialDetails>>>> GetDirectors(string businessId, CancellationToken cancellationToken = default);

        #endregion

        #region Others LookUp
        // lookup/banks
        [Get("/lookup/banks")]
        Task<IApiResponse<MonoStandardResponse<BanksResponse>>> GetBanks(CancellationToken cancellationToken = default);

        // /lookup/address
        [Get("/lookup/address")]
        Task<IApiResponse<MonoStandardResponse<AddressLookUpResponseModel>>> GetAddress([Body] AddressLookUpRequestModel addressLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/passport
        [Get("/lookup/passport")]
        Task<IApiResponse<MonoStandardResponse<InternationalPassportResponse>>> GetPassport([Body] InternationalPassportRequestModel passportLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/tin
        [Get("/lookup/tin")]
        Task<IApiResponse<MonoStandardResponse<TinResponseModel>>> GetTin([Body] TinRequestModel tinLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/nin
        [Get("/lookup/nin")]
        Task<IApiResponse<MonoStandardResponse<NinResponseModel>>> GetNin([Body] NinRequestModel ninLookUpRequestModel, CancellationToken cancellationToken = default);

        // lookup/driver_license
        [Get("/lookup/driver_license")]
        Task<IApiResponse<MonoStandardResponse<DriversLicenseResponse>>> GetDriverLicense([Body] DriversLicenseRequestModel driverLicenseLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/account-number
        [Get("/lookup/account-number")]
        Task<IApiResponse<MonoStandardResponse<AccountResponse>>> GetAccountNumber([Body] AccountRequestModel accountNumberLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/credit-history/{provider}
        [Get("/lookup/credit-history/{provider}")]
        Task<IApiResponse<MonoStandardResponse<CreditHistoryResponse>>> GetCreditHistory(string provider, [Body] CreditHistoryRequestModel creditHistoryLookUpRequestModel, CancellationToken cancellationToken = default);

        // /lookup/mashup
        [Get("/lookup/mashup")]
        Task<IApiResponse<MonoStandardResponse<MashUpResponse>>> GetMashUp([Body] MashUpRequestModel mashUpLookUpRequestModel, CancellationToken cancellationToken = default);

        #endregion

    }
}

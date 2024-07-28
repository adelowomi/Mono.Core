using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.LookUp
{
    public interface IMonoLookUpService
    {
        Task<MonoStandardResponse<InitiateBvnLookUpResponseModel>> InitiateBvnLookUp(InitiateBvnLookUpModel bvnInitiateRequestModel, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<bool>> VerifyBvnLookUp(VerifyBvnLookUpOtpModel verifyBvnLookUpOtpModel,string sessionId, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<BvnDetailsResponse>> GetBvnDetails(BvnDetailsModel bvnDetailsModel, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<List<BusinessDetails>>> GetCacLookUp(string search, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<List<OfficialDetails>>> GetCacCompany(string businessId, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<PreviousAddressResponse>> GetPreviousAddress(string businessId, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<ChangeOfNameResponse>> GetChangeOfName(string businessId, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<List<OfficialDetails>>> GetSecretary(string businessId, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<List<OfficialDetails>>> GetDirectors(string businessId, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<BanksResponse>> GetBanks(CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<AddressLookUpResponseModel>> GetAddress(AddressLookUpRequestModel addressLookUpRequestModel, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<InternationalPassportResponse>> GetPassport(InternationalPassportRequestModel passportLookUpRequestModel, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<NinResponseModel>> GetNin(NinRequestModel ninRequestModel, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<DriversLicenseResponse>> GetDriverLicense(DriversLicenseRequestModel driverLicenseLookUpRequestModel, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<AccountResponse>> GetAccountNumber(AccountRequestModel accountNumberLookUpRequestModel, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<CreditHistoryResponse>> GetCreditHistory(string provider, CreditHistoryRequestModel creditHistoryLookUpRequestModel, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<MashUpResponse>> GetMashUp(MashUpRequestModel mashUpLookUpRequestModel, CancellationToken cancellationToken = default);
    }
}

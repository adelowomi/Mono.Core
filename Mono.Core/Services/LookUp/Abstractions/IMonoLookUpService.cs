using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.LookUp
{
    public interface IMonoLookUp
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bvnInitiateRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<InitiateBvnLookUpResponseModel>> InitiateBvnLookUp(InitiateBvnLookUpModel bvnInitiateRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verifyBvnLookUpOtpModel"></param>
        /// <param name="sessionId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<bool>> VerifyBvnLookUp(VerifyBvnLookUpOtpModel verifyBvnLookUpOtpModel, string sessionId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bvnDetailsModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<BvnDetailsResponse>> GetBvnDetails(BvnDetailsModel bvnDetailsModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<List<BusinessDetails>>> GetCacLookUp(string search, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<List<OfficialDetails>>> GetCacCompany(string businessId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<PreviousAddressResponse>> GetPreviousAddress(string businessId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<ChangeOfNameResponse>> GetChangeOfName(string businessId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<List<OfficialDetails>>> GetSecretary(string businessId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessId"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<List<OfficialDetails>>> GetDirectors(string businessId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<BanksResponse>> GetBanks(CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="addressLookUpRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<AddressLookUpResponseModel>> GetAddress(AddressLookUpRequestModel addressLookUpRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passportLookUpRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<InternationalPassportResponse>> GetPassport(InternationalPassportRequestModel passportLookUpRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ninRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<NinResponseModel>> GetNin(NinRequestModel ninRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="driverLicenseLookUpRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<DriversLicenseResponse>> GetDriverLicense(DriversLicenseRequestModel driverLicenseLookUpRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountNumberLookUpRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<AccountResponse>> GetAccountNumber(AccountRequestModel accountNumberLookUpRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="creditHistoryLookUpRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<CreditHistoryResponse>> GetCreditHistory(string provider, CreditHistoryRequestModel creditHistoryLookUpRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mashUpLookUpRequestModel"></param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns></returns>
        Task<MonoStandardResponse<MashUpResponse>> GetMashUp(MashUpRequestModel mashUpLookUpRequestModel, CancellationToken cancellationToken = default);
    }
}

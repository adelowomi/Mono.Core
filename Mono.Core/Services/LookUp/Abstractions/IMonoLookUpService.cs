using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.LookUp
{
    public interface IMonoLookUp
    {
          /// <summary>
        /// This method initiate an BVN Lookup base on the details of the BVN provided.
        /// </summary>
        /// <param name="bvnInitiateRequestModel">The model require to intiate a bvn request model.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous task that returns a MonoStandardResponse containing an InitiateBvnLookUpResponseModel. </returns>
        Task<MonoStandardResponse<InitiateBvnLookUpResponseModel>> InitiateBvnLookUp(InitiateBvnLookUpModel bvnInitiateRequestModel, CancellationToken cancellationToken = default);

          /// <summary>
        /// This method verifies the BVN lookup using the BvnOtpModel and the sessionId.
        /// </summary>
        /// <param name="verifyBvnLookUpOtpModel">The model that contain the OTP model for BVNLookup.</param>
        /// <param name="sessionId">The session ID with the BVN LookUp.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous task that returns a MonoStandardResponse and a boolen to verify the success of the verifications.</returns>
        Task<MonoStandardResponse<dynamic>> VerifyBvnLookUp(VerifyBvnLookUpOtpModel verifyBvnLookUpOtpModel, string sessionId, CancellationToken cancellationToken = default);

         /// <summary>
        /// This method retrive and verify the Bvn Informations.
        /// </summary>
        /// <param name="bvnDetailsModel">The model that contains the OTP for Bvn Verifications</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous task that return a MonoStandardResponse and a BvnDetailsResponse</returns>
        Task<MonoStandardResponse<BvnDetailsResponse>> GetBvnDetails(BvnDetailsModel bvnDetailsModel, string sessionId, CancellationToken cancellationToken = default);

        /// <summary>
        ///  This method get a cac lookup base on the search query provided.
        /// </summary>
        /// <param name="search">The search query to get the cac lookup.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous task that returns a MonoStandardResponse and a list of business details.</returns>
        Task<MonoStandardResponse<List<BusinessDetails>>> GetCacLookUp(string search, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method get a details of a specific company using the business Id.
        /// </summary>
        /// <param name="businessId">The ID to retrive the details about a specific bussiness.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous operation that return a MonoStandardResponse and a list of Official details.</returns>
        Task<MonoStandardResponse<List<OfficialDetails>>> GetCacCompany(string businessId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method retrive the address for a specific business using the provided businessId.
        /// </summary>
        /// <param name="businessId">The ID to retrive the address for a specific business</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous operations that returns a MonoStandardResponse containing PreviousAddressResponse.</returns>
        Task<MonoStandardResponse<PreviousAddressResponse>> GetPreviousAddress(string businessId, CancellationToken cancellationToken = default);

         /// <summary>
        /// This method get the change of name for a specific business using the provided businessId.
        /// </summary>
        /// <param name="businessId">The Id to change the name of a specific business.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous operations that returns a MonoStandardResponse containing ChangeOfNameResponse.</returns>
        Task<MonoStandardResponse<ChangeOfNameResponse>> GetChangeOfName(string businessId, CancellationToken cancellationToken = default);

         /// <summary>
        /// This method get the secretary for a specific business using the provided businessId.
        /// </summary>
        /// <param name="businessId">The Id used to get secretary of a specific business. </param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous operation that returns a MonoStandardResponse and a list of Official details.</returns>
        Task<MonoStandardResponse<List<OfficialDetails>>> GetSecretary(string businessId, CancellationToken cancellationToken = default);

         /// <summary>
        /// This method retrive the directors for a specific business using the provided businessId.
        /// </summary>
        /// <param name="businessId">The Id used to get the director of a specific businessId.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>A task of an asychronous operation, returning a response of a MonoStandardResponse containing a list of OfficialDetails.</returns>
        Task<MonoStandardResponse<List<OfficialDetails>>> GetDirectors(string businessId, CancellationToken cancellationToken = default);

         /// <summary>
        /// This method retrive the list of banks 
        /// </summary>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a BanksResponse object.</returns>
        Task<MonoStandardResponse<BanksResponse>> GetBanks(CancellationToken cancellationToken = default);

         /// <summary>
        /// This method retrieves address details based on the provided address lookup request model.
        /// </summary>
        /// <param name="addressLookUpRequestModel">The model containing the details for the address lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with an AddressLookUpResponseModel.</returns>
        Task<MonoStandardResponse<AddressLookUpResponseModel>> GetAddress(AddressLookUpRequestModel addressLookUpRequestModel, CancellationToken cancellationToken = default);

         /// <summary>
        /// The method retrieves international passport details based on the provided passport lookup request model.
        /// </summary>
        /// <param name="passportLookUpRequestModel">The model containing the details for the international passport lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with an InternationalPassportResponse.</returns>
        Task<MonoStandardResponse<InternationalPassportResponse>> GetPassport(InternationalPassportRequestModel passportLookUpRequestModel, CancellationToken cancellationToken = default);

         /// <summary>
        /// This method retrieves National Identification Number (NIN) details based on the provided NIN lookup request model.
        /// </summary>
        /// <param name="ninLookUpRequestModel">The model containing the details for the NIN lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a NinResponseModel.</returns>
        Task<MonoStandardResponse<NinResponseModel>> GetNin(NinRequestModel ninRequestModel, CancellationToken cancellationToken = default);

            /// <summary>
        /// This method retrieves driver's license details based on the provided driver's license lookup request model.
        /// </summary>
        /// <param name="driverLicenseLookUpRequestModel">The model containing the details for the driver's license lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a DriversLicenseResponse.</returns>
        Task<MonoStandardResponse<DriversLicenseResponse>> GetDriverLicense(DriversLicenseRequestModel driverLicenseLookUpRequestModel, CancellationToken cancellationToken = default);

            /// <summary>
        /// This method retrieves account number details based on the provided account number lookup request model.
        /// </summary>
        /// <param name="accountNumberLookUpRequestModel">The model containing the details for the account number lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with an AccountResponse.</returns>
        Task<MonoStandardResponse<AccountResponse>> GetAccountNumber(AccountRequestModel accountNumberLookUpRequestModel, CancellationToken cancellationToken = default);

          /// <summary>
        /// This method retrieves credit history details based on the provided provider and credit history lookup request model.
        /// </summary>
        /// <param name="provider">The provider through which to retrieve the credit history.</param>
        /// <param name="creditHistoryLookUpRequestModel">The model containing the details for the credit history lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a CreditHistoryResponse.</returns>
        Task<MonoStandardResponse<CreditHistoryResponse>> GetCreditHistory(string provider, CreditHistoryRequestModel creditHistoryLookUpRequestModel, CancellationToken cancellationToken = default);

            /// <summary>
        /// This method retrieves mash-up details based on the provided mash-up lookup request model.
        /// </summary>
        /// <param name="mashUpLookUpRequestModel">The model containing the details for the mash-up lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a MashUpResponse.</returns>
        Task<MonoStandardResponse<MashUpResponse>> GetMashUp(MashUpRequestModel mashUpLookUpRequestModel, CancellationToken cancellationToken = default);
    }
}

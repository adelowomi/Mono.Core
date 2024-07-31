using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.LookUp
{
    public class LookUpService : IMonoLookUp
    {
        private readonly ILookUpService _lookUpService;
        private readonly ILookUpService _lookUpServiceV3;

        public LookUpService(IRefitClientBuilder<ILookUpService> lookUpService)
        {
            _lookUpService = lookUpService.Build();
            _lookUpServiceV3 = lookUpService.BuildV3();
        }

          /// <summary>
        /// Initiate an BVN Lookup base on the details of the BVN provided.
        /// </summary>
        /// <param name="bvnInitiateRequestModel">The model require to intiate a bvn request model.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous task that returns a MonoStandardResponse containing an InitiateBvnLookUpResponseModel. </returns>
        public async Task<MonoStandardResponse<InitiateBvnLookUpResponseModel>> InitiateBvnLookUp(InitiateBvnLookUpModel bvnInitiateRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpService.InitiateBvnLookUp(bvnInitiateRequestModel, cancellationToken);
            return response.HandleResponse();
        }

          /// <summary>
        /// verifies the BVN lookup using the BvnOtpModel and the sessionId.
        /// </summary>
        /// <param name="verifyBvnLookUpOtpModel">The model that contain the OTP model for BVNLookup.</param>
        /// <param name="sessionId">The session ID with the BVN LookUp.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous task that returns a MonoStandardResponse and a boolen to verify the success of the verifications.</returns>
        public async Task<MonoStandardResponse<bool>> VerifyBvnLookUp(VerifyBvnLookUpOtpModel verifyBvnLookUpOtpModel, string sessionId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpService.VerifyBvnLookUp(verifyBvnLookUpOtpModel, sessionId, cancellationToken);
            return response.HandleResponse();
        }
        
         /// <summary>
        /// Retrive and verify the Bvn Informations.
        /// </summary>
        /// <param name="bvnDetailsModel">The model that contains the OTP for Bvn Verifications</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous task that return a MonoStandardResponse and a BvnDetailsResponse</returns>
        public async Task<MonoStandardResponse<BvnDetailsResponse>> GetBvnDetails(BvnDetailsModel bvnDetailsModel, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpService.GetBvnDetails(bvnDetailsModel, cancellationToken);
            return response.HandleResponse();
        }

        /// <summary>
        ///  Get a cac lookup base on the search query provided.
        /// </summary>
        /// <param name="search">The search query to get the cac lookup.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous task that returns a MonoStandardResponse and a list of business details.</returns>
        public async Task<MonoStandardResponse<List<BusinessDetails>>> GetCacLookUp(string search, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetCacLookUp(search, cancellationToken);
            return response.HandleResponse();
        }

        /// <summary>
        /// Get a details of a specific company using the business Id.
        /// </summary>
        /// <param name="businessId">The ID to retrive the details about a specific bussiness.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous operation that return a MonoStandardResponse and a list of Official details.</returns>
        public async Task<MonoStandardResponse<List<OfficialDetails>>> GetCacCompany(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetCacCompany(businessId, cancellationToken);
            return response.HandleResponse();
        }

        /// <summary>
        /// Retrive the address for a specific business using the provided businessId.
        /// </summary>
        /// <param name="businessId">The ID to retrive the address for a specific business</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous operations that returns a MonoStandardResponse containing PreviousAddressResponse.</returns>
        public async Task<MonoStandardResponse<PreviousAddressResponse>> GetPreviousAddress(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetPreviousAddress(businessId, cancellationToken);
            return response.HandleResponse();
        }

         /// <summary>
        /// Get the change of name for a specific business using the provided businessId.
        /// </summary>
        /// <param name="businessId">The Id to change the name of a specific business.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous operations that returns a MonoStandardResponse containing ChangeOfNameResponse.</returns>
        public async Task<MonoStandardResponse<ChangeOfNameResponse>> GetChangeOfName(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetChangeOfName(businessId, cancellationToken);
            return response.HandleResponse();
        }

         /// <summary>
        /// Get the secretary for a specific business using the provided businessId.
        /// </summary>
        /// <param name="businessId">The Id used to get secretary of a specific business. </param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>An asynchronous operation that returns a MonoStandardResponse and a list of Official details.</returns>
        public async Task<MonoStandardResponse<List<OfficialDetails>>> GetSecretary(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetSecretary(businessId, cancellationToken);
            return response.HandleResponse();
        }

         /// <summary>
        /// Retrive the directors for a specific business using the provided businessId.
        /// </summary>
        /// <param name="businessId">The Id used to get the director of a specific businessId.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>A task of an asychronous operation, returning a response of a MonoStandardResponse containing a list of OfficialDetails.</returns>
        public async Task<MonoStandardResponse<List<OfficialDetails>>> GetDirectors(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetDirectors(businessId, cancellationToken);
            return response.HandleResponse();
        }

        /// <summary>
        /// Retrive the list of banks 
        /// </summary>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a BanksResponse object.</returns>
        public async Task<MonoStandardResponse<BanksResponse>> GetBanks(CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetBanks(cancellationToken);
            return response.HandleResponse();
        }

         /// <summary>
        /// Retrieves address details based on the provided address lookup request model.
        /// </summary>
        /// <param name="addressLookUpRequestModel">The model containing the details for the address lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. i.e This will terminate the http request</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with an AddressLookUpResponseModel.</returns>
        public async Task<MonoStandardResponse<AddressLookUpResponseModel>> GetAddress(AddressLookUpRequestModel addressLookUpRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetAddress(addressLookUpRequestModel, cancellationToken);
            return response.HandleResponse();
        }

         /// <summary>
        /// Retrieves international passport details based on the provided passport lookup request model.
        /// </summary>
        /// <param name="passportLookUpRequestModel">The model containing the details for the international passport lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with an InternationalPassportResponse.</returns>
        public async Task<MonoStandardResponse<InternationalPassportResponse>> GetPassport(InternationalPassportRequestModel passportLookUpRequestModel, CancellationToken cancellationToken = default)
        {
          var response = await _lookUpServiceV3.GetPassport(passportLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }


        public async Task<MonoStandardResponse<TinResponseModel>> GetTin(TinRequestModel tinLookUpRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetTin(tinLookUpRequestModel, cancellationToken);
            return response.HandleResponse();
        }

         /// <summary>
        /// Retrieves National Identification Number (NIN) details based on the provided NIN lookup request model.
        /// </summary>
        /// <param name="ninLookUpRequestModel">The model containing the details for the NIN lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a NinResponseModel.</returns>
        public async Task<MonoStandardResponse<NinResponseModel>> GetNin(NinRequestModel ninLookUpRequestModel, CancellationToken cancellationToken = default)
        {
           var response = await _lookUpServiceV3.GetNin(ninLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }


            /// <summary>
        /// Retrieves driver's license details based on the provided driver's license lookup request model.
        /// </summary>
        /// <param name="driverLicenseLookUpRequestModel">The model containing the details for the driver's license lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a DriversLicenseResponse.</returns>
        public async Task<MonoStandardResponse<DriversLicenseResponse>> GetDriverLicense(DriversLicenseRequestModel driverLicenseLookUpRequestModel, CancellationToken cancellationToken = default)
        {
          var response = await _lookUpServiceV3.GetDriverLicense(driverLicenseLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }


            /// <summary>
        /// Retrieves account number details based on the provided account number lookup request model.
        /// </summary>
        /// <param name="accountNumberLookUpRequestModel">The model containing the details for the account number lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with an AccountResponse.</returns>
        public async Task<MonoStandardResponse<AccountResponse>> GetAccountNumber(AccountRequestModel accountNumberLookUpRequestModel, CancellationToken cancellationToken = default)
        {
         var response = await _lookUpServiceV3.GetAccountNumber(accountNumberLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }


          /// <summary>
        /// Retrieves credit history details based on the provided provider and credit history lookup request model.
        /// </summary>
        /// <param name="provider">The provider through which to retrieve the credit history.</param>
        /// <param name="creditHistoryLookUpRequestModel">The model containing the details for the credit history lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a CreditHistoryResponse.</returns>
        public async Task<MonoStandardResponse<CreditHistoryResponse>> GetCreditHistory(string provider, CreditHistoryRequestModel creditHistoryLookUpRequestModel, CancellationToken cancellationToken = default)
        {
          var response = await _lookUpServiceV3.GetCreditHistory(provider, creditHistoryLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }


            /// <summary>
        /// Retrieves mash-up details based on the provided mash-up lookup request model.
        /// </summary>
        /// <param name="mashUpLookUpRequestModel">The model containing the details for the mash-up lookup request.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with a MashUpResponse.</returns>
        public async Task<MonoStandardResponse<MashUpResponse>> GetMashUp(MashUpRequestModel mashUpLookUpRequestModel, CancellationToken cancellationToken = default)
        {
          var response = await _lookUpServiceV3.GetMashUp(mashUpLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }

    }
}

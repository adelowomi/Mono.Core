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
            _lookUpService = lookUpService.Build(ServiceTypes.Lookup);
            _lookUpServiceV3 = lookUpService.BuildV3(ServiceTypes.Lookup);
        }

        
        public async Task<MonoStandardResponse<InitiateBvnLookUpResponseModel>> InitiateBvnLookUp(InitiateBvnLookUpModel bvnInitiateRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpService.InitiateBvnLookUp(bvnInitiateRequestModel, cancellationToken);
            return response.HandleResponse();
        }

        
        public async Task<MonoStandardResponse<dynamic>> VerifyBvnLookUp(VerifyBvnLookUpOtpModel verifyBvnLookUpOtpModel, string sessionId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpService.VerifyBvnLookUp(verifyBvnLookUpOtpModel, sessionId, cancellationToken);
            return response.HandleResponse();
        }
        
        
        public async Task<MonoStandardResponse<BvnDetailsResponse>> GetBvnDetails(BvnDetailsModel bvnDetailsModel, string sessionId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpService.GetBvnDetails(bvnDetailsModel, sessionId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<List<BusinessDetails>>> GetCacLookUp(string search, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetCacLookUp(search, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<List<OfficialDetails>>> GetCacCompany(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetCacCompany(businessId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<PreviousAddressResponse>> GetPreviousAddress(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetPreviousAddress(businessId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ChangeOfNameResponse>> GetChangeOfName(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetChangeOfName(businessId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<List<OfficialDetails>>> GetSecretary(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetSecretary(businessId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<List<OfficialDetails>>> GetDirectors(string businessId, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetDirectors(businessId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<BanksResponse>> GetBanks(CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetBanks(cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<AddressLookUpResponseModel>> GetAddress(AddressLookUpRequestModel addressLookUpRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _lookUpServiceV3.GetAddress(addressLookUpRequestModel, cancellationToken);
            return response.HandleResponse();
        }

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

        public async Task<MonoStandardResponse<NinResponseModel>> GetNin(NinRequestModel ninLookUpRequestModel, CancellationToken cancellationToken = default)
        {
           var response = await _lookUpServiceV3.GetNin(ninLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<DriversLicenseResponse>> GetDriverLicense(DriversLicenseRequestModel driverLicenseLookUpRequestModel, CancellationToken cancellationToken = default)
        {
          var response = await _lookUpServiceV3.GetDriverLicense(driverLicenseLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<AccountResponse>> GetAccountNumber(AccountRequestModel accountNumberLookUpRequestModel, CancellationToken cancellationToken = default)
        {
         var response = await _lookUpServiceV3.GetAccountNumber(accountNumberLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<CreditHistoryResponse>> GetCreditHistory(string provider, CreditHistoryRequestModel creditHistoryLookUpRequestModel, CancellationToken cancellationToken = default)
        {
          var response = await _lookUpServiceV3.GetCreditHistory(provider, creditHistoryLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<MashUpResponse>> GetMashUp(MashUpRequestModel mashUpLookUpRequestModel, CancellationToken cancellationToken = default)
        {
          var response = await _lookUpServiceV3.GetMashUp(mashUpLookUpRequestModel, cancellationToken);
             return response.HandleResponse();
        }

    }
}

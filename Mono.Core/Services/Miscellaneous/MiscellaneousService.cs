using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Miscellaneous
{
    public class MiscellaneousService : IMonoMiscellaneous
    {
        private readonly IMiscellaneousService _miscellaneousService;

        public MiscellaneousService(IRefitClientBuilder<IMiscellaneousService> miscellaneousService)
        {
            _miscellaneousService = miscellaneousService.Build();
        }

        
        public async Task<MonoStandardResponse<InstitutionResponseModel>> GetCoverage(CancellationToken cancellationToken = default)
        {
          var response = await _miscellaneousService.GetCoverage(cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<BusinessLookUpResponseModel>> GetCacLookup(CancellationToken cancellationToken = default)
        {
           var response = await _miscellaneousService.GetCacLookup(cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<ShareHolderResponseModel>> GetCacCompany(string businessId, CancellationToken cancellationToken = default)
        {
          var response = await _miscellaneousService.GetCacCompany(businessId, cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<string>> UnlinkAccount(string accountId, CancellationToken cancellationToken = default)
        {
          var response = await _miscellaneousService.UnlinkAccount(accountId, cancellationToken);
             return response.HandleResponse();
        }

    }
}

using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Accounts
{
    public class AccountService : IMonoAccounts
    {
        private readonly IAccountService _accountService;

        public AccountService(IRefitClientBuilder<IAccountService> accountService)
        {
            _accountService = accountService.Build(ServiceTypes.Connect);
        }

        public async Task<MonoStandardResponse<AccountLinkingResponseModel>> InitiateAccountLinking(AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default)
        {
          var response = await _accountService.InitiateAccountLinking(accountLinkingModel, cancellationToken);
             return response.HandleResponse();
        }

       
        public async Task<MonoStandardResponse<InformationResponseModel>> GetAccount(string accountId, CancellationToken cancellationToken = default)
        {
          var response = await _accountService.GetAccount(accountId, cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<StatementPdfResponseModel>> GetPollStatementPdf(string accountId, string jobId, CancellationToken cancellationToken = default)
        {
          var response = await _accountService.GetPollStatementPdf(accountId, jobId, cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<IncomeResponseModel>> GetIncome(string accountId, CancellationToken cancellationToken = default)
        {
         var response = await _accountService.GetIncome(accountId, cancellationToken);
             return response.HandleResponse();
        }


        public async Task<MonoStandardResponse<IdentityResponseModel>> GetIdentity(string accountId, CancellationToken cancellationToken = default)
        {
          var response = await _accountService.GetIdentity(accountId, cancellationToken);
             return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<StatementResponseModel>> GetStatement(string accountId, StatementRequestModels statementRequestModels, CancellationToken cancellationToken = default)
        {
             var response = await _accountService.GetStatement(accountId, statementRequestModels, cancellationToken);
                 return response.HandleResponse();
        }


        public async Task<MonoStandardResponse<TransactionResponseModel>> GetTransactions(string accountId, AccountTransactionsOptionsRequest accountTransactionsOptionsRequest, CancellationToken cancellationToken = default)
        {
            // Refit deserializes into MonoStandardResponse<List<Transaction>>
            // because that matches Mono's actual flat response shape. We
            // adapt to TransactionResponseModel here so the public surface
            // stays backward-compatible.
            var response = await _accountService.GetTransactions(accountId, accountTransactionsOptionsRequest, cancellationToken);
            var envelope = response.HandleResponse();
            return new MonoStandardResponse<TransactionResponseModel>
            {
                Success = envelope.Success,
                Status = envelope.Status,
                Message = envelope.Message,
                Timestamp = envelope.Timestamp,
                MonoErrors = envelope.MonoErrors,
                InAppErrors = envelope.InAppErrors,
                Data = envelope.Data == null ? null : new TransactionResponseModel
                {
                    Transactions = envelope.Data,
                    // Mono returns pagination info under "meta" on the top-level
                    // envelope (both paginate=true and paginate=false). We
                    // surface it on TransactionResponseModel.Paging so
                    // consumers can read cursor + total via the same shape
                    // they already expect.
                    Paging = envelope.Meta
                }
            };
        }

        public async Task<MonoStandardResponse<AccountBalanceResponse>> GetAccountBalance(string accountId, CancellationToken cancellationToken = default)
        {
            var response = await _accountService.GetAccountBalance(accountId, cancellationToken);
            return response.HandleResponse();
        }

    }
}

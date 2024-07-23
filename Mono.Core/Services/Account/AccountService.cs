using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Accounts
{
    public class AccountService : IMonoAccounts
    {
        private readonly IAccountService _accountService;

        public AccountService(IRefitClientBuilder<IAccountService> accountService)
        {
            _accountService = accountService.Build();
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
            var response = await _accountService.GetTransactions(accountId, accountTransactionsOptionsRequest, cancellationToken);
            return response.HandleResponse();
        }
    }
}

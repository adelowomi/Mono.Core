using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Mono.Core.Accounts
{
    // this is the refit interface
    public interface IAccountService
    {
        // /accounts/initiate
        [Post("/accounts/initiate")]
        Task<IApiResponse<MonoStandardResponse<AccountLinkingResponseModel>>> InitiateAccountLinking([Body] AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default);
        [Get("/accounts/{accountId}")]
        Task<IApiResponse<MonoStandardResponse<InformationResponseModel>>> GetAccount(string accountId, CancellationToken cancellationToken = default);
        // "accounts/{accountId}/statement/jobs/{jobId}
        [Get("/accounts/{accountId}/statement/jobs/{jobId}")]
        Task<IApiResponse<MonoStandardResponse<StatementPdfResponseModel>>> GetPollStatementPdf(string accountId, string jobId, CancellationToken cancellationToken = default);
        // "accounts/{accountId}/income"
        [Get("/accounts/{accountId}/income")]
        Task<IApiResponse<MonoStandardResponse<IncomeResponseModel>>> GetIncome(string accountId, CancellationToken cancellationToken = default);
        //"accounts/{accountId}/identity"
        [Get("/accounts/{accountId}/identity")]
        Task<IApiResponse<MonoStandardResponse<IdentityResponseModel>>> GetIdentity(string accountId, CancellationToken cancellationToken = default);
        //accounts/{accountId}/statement?period={period}&output={output}
        [Get("/accounts/{accountId}/statement")]
        Task<IApiResponse<MonoStandardResponse<StatementResponseModel>>> GetStatement(string accountId, [Query] StatementRequestModels statementRequestModels, CancellationToken cancellationToken = default);
        // accounts/{accountId}/transactions?start={start}&end={end}&narration={narration}&type={type}&paginate={paginate}&limit={limit}
        [Get("/accounts/{accountId}/transactions")]
        Task<IApiResponse<MonoStandardResponse<TransactionResponseModel>>> GetTransactions(string accountId, [Query] AccountTransactionsOptionsRequest accountTransactionsOptionsRequest, CancellationToken cancellationToken = default);
    }
}

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
        //
        // Mono returns this endpoint as a flat envelope:
        //   { "status", "message", "timestamp", "data": [ {...transactions...} ] }
        // — the `data` field is the transaction array directly, NOT wrapped in
        // a nested object with its own `paging` + `data` fields. Refit needs
        // the generic to match that shape (`List<Transaction>`) or
        // System.Text.Json hits a type mismatch on `data: array → object` and
        // hands us null Content. The public wrapper in AccountService
        // converts back into TransactionResponseModel so existing consumers
        // (e.g. `response.Data.Transactions`) keep working.
        [Get("/accounts/{accountId}/transactions")]
        Task<IApiResponse<MonoStandardResponse<System.Collections.Generic.List<Transaction>>>> GetTransactions(string accountId, [Query] AccountTransactionsOptionsRequest accountTransactionsOptionsRequest, CancellationToken cancellationToken = default);

        // /accounts/{accountId}/balance — real-time balance (Feb 2025)
        [Get("/accounts/{accountId}/balance")]
        Task<IApiResponse<MonoStandardResponse<AccountBalanceResponse>>> GetAccountBalance(string accountId, CancellationToken cancellationToken = default);
    }
}

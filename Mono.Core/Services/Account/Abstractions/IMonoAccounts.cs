using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.Accounts
{
    public interface IMonoAccounts
    {
         /// <summary>
        /// This method initiates the process of linking an account based on the provided account linking model.
        /// </summary>
        /// <param name="accountLinkingModel">The model containing the details required to initiate account linking.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation, containing a MonoStandardResponse with an AccountLinkingResponseModel.</returns>
        Task<MonoStandardResponse<AccountLinkingResponseModel>> InitiateAccountLinking(AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method gets the account information for a specified account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve information for.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with an InformationResponseModel.</returns>
        Task<MonoStandardResponse<InformationResponseModel>> GetAccount(string accountId, CancellationToken cancellationToken = default);

       
         /// <summary>
        /// This method retrieves the statement PDF for a specified account and job ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve the statement for.</param>
        /// <param name="jobId">The ID of the job associated with the statement.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a StatementPdfResponseModel.</returns>
        Task<MonoStandardResponse<StatementPdfResponseModel>> GetPollStatementPdf(string accountId, string jobId, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method retrieves the income information for a specified account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve income information for.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with an IncomeResponseModel.</returns>        Task<MonoStandardResponse<IncomeResponseModel>> GetIncome(string accountId, CancellationToken cancellationToken = default);
        Task<MonoStandardResponse<IdentityResponseModel>> GetIdentity(string accountId, CancellationToken cancellationToken = default);

           /// <summary>
        /// This method retrieves the statement for a specified account ID based on the provided request models.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve the statement for.</param>
        /// <param name="statementRequestModels">The request models containing the parameters for the statement retrieval.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a StatementResponseModel.</returns>
        Task<MonoStandardResponse<StatementResponseModel>> GetStatement(string accountId, StatementRequestModels statementRequestModels, CancellationToken cancellationToken = default);

           /// <summary>
        /// This method retrieves the transaction information for a specified account ID based on the provided request options.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve transactions for.</param>
        /// <param name="accountTransactionsOptionsRequest">The request options containing the parameters for the transaction retrieval.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a TransactionResponseModel.</returns>
        Task<MonoStandardResponse<TransactionResponseModel>> GetTransactions(string accountId, AccountTransactionsOptionsRequest accountTransactionsOptionsRequest, CancellationToken cancellationToken = default);
    }
}

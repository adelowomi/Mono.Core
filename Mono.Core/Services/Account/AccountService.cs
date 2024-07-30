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

        // public async Task<MonoStandardResponse<AccountLinkingResponseModel>> InitiateAccountLinking(AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default)
        // {
        //     var response = await _accountService.InitiateAccountLinking(accountLinkingModel, cancellationToken);
        //     return response.HandleResponse();
        // }

        /// <summary>
        /// Initiates the process of linking an account using the provided account linking model.
        /// </summary>
        /// <param name="accountLinkingModel">An instance of <see cref="AccountLinkingModel"/> that contains the details required for linking the account.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the task. If cancellation is requested, this will terminate the HTTP request.</param>
        /// <returns>A task representing the asynchronous operation. The task result is a <see cref="MonoStandardResponse{AccountLinkingResponseModel}"/> that includes the response details of the account linking process.</returns>
        public async Task<MonoStandardResponse<AccountLinkingResponseModel>> InitiateAccountLinking(AccountLinkingModel accountLinkingModel, CancellationToken cancellationToken = default)
        {
          // Call the _accountService's InitiateAccountLinking method to start the linking process
         var response = await _accountService.InitiateAccountLinking(accountLinkingModel, cancellationToken);
    
          // Handle the response from the account service and return it
          return response.HandleResponse();
        }

        /// <summary>
        /// Gets the account information for a specified account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve information for.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with an InformationResponseModel.</returns>
        public async Task<MonoStandardResponse<InformationResponseModel>> GetAccount(string accountId, CancellationToken cancellationToken = default)
        {
          var response = await _accountService.GetAccount(accountId, cancellationToken);
             return response.HandleResponse();
        }

         /// <summary>
        /// Retrieves the statement PDF for a specified account and job ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve the statement for.</param>
        /// <param name="jobId">The ID of the job associated with the statement.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a StatementPdfResponseModel.</returns>
        public async Task<MonoStandardResponse<StatementPdfResponseModel>> GetPollStatementPdf(string accountId, string jobId, CancellationToken cancellationToken = default)
        {
          var response = await _accountService.GetPollStatementPdf(accountId, jobId, cancellationToken);
             return response.HandleResponse();
        }


         /// <summary>
        /// Retrieves the income information for a specified account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve income information for.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with an IncomeResponseModel.</returns>
        public async Task<MonoStandardResponse<IncomeResponseModel>> GetIncome(string accountId, CancellationToken cancellationToken = default)
        {
         var response = await _accountService.GetIncome(accountId, cancellationToken);
             return response.HandleResponse();
        }


          /// <summary>
        /// Retrieves the identity information for a specified account ID.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve identity information for.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with an IdentityResponseModel.</returns>
        public async Task<MonoStandardResponse<IdentityResponseModel>> GetIdentity(string accountId, CancellationToken cancellationToken = default)
        {
          var response = await _accountService.GetIdentity(accountId, cancellationToken);
             return response.HandleResponse();
        }


           /// <summary>
        /// Retrieves the statement for a specified account ID based on the provided request models.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve the statement for.</param>
        /// <param name="statementRequestModels">The request models containing the parameters for the statement retrieval.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a StatementResponseModel.</returns>
        public async Task<MonoStandardResponse<StatementResponseModel>> GetStatement(string accountId, StatementRequestModels statementRequestModels, CancellationToken cancellationToken = default)
        {
             var response = await _accountService.GetStatement(accountId, statementRequestModels, cancellationToken);
                 return response.HandleResponse();
        }


           /// <summary>
        /// Retrieves the transaction information for a specified account ID based on the provided request options.
        /// </summary>
        /// <param name="accountId">The ID of the account to retrieve transactions for.</param>
        /// <param name="accountTransactionsOptionsRequest">The request options containing the parameters for the transaction retrieval.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a MonoStandardResponse with a TransactionResponseModel.</returns>
        public async Task<MonoStandardResponse<TransactionResponseModel>> GetTransactions(string accountId, AccountTransactionsOptionsRequest accountTransactionsOptionsRequest, CancellationToken cancellationToken = default)
        {
           var response = await _accountService.GetTransactions(accountId, accountTransactionsOptionsRequest, cancellationToken);
             return response.HandleResponse();
        }

    }
}

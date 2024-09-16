using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mono.Core.Services.DirectPay.Models;

namespace Mono.Core.DirectPay
{
    public interface IMonoDirectPay
    {
           /// <summary>
        /// This method initiates a one-time payment based on the provided payment request model.
        /// </summary>
        /// <param name="initiatePaymentRequestModel">The model containing the details required to initiate the one-time payment.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns result that contains a MonoStandardResponse containing an InitiateOneTimePaymentResponse.</returns>
        Task<MonoStandardResponse<InitiateOneTimePaymentResponse>> InitiatePayment(InitiateOneTimePayment initiatePaymentRequestModel, CancellationToken cancellationToken = default);

            /// <summary>
        /// This method verifies the payment using provided reference
        /// </summary>
        /// <param name="reference">The reference used to verify the payment</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse containing a VerifyPaymentReferenceResponse.</returns>
        Task<MonoStandardResponse<VerifyPaymentReferenceResponse>> VerifyPayment(string reference, CancellationToken cancellationToken = default);

         /// <summary>
        /// This methos get a list of payment transactions based on the provided query options.
        /// </summary>
        /// <param name="options">The query options to filter and retrive payment transactions.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a list of PaymentResponseModel.</returns>
        Task<MonoStandardResponse<List<PaymentResponseModel>>> GetTransactions(PaymentRequestQueryOptions options, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method retrieves all debits based on the provided mandate id.
        /// </summary>
        /// <param name="id">The mandate id used to retrieve all debits.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a list of RetrieveDebitResponse.</returns>
        /// <returns></returns>
        Task<MonoStandardResponse<List<RetrieveDebitResponse>>> RetrieveAllDebits(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method retrieves a debit based on the provided mandate id and reference.
        /// </summary>
        /// <param name="id">The mandate id used to retrieve the debit.</param>
        /// <param name="reference">The reference used to retrieve the debit.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a RetrieveDebitResponse.</returns>
        Task<MonoStandardResponse<RetrieveDebitResponse>> RetrieveDebitByReference(string id, string reference, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method debits a mandate based on the provided debit mandate model.
        /// </summary>
        /// <param name="debitMandateModel">The model containing the details required to debit the mandate.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a DebitAccountResponse.</returns>
        Task<MonoStandardResponse<DebitAccountResponse>> DebitMandate(DebitAccountModel debitMandateModel, string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method checks the balance of a mandate based on the provided mandate id and amount.
        /// </summary>
        /// <param name="id">The mandate id used to check the balance.</param>
        /// <param name="amount">The amount used to check the balance of the mandate.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a BalanceEnquiryResponse.</returns>
        Task<MonoStandardResponse<BalanceEnquiryResponse>> BalanceInquiry(string id, string amount, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method reinstates a mandate based on the provided mandate id.
        /// </summary>
        /// <param name="id">The mandate id used to reinstate the mandate.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse.</returns>
        /// <returns></returns>
        Task<MonoStandardResponse<dynamic>> ReinstateMandate(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method pauses a mandate based on the provided mandate id.
        /// </summary>
        /// <param name="id">The mandate id used to pause the mandate.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse.</returns>
        Task<MonoStandardResponse<dynamic>> PauseMandate(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method cancels a mandate based on the provided mandate id.
        /// </summary>
        /// <param name="id">The mandate id used to cancel the mandate.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse.</returns>
        Task<MonoStandardResponse<dynamic>> CancelMandate(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method gets a list of mandates based on the provided query options.
        /// </summary>
        /// <param name="options">The query options to filter and retrieve mandates.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a list of MonoMandate.</returns>
        Task<MonoStandardResponse<List<MonoMandate>>> GetMandates(MandateRequestQueryOptions options, CancellationToken cancellationToken = default);


        /// <summary>
        /// This method gets a mandate based on the provided mandate id.
        /// </summary>
        /// <param name="id">The mandate id used to retrieve the mandate.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a MonoMandate.</returns>
        Task<MonoStandardResponse<MonoMandate>> GetMandateById(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method sets the OTP method for mandate verification.
        /// </summary>
        /// <param name="verifyOtpModel">The model containing the details required to set the OTP method for mandate verification.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a SetOtpMethodResponse.</returns>
        /// <returns></returns>
        Task<MonoStandardResponse<SetOtpMethodResponse>> SetOtpMethod(VerifyMandateOtpModel verifyOtpModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method verifies the OTP and gets the mandate.
        /// </summary>
        /// <param name="verifyOtpModel">The model containing the details required to verify the OTP and get the mandate.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a MandateResponse.</returns>
        Task<MonoStandardResponse<MandateResponse>> VerifyOtpAndGetMandate(VerifyMandateOtpModel verifyOtpModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// This method creates a mandate based on the provided create mandate model.
        /// </summary>
        /// <param name="createMandateModel">The model containing the details required to create the mandate.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a TokenizedMandateResponse.</returns>
        Task<MonoStandardResponse<TokenizedMandateResponse>> CreateMandate(CreateMandateModel createMandateModel, CancellationToken cancellationToken = default);
    }
}

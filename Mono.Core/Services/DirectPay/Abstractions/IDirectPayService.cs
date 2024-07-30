using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Mono.Core.DirectPay
{
    public interface IDirectPayService
    {
         // /payments/initiate

        /// <summary>
        /// Initiates a one-time payment based on the provided payment request model.
        /// </summary>
        /// <param name="initiatePaymentRequestModel">The model containing the details required to initiate the one-time payment.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IApiResponse with a MonoStandardResponse containing an InitiateOneTimePaymentResponse.</returns>
        [Post("/payments/initiate")]
        Task<IApiResponse<MonoStandardResponse<InitiateOneTimePaymentResponse>>> InitiatePayment(InitiateOneTimePayment initiatePaymentRequestModel, CancellationToken cancellationToken = default);

        // /payments/verify

          /// <summary>
        /// Verifies a payment using the provided reference.
        /// </summary>
        /// <param name="reference">The reference used to verify the payment.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IApiResponse with a MonoStandardResponse containing a VerifyPaymentReferenceResponse.</returns>
        [Post("/payments/verify/{reference}")]
        Task<IApiResponse<MonoStandardResponse<VerifyPaymentReferenceResponse>>> VerifyPayment(string reference, CancellationToken cancellationToken = default);

        // payments/transactions?page=1&start=01-01-2022&end=10-01-2022&status=successful'

         /// <summary>
        /// Retrieves a list of payment transactions based on the provided query options.
        /// </summary>
        /// <param name="options">The query options to filter and retrieve the payment transactions.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an IApiResponse with a MonoStandardResponse containing a list of PaymentResponseModel.</returns>
        [Get("/payments/transactions")]
        Task<IApiResponse<MonoStandardResponse<List<PaymentResponseModel>>>> GetTransactions([Query] PaymentRequestQueryOptions options, CancellationToken cancellationToken = default);


    }
}

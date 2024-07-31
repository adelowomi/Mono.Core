using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.DirectPay
{
    public class DirectPayService : IMonoDirectPay
    {
        private readonly IDirectPayService _directPayService;

        public DirectPayService(IRefitClientBuilder<IDirectPayService> directPayService)
        {
            _directPayService = directPayService.Build();
        }

           /// <summary>
        /// Initiates a one-time payment based on the provided payment request model.
        /// </summary>
        /// <param name="initiatePaymentRequestModel">The model containing the details required to initiate the one-time payment.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns result that contains a MonoStandardResponse containing an InitiateOneTimePaymentResponse.</returns>
        public async Task<MonoStandardResponse<InitiateOneTimePaymentResponse>> InitiatePayment(InitiateOneTimePayment initiatePaymentRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _directPayService.InitiatePayment(initiatePaymentRequestModel, cancellationToken);
            return response.HandleResponse();
        }

        /// <summary>
        /// Verifies the payment using provided reference
        /// </summary>
        /// <param name="reference">The reference used to verify the payment</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse containing a VerifyPaymentReferenceResponse.</returns>
        public async Task<MonoStandardResponse<VerifyPaymentReferenceResponse>> VerifyPayment(string reference, CancellationToken cancellationToken = default)
        {
            var response = await _directPayService.VerifyPayment(reference, cancellationToken);
            return response.HandleResponse();
        }

        /// <summary>
        /// Get a list of payment transactions based on the provided query options.
        /// </summary>
        /// <param name="options">The query options to filter and retrive payment transactions.</param>
        /// <param name="cancellationToken">A Cancellation token that can be used to cancel the task. This will terminate the HTTP request if triggered.</param>
        /// <returns>A synchronous task that returns a MonoStandardResponse and a list of PaymentResponseModel.</returns>
        public async Task<MonoStandardResponse<List<PaymentResponseModel>>> GetTransactions(PaymentRequestQueryOptions options, CancellationToken cancellationToken = default)
        {
            var response = await _directPayService.GetTransactions(options, cancellationToken);
            return response.HandleResponse();
        }
    }
}

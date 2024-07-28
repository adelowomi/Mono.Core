using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.DirectPay
{
    public interface IMonoDirectPay
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiatePaymentRequestModel"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MonoStandardResponse<InitiateOneTimePaymentResponse>> InitiatePayment(InitiateOneTimePayment initiatePaymentRequestModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MonoStandardResponse<VerifyPaymentReferenceResponse>> VerifyPayment(string reference, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MonoStandardResponse<List<PaymentResponseModel>>> GetTransactions(PaymentRequestQueryOptions options, CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mono.Core.DirectPay
{
    public interface IMonoDirectPayService
    {
        Task<MonoStandardResponse<InitiateOneTimePaymentResponse>> InitiatePayment(InitiateOneTimePayment initiatePaymentRequestModel, CancellationToken cancellationToken = default);

        Task<MonoStandardResponse<VerifyPaymentReferenceResponse>> VerifyPayment(string reference, CancellationToken cancellationToken = default);

        Task<MonoStandardResponse<List<PaymentResponseModel>>> GetTransactions(PaymentRequestQueryOptions options, CancellationToken cancellationToken = default);
    }
}

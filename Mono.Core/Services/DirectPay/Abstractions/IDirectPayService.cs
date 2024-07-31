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
        [Post("/payments/initiate")]
       Task<IApiResponse<MonoStandardResponse<InitiateOneTimePaymentResponse>>> InitiatePayment(InitiateOneTimePayment initiatePaymentRequestModel, CancellationToken cancellationToken = default);
        

        // /payments/verify
        [Post("/payments/verify/{reference}")]
        Task<IApiResponse<MonoStandardResponse<VerifyPaymentReferenceResponse>>> VerifyPayment(string reference, CancellationToken cancellationToken = default);

        // payments/transactions?page=1&start=01-01-2022&end=10-01-2022&status=successful'
        [Get("/payments/transactions")]
        Task<IApiResponse<MonoStandardResponse<List<PaymentResponseModel>>>> GetTransactions([Query] PaymentRequestQueryOptions options, CancellationToken cancellationToken = default);


    }
}

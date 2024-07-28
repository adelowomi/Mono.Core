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

        public async Task<MonoStandardResponse<InitiateOneTimePaymentResponse>> InitiatePayment(InitiateOneTimePayment initiatePaymentRequestModel, CancellationToken cancellationToken = default)
        {
            var response = await _directPayService.InitiatePayment(initiatePaymentRequestModel, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<VerifyPaymentReferenceResponse>> VerifyPayment(string reference, CancellationToken cancellationToken = default)
        {
            var response = await _directPayService.VerifyPayment(reference, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<List<PaymentResponseModel>>> GetTransactions(PaymentRequestQueryOptions options, CancellationToken cancellationToken = default)
        {
            var response = await _directPayService.GetTransactions(options, cancellationToken);
            return response.HandleResponse();
        }
    }
}

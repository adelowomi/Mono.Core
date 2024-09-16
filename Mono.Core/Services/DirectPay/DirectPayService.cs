using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mono.Core.Services.DirectPay.Models;

namespace Mono.Core.DirectPay
{
    public class DirectPayService : IMonoDirectPay
    {
        private readonly IDirectPayService _directPayService;
        private readonly IDirectPayService _directPayServiceV3;

        public DirectPayService(IRefitClientBuilder<IDirectPayService> directPayService, IRefitClientBuilder<IDirectPayService> directPayServiceV3)
        {
            _directPayService = directPayService.Build();
            _directPayServiceV3 = directPayServiceV3.BuildV3();
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
            var response = await _directPayServiceV3.GetTransactions(options, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<List<RetrieveDebitResponse>>> RetrieveAllDebits(string id, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.RetrieveAllDebits(id, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<RetrieveDebitResponse>> RetrieveDebitByReference(string id, string reference, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.RetrieveDebitByReference(id, reference, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<DebitAccountResponse>> DebitMandate(DebitAccountModel debitMandateModel,string mandateId, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.DebitMandate(debitMandateModel, mandateId, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<BalanceEnquiryResponse>> BalanceInquiry(string id, string amount, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.BalanceInquiry(id, amount, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<dynamic>> ReinstateMandate(string id, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.ReinstateMandate(id, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<dynamic>> PauseMandate(string id, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.PauseMandate(id, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<dynamic>> CancelMandate(string id, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.CancelMandate(id, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<List<MonoMandate>>> GetMandates(MandateRequestQueryOptions options, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.GetMandates(options, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<MonoMandate>> GetMandateById(string id, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.GetMandateById(id, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<SetOtpMethodResponse>> SetOtpMethod(VerifyMandateOtpModel verifyOtpModel, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.SetOtpMethod(verifyOtpModel, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<MandateResponse>> VerifyOtpAndGetMandate(VerifyMandateOtpModel verifyOtpModel, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.VerifyOtpAndGetMandate(verifyOtpModel, cancellationToken);
            return response.HandleResponse();
        }

        public async Task<MonoStandardResponse<TokenizedMandateResponse>> CreateMandate(CreateMandateModel createMandateModel, CancellationToken cancellationToken = default)
        {
            var response = await _directPayServiceV3.CreateMandate(createMandateModel, cancellationToken);
            return response.HandleResponse();
        }
    }
}

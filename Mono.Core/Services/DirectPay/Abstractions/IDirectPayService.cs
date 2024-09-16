using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mono.Core.Services.DirectPay.Models;
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

        // /payments/mandates/id/debits
        [Get("/payments/mandates/{id}/debits")]
        Task<IApiResponse<MonoStandardResponse<List<RetrieveDebitResponse>>>> RetrieveAllDebits(string id, CancellationToken cancellationToken = default);

        // /payments/mandates/id/debits/reference 
        [Get("/payments/mandates/{id}/debits/{reference}")]
        Task<IApiResponse<MonoStandardResponse<RetrieveDebitResponse>>> RetrieveDebitByReference(string id, string reference, CancellationToken cancellationToken = default);

        // /payments/mandates/id/debit 
        [Post("/payments/mandates/{id}/debit")]
        Task<IApiResponse<MonoStandardResponse<DebitAccountResponse>>> DebitMandate(DebitAccountModel debitMandateModel,string id, CancellationToken cancellationToken = default);

        // /payments/mandates/id/balance-inquiry?amount=1000
        [Get("/payments/mandates/{id}/balance-inquiry")]
        Task<IApiResponse<MonoStandardResponse<BalanceEnquiryResponse>>> BalanceInquiry(string id, [Query] string amount, CancellationToken cancellationToken = default);

        // /payments/mandates/id/reinstate
        [Patch("/payments/mandates/{id}/reinstate")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> ReinstateMandate(string id, CancellationToken cancellationToken = default);

        // payments/mandates/id/pause 
        [Patch("/payments/mandates/{id}/pause")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> PauseMandate(string id, CancellationToken cancellationToken = default);

        // /payments/mandates/id/cancel 
        [Patch("/payments/mandates/{id}/cancel")]
        Task<IApiResponse<MonoStandardResponse<dynamic>>> CancelMandate(string id, CancellationToken cancellationToken = default);

        //  /payments/mandates?limit=10&page=1
        [Get("/payments/mandates")]
        Task<IApiResponse<MonoStandardResponse<List<MonoMandate>>>> GetMandates([Query] MandateRequestQueryOptions options, CancellationToken cancellationToken = default);

        // /payments/mandates/id
        [Get("/payments/mandates/{id}")]
        Task<IApiResponse<MonoStandardResponse<MonoMandate>>> GetMandateById(string id, CancellationToken cancellationToken = default);

        // /payments/mandates/verify/otp 
        [Post("/payments/mandates/verify/otp")]
        Task<IApiResponse<MonoStandardResponse<SetOtpMethodResponse>>> SetOtpMethod([Body]VerifyMandateOtpModel verifyOtpModel, CancellationToken cancellationToken = default);

        // /payments/mandates/verify/otp 
        [Post("/payments/mandates/verify/otp")]
        Task<IApiResponse<MonoStandardResponse<MandateResponse>>> VerifyOtpAndGetMandate([Body]VerifyMandateOtpModel verifyOtpModel, CancellationToken cancellationToken = default);

        // /payments/mandates
        [Post("/payments/mandates")]
        Task<IApiResponse<MonoStandardResponse<TokenizedMandateResponse>>> CreateMandate(CreateMandateModel createMandateModel, CancellationToken cancellationToken = default);

    }
}

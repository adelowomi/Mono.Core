using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Refit;
using Xunit;
using Mono.Core;
using Mono.Core.Accounts;
using Mono.Core.DirectPay;
using Mono.Core.LookUp;
using Mono.Core.Services.DirectPay.Models;

namespace Mono.Core.NewSurfaceCoverage.Tests
{
    /// <summary>
    /// Coverage for endpoints added in v1.6.0 (Connect: balance, account-match,
    /// NIN PDF), v1.8.0 (CAC: PSC/Profile/Status-Report), and v1.9.0
    /// (DirectPay: refund, payouts, sub-accounts). One happy-path test per
    /// endpoint — enough to catch wiring/signature regressions.
    /// </summary>
    public class NewSurfaceCoverageTests
    {
        private static ApiResponse<MonoStandardResponse<T>> Ok<T>(T data) =>
            new ApiResponse<MonoStandardResponse<T>>(
                new HttpResponseMessage(HttpStatusCode.OK),
                new MonoStandardResponse<T> { Data = data, Status = "successful", Success = true },
                new RefitSettings());

        // ============ v1.6.0 — Account balance ============

        [Fact]
        public async Task GetAccountBalance_RoutesToConnectBuilder()
        {
            var mockRefit = new Mock<IAccountService>();
            var builder = new Mock<IRefitClientBuilder<IAccountService>>();
            builder.Setup(x => x.Build(It.IsAny<string>())).Returns(mockRefit.Object);
            var service = new AccountService(builder.Object);

            var expected = new AccountBalanceResponse
            {
                Id = "acc_1",
                AccountNumber = "0123456789",
                Currency = "NGN",
                Balance = 1_250_000_00,
                AvailableBalance = 1_200_000_00,
            };
            mockRefit.Setup(x => x.GetAccountBalance("acc_1", It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.GetAccountBalance("acc_1");

            Assert.Equal(1_250_000_00, result.Data.Balance);
            Assert.Equal(1_200_000_00, result.Data.AvailableBalance);
        }

        // ============ v1.6.0 — NIN PDF + poll job ============

        [Fact]
        public async Task GetNinPdf_ReturnsJobInitiation()
        {
            var lookup = new Mock<ILookUpService>();
            var v3 = new Mock<ILookUpService>();
            var builder = new Mock<IRefitClientBuilder<ILookUpService>>();
            builder.Setup(x => x.Build(It.IsAny<string>())).Returns(lookup.Object);
            builder.Setup(x => x.BuildV3(It.IsAny<string>())).Returns(v3.Object);
            var service = new LookUpService(builder.Object);

            var expected = new NinPdfJobInitiationResponse { JobId = "job_42", Status = NinJobStatusConstants.Processing };
            v3.Setup(x => x.GetNinPdf(It.IsAny<NinPdfRequestModel>(), It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.GetNinPdf(new NinPdfRequestModel { Nin = "12345678901" });

            Assert.Equal("job_42", result.Data.JobId);
            Assert.Equal(NinJobStatusConstants.Processing, result.Data.Status);
        }

        [Fact]
        public async Task PollNinJob_ReturnsCompletedWithUrl()
        {
            var lookup = new Mock<ILookUpService>();
            var v3 = new Mock<ILookUpService>();
            var builder = new Mock<IRefitClientBuilder<ILookUpService>>();
            builder.Setup(x => x.Build(It.IsAny<string>())).Returns(lookup.Object);
            builder.Setup(x => x.BuildV3(It.IsAny<string>())).Returns(v3.Object);
            var service = new LookUpService(builder.Object);

            var expected = new NinPollJobResponse
            {
                JobId = "job_42",
                Status = NinJobStatusConstants.Completed,
                Url = "https://files.mono.co/nin/job_42.pdf",
                Result = new NinResponseModel { Nin = "12345678901", Firstname = "Ada" },
            };
            v3.Setup(x => x.PollNinJob("job_42", It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.PollNinJob("job_42");

            Assert.Equal(NinJobStatusConstants.Completed, result.Data.Status);
            Assert.StartsWith("https://", result.Data.Url);
            Assert.Equal("Ada", result.Data.Result.Firstname);
        }

        // ============ v1.8.0 — CAC PSC + Profile + Status Report ============

        [Fact]
        public async Task GetCacPsc_ReturnsList()
        {
            var lookup = new Mock<ILookUpService>();
            var v3 = new Mock<ILookUpService>();
            var builder = new Mock<IRefitClientBuilder<ILookUpService>>();
            builder.Setup(x => x.Build(It.IsAny<string>())).Returns(lookup.Object);
            builder.Setup(x => x.BuildV3(It.IsAny<string>())).Returns(v3.Object);
            var service = new LookUpService(builder.Object);

            var expected = new List<CacPscEntry>
            {
                new CacPscEntry { Name = "Ada Lovelace", OwnershipPercentage = 51.0, NatureOfControl = new List<string> { "voting_rights" } },
                new CacPscEntry { Name = "Grace Hopper", OwnershipPercentage = 49.0 },
            };
            v3.Setup(x => x.GetCacPsc("biz_1", It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.GetCacPsc("biz_1");

            Assert.Equal(2, result.Data.Count);
            Assert.Equal(51.0, result.Data[0].OwnershipPercentage);
        }

        [Fact]
        public async Task GetCacProfile_TakesRcNumberNotBusinessId()
        {
            var lookup = new Mock<ILookUpService>();
            var v3 = new Mock<ILookUpService>();
            var builder = new Mock<IRefitClientBuilder<ILookUpService>>();
            builder.Setup(x => x.Build(It.IsAny<string>())).Returns(lookup.Object);
            builder.Setup(x => x.BuildV3(It.IsAny<string>())).Returns(v3.Object);
            var service = new LookUpService(builder.Object);

            var expected = new CacProfileResponse
            {
                Business = new BusinessDetails { ApprovedName = "ACME LIMITED", RcNumber = "RC123456" },
                Directors = new List<OfficialDetails> { new OfficialDetails { Surname = "Lovelace", Firstname = "Ada" } },
                Shareholders = new List<OfficialDetails>(),
                Secretaries = new List<OfficialDetails>(),
                Psc = new List<CacPscEntry>(),
            };
            v3.Setup(x => x.GetCacProfile("RC123456", It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.GetCacProfile("RC123456");

            Assert.Equal("ACME LIMITED", result.Data.Business.ApprovedName);
            Assert.Single(result.Data.Directors);
        }

        [Fact]
        public async Task GetCacStatusReport_OnSuccess_WrapsPdfBytes()
        {
            var lookup = new Mock<ILookUpService>();
            var v3 = new Mock<ILookUpService>();
            var builder = new Mock<IRefitClientBuilder<ILookUpService>>();
            builder.Setup(x => x.Build(It.IsAny<string>())).Returns(lookup.Object);
            builder.Setup(x => x.BuildV3(It.IsAny<string>())).Returns(v3.Object);
            var service = new LookUpService(builder.Object);

            var pdfBytes = Encoding.ASCII.GetBytes("%PDF-1.4\n%fake-cac-report");
            var http = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(pdfBytes) };
            v3.Setup(x => x.GetCacStatusReport("biz_1", It.IsAny<CancellationToken>())).ReturnsAsync(http);

            var result = await service.GetCacStatusReport("biz_1");

            Assert.True(result.Success);
            Assert.Equal(pdfBytes.Length, result.Data.Length);
        }

        // ============ v1.9.0 — DirectPay money-ops ============

        private static DirectPayService BuildDirectPay(Mock<IDirectPayService> mockRefit)
        {
            var builder = new Mock<IRefitClientBuilder<IDirectPayService>>();
            builder.Setup(x => x.Build(It.IsAny<string>())).Returns(mockRefit.Object);
            builder.Setup(x => x.BuildV3(It.IsAny<string>())).Returns(mockRefit.Object);
            return new DirectPayService(builder.Object, builder.Object);
        }

        [Fact]
        public async Task RefundPayment_PassesReferenceAndSource()
        {
            var mockRefit = new Mock<IDirectPayService>();
            var service = BuildDirectPay(mockRefit);
            var model = new RefundPaymentModel { Reference = "pay_abc", Source = RefundSourceConstants.Wallet };
            var expected = new RefundPaymentResponse { Id = "rfd_1", Reference = "pay_abc", Status = "processed", Source = "wallet" };
            mockRefit.Setup(x => x.RefundPayment(model, It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.RefundPayment(model);

            Assert.Equal("rfd_1", result.Data.Id);
            Assert.Equal("wallet", result.Data.Source);
        }

        [Fact]
        public async Task GetPayouts_FiltersByStatus()
        {
            var mockRefit = new Mock<IDirectPayService>();
            var service = BuildDirectPay(mockRefit);
            var options = new PayoutListQueryOptions { Status = PayoutStatusConstants.Settled };
            var expected = new PayoutListResponse
            {
                Payouts = new List<PayoutResponse>
                {
                    new PayoutResponse { Id = "pay_1", Status = PayoutStatusConstants.Settled, Amount = 100_000 },
                },
                Meta = new PayoutPaginationMeta { Total = 1, Page = 1 },
            };
            mockRefit.Setup(x => x.GetPayouts(options, It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.GetPayouts(options);

            Assert.Single(result.Data.Payouts);
            Assert.Equal(PayoutStatusConstants.Settled, result.Data.Payouts[0].Status);
        }

        [Fact]
        public async Task GetPayoutTransactions_PassesPayoutIdInPath()
        {
            var mockRefit = new Mock<IDirectPayService>();
            var service = BuildDirectPay(mockRefit);
            var expected = new PayoutTransactionsResponse
            {
                Transactions = new List<PayoutTransaction>
                {
                    new PayoutTransaction { Id = "ptx_1", Payout = "pay_1", Amount = 50_000 },
                },
                Meta = new PayoutPaginationMeta { Total = 1, Page = 1 },
            };
            mockRefit.Setup(x => x.GetPayoutTransactions("pay_1", It.IsAny<PayoutTransactionsQueryOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.GetPayoutTransactions("pay_1");

            Assert.Single(result.Data.Transactions);
            Assert.Equal("pay_1", result.Data.Transactions[0].Payout);
        }

        [Fact]
        public async Task CreateSubAccount_RoundTripsFields()
        {
            var mockRefit = new Mock<IDirectPayService>();
            var service = BuildDirectPay(mockRefit);
            var model = new CreateSubAccountModel { Name = "Vendor A", AccountNumber = "0123456789", BankCode = "044", NipCode = "000014" };
            var expected = new SubAccountResponse { Id = "sub_1", Name = "Vendor A", AccountNumber = "0123456789", Status = "active" };
            mockRefit.Setup(x => x.CreateSubAccount(model, It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await service.CreateSubAccount(model);

            Assert.Equal("sub_1", result.Data.Id);
            Assert.Equal("active", result.Data.Status);
        }
    }
}

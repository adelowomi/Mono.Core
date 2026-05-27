using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Refit;
using Xunit;
using Mono.Core;
using Mono.Core.Disburse;

namespace Mono.Core.Disburse.Tests
{
    public class DisburseServiceTests
    {
        private readonly Mock<IDisburseService> _mockRefit;
        private readonly DisburseService _service;

        public DisburseServiceTests()
        {
            _mockRefit = new Mock<IDisburseService>();
            var builder = new Mock<IRefitClientBuilder<IDisburseService>>();
            // Disburse uses BuildV3() — set up only the v3 path.
            builder.Setup(x => x.BuildV3(It.IsAny<string>())).Returns(_mockRefit.Object);
            _service = new DisburseService(builder.Object);
        }

        private static ApiResponse<MonoStandardResponse<T>> Ok<T>(T data) =>
            new ApiResponse<MonoStandardResponse<T>>(
                new HttpResponseMessage(HttpStatusCode.OK),
                new MonoStandardResponse<T> { Data = data, Status = "successful", Success = true },
                new RefitSettings());

        [Fact]
        public async Task CreateSourceAccount_ShouldUnwrap()
        {
            var model = new CreateSourceAccountModel
            {
                App = "app_123",
                AccountNumber = "0123456789",
                BankCode = "044",
                Email = "ops@example.com",
            };
            var expected = new SourceAccountResponse { Id = "src_abc", AccountNumber = "0123456789", Status = "active" };
            _mockRefit.Setup(x => x.CreateSourceAccount(model, It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await _service.CreateSourceAccount(model);

            Assert.Equal("src_abc", result.Data.Id);
        }

        [Fact]
        public async Task CreateInstantDisbursement_SetsTypeFlag()
        {
            CreateDisbursementModel captured = null;
            var expected = new DisbursementResponse { Id = "dsb_1", Status = "processing", Type = "instant" };
            _mockRefit.Setup(x => x.CreateDisbursement(It.IsAny<CreateDisbursementModel>(), It.IsAny<CancellationToken>()))
                .Callback<CreateDisbursementModel, CancellationToken>((m, _) => captured = m)
                .ReturnsAsync(Ok(expected));

            var model = new CreateDisbursementModel
            {
                Reference = "payroll-2026-05",
                Account = "src_abc",
                TotalAmount = 25_000_00,
                Description = "May payroll",
                Distribution = new List<DistributionModel>
                {
                    new DistributionModel
                    {
                        Reference = "p-ada",
                        RecipientEmail = "ada@example.com",
                        Account = new DisbursementRecipientAccount { AccountNumber = "0123456789", BankCode = "044" },
                        Amount = 25_000_00,
                        Narration = "May salary",
                    },
                },
                // intentionally omit Type to verify wrapper sets it
            };

            var result = await _service.CreateInstantDisbursement(model);

            Assert.Equal(DisbursementTypeConstants.Instant, captured.Type);
            Assert.Equal("dsb_1", result.Data.Id);
        }

        [Fact]
        public async Task CreateScheduledDisbursement_SetsTypeFlag()
        {
            CreateDisbursementModel captured = null;
            var expected = new DisbursementResponse { Id = "dsb_2", Type = "scheduled" };
            _mockRefit.Setup(x => x.CreateDisbursement(It.IsAny<CreateDisbursementModel>(), It.IsAny<CancellationToken>()))
                .Callback<CreateDisbursementModel, CancellationToken>((m, _) => captured = m)
                .ReturnsAsync(Ok(expected));

            var model = new CreateDisbursementModel
            {
                Reference = "future-1",
                Account = "src_abc",
                TotalAmount = 1000,
                Description = "future payout",
                ScheduledDate = "2026-06-01T09:00:00Z",
                Distribution = new List<DistributionModel>(),
            };

            await _service.CreateScheduledDisbursement(model);

            Assert.Equal(DisbursementTypeConstants.Scheduled, captured.Type);
        }

        [Fact]
        public async Task TransitionDisbursement_ShouldPassActionThrough()
        {
            var expected = new DisbursementResponse { Id = "dsb_2", Status = "cancelled" };
            _mockRefit.Setup(x => x.TransitionDisbursement("dsb_2", It.Is<TransitionDisbursementModel>(t => t.Action == TransitionActionConstants.Cancel), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Ok(expected));

            var result = await _service.TransitionDisbursement("dsb_2", new TransitionDisbursementModel { Action = TransitionActionConstants.Cancel });

            Assert.Equal("cancelled", result.Data.Status);
        }
    }
}

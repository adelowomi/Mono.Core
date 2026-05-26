using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Refit;
using Xunit;
using Mono.Core;
using Mono.Core.Prove;

namespace Mono.Core.Prove.Tests
{
    public class ProveServiceTests
    {
        private readonly Mock<IProveService> _mockRefit;
        private readonly ProveService _service;

        public ProveServiceTests()
        {
            _mockRefit = new Mock<IProveService>();
            var builder = new Mock<IRefitClientBuilder<IProveService>>();
            // Prove uses BuildV1() — /v1/prove/...
            builder.Setup(x => x.BuildV1(It.IsAny<string>())).Returns(_mockRefit.Object);
            _service = new ProveService(builder.Object);
        }

        private static ApiResponse<MonoStandardResponse<T>> Ok<T>(T data) =>
            new ApiResponse<MonoStandardResponse<T>>(
                new HttpResponseMessage(HttpStatusCode.OK),
                new MonoStandardResponse<T> { Data = data, Status = "successful", Success = true },
                new RefitSettings());

        [Fact]
        public async Task InitiateProve_ReturnsMonoUrlAndSessionId()
        {
            var model = new InitiateProveModel
            {
                Customer = new ProveCustomer
                {
                    Name = "Ada Lovelace",
                    Phone = "+2348012345678",
                    Address = "12 Analytical St, Lagos",
                    Email = "ada@example.com",
                    Identity = new ProveCustomerIdentity { Type = ProveIdentityTypeConstants.Bvn, Number = "12345678901" },
                },
                Reference = "kyc-ada-1",
                RedirectUrl = "https://app.example.com/done",
                KycLevel = ProveKycLevelConstants.Tier2,
            };
            var expected = new InitiateProveResponse
            {
                MonoUrl = "https://prove.mono.co/sess_abc",
                SessionId = "sess_abc",
                Reference = "kyc-ada-1",
            };
            _mockRefit.Setup(x => x.InitiateProve(model, It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await _service.InitiateProve(model);

            Assert.Equal("sess_abc", result.Data.SessionId);
            Assert.StartsWith("https://", result.Data.MonoUrl);
        }

        [Fact]
        public async Task BlacklistCustomer_ShouldPassCodeAndReason()
        {
            var model = new BlacklistCustomerModel
            {
                Reference = "kyc-ada-1",
                Reason = "Confirmed fraud",
                Code = ProveBlacklistCodeConstants.Code101,
            };
            var expected = new ProveCustomerResponse
            {
                Reference = "kyc-ada-1",
                Blacklisted = true,
                BlacklistReason = "Confirmed fraud",
                BlacklistCode = ProveBlacklistCodeConstants.Code101,
            };
            _mockRefit.Setup(x => x.BlacklistCustomer(model, It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await _service.BlacklistCustomer(model);

            Assert.True(result.Data.Blacklisted);
            Assert.Equal(101, result.Data.BlacklistCode);
        }

        [Fact]
        public async Task RevokeDataAccess_ShouldRoundTripReference()
        {
            _mockRefit.Setup(x => x.RevokeDataAccess("kyc-ada-1", It.IsAny<CancellationToken>())).ReturnsAsync(Ok<dynamic>(null));

            var result = await _service.RevokeDataAccess("kyc-ada-1");

            Assert.True(result.Success);
            _mockRefit.Verify(x => x.RevokeDataAccess("kyc-ada-1", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

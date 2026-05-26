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
using Mono.Core.Watchlist;

namespace Mono.Core.Watchlist.Tests
{
    public class WatchlistServiceTests
    {
        private readonly Mock<IWatchlistService> _mockRefit;
        private readonly WatchlistService _service;

        public WatchlistServiceTests()
        {
            _mockRefit = new Mock<IWatchlistService>();
            var builder = new Mock<IRefitClientBuilder<IWatchlistService>>();
            builder.Setup(x => x.BuildV3(It.IsAny<string>())).Returns(_mockRefit.Object);
            _service = new WatchlistService(builder.Object);
        }

        private static ApiResponse<MonoStandardResponse<T>> Ok<T>(T data) =>
            new ApiResponse<MonoStandardResponse<T>>(
                new HttpResponseMessage(HttpStatusCode.OK),
                new MonoStandardResponse<T> { Data = data, Status = "successful", Success = true },
                new RefitSettings());

        [Fact]
        public async Task SubmitIndividualScreening_ShouldUnwrap()
        {
            var model = new SubmitIndividualScreeningModel
            {
                Name = "Ada Lovelace",
                DateOfBirth = "1815-12-10",
                Country = "NG",
                Bvn = "12345678901",
            };
            var expected = new ScreeningResponse
            {
                Id = "scr_1",
                Status = ScreeningStatusConstants.Completed,
                RiskScore = 12.5,
                RiskLevel = RiskLevelConstants.Low,
                Matches = new List<WatchlistMatch>(),
            };

            _mockRefit.Setup(x => x.SubmitIndividualScreening(model, It.IsAny<CancellationToken>())).ReturnsAsync(Ok(expected));

            var result = await _service.SubmitIndividualScreening(model);

            Assert.Equal(RiskLevelConstants.Low, result.Data.RiskLevel);
        }

        [Fact]
        public async Task GetScreeningReport_OnSuccess_WrapsBytes()
        {
            // Mono returns a binary PDF; the Refit method returns HttpResponseMessage
            // and the high-level service reads the bytes.
            var pdfBytes = Encoding.ASCII.GetBytes("%PDF-1.4\n%fake-test-pdf");
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(pdfBytes),
            };
            _mockRefit.Setup(x => x.GetScreeningReport("scr_1", It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _service.GetScreeningReport("scr_1");

            Assert.True(result.Success);
            Assert.Equal(pdfBytes.Length, result.Data.Length);
            Assert.Equal("%PDF-1.4", Encoding.ASCII.GetString(result.Data, 0, 8));
        }

        [Fact]
        public async Task GetScreeningReport_OnJsonError_ParsesStandardResponse()
        {
            var errorJson = "{\"status\":\"failed\",\"message\":\"Screening not found\",\"errors\":[]}";
            var response = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(errorJson, Encoding.UTF8, "application/json"),
            };
            _mockRefit.Setup(x => x.GetScreeningReport("missing", It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _service.GetScreeningReport("missing");

            Assert.False(result.Success);
            Assert.Equal("Screening not found", result.Message);
        }

        [Fact]
        public async Task GetScreeningReport_OnNonJsonError_FallsBackToGenericMessage()
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("internal server error", Encoding.UTF8, "text/plain"),
            };
            _mockRefit.Setup(x => x.GetScreeningReport("oops", It.IsAny<CancellationToken>())).ReturnsAsync(response);

            var result = await _service.GetScreeningReport("oops");

            Assert.False(result.Success);
            Assert.Contains("internal server error", result.Message);
        }
    }
}

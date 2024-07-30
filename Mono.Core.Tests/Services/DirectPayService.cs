using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Refit;
using Xunit;
using Mono.Core.DirectPay;
using Xunit.Sdk;

namespace Mono.Core.Tests
{
    public class DirectPayServiceTests
    {
        private readonly Mock<IDirectPayService> _mockDirectPayService;
        private readonly DirectPayService _directPayService;

        public DirectPayServiceTests()
        {
            _mockDirectPayService = new Mock<IDirectPayService>();
            var refitClientBuilder = new Mock<IRefitClientBuilder<IDirectPayService>>();
            refitClientBuilder.Setup(x => x.Build()).Returns(_mockDirectPayService.Object);
            _directPayService = new DirectPayService(refitClientBuilder.Object);
        }

        [Fact]
        public async Task InitiatePayment_ShouldReturnExpectedResponse()
        {
            // Arrange
            var initiatePaymentRequestModel = new InitiateOneTimePayment ();
            var expectedResponse = new MonoStandardResponse<InitiateOneTimePaymentResponse> ();
            _mockDirectPayService.Setup(x => x.InitiatePayment(It.IsAny<InitiateOneTimePayment>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<InitiateOneTimePaymentResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _directPayService.InitiatePayment(initiatePaymentRequestModel);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockDirectPayService.Verify(x => x.InitiatePayment(initiatePaymentRequestModel, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InitiatePayment_ShouldHandleErrorResponse()
        {
            // Arrange
            var initiatePaymentRequestModel = new InitiateOneTimePayment ();
            var errorResponse = new ApiResponse<MonoStandardResponse<InitiateOneTimePaymentResponse>>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, new RefitSettings());
            _mockDirectPayService.Setup(x => x.InitiatePayment(It.IsAny<InitiateOneTimePayment>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(errorResponse);

            // Act & Assert
            await Assert.ThrowsAsync <NullReferenceException>(() => _directPayService.InitiatePayment(initiatePaymentRequestModel));
            _mockDirectPayService.Verify(x => x.InitiatePayment(initiatePaymentRequestModel, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task VerifyPayment_ShouldReturnExpectedResponse()
        {
            // Arrange
            var reference = "test-reference";
            var expectedResponse = new MonoStandardResponse<VerifyPaymentReferenceResponse>();
            _mockDirectPayService.Setup(x => x.VerifyPayment(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<VerifyPaymentReferenceResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _directPayService.VerifyPayment(reference);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockDirectPayService.Verify(x => x.VerifyPayment(reference, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task VerifyPayment_ShouldHandleErrorResponse()
        {
            // Arrange
            var reference = "test-reference";
            var errorResponse = new ApiResponse<MonoStandardResponse<VerifyPaymentReferenceResponse>>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, new RefitSettings());
            var failedVerification = _mockDirectPayService.Setup(x => x.VerifyPayment(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(errorResponse);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _directPayService.VerifyPayment(reference));
            _mockDirectPayService.Verify(x => x.VerifyPayment(reference, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTransactions_ShouldReturnExpectedResponse()
        {
            // Arrange
            var options = new PaymentRequestQueryOptions();
            var expectedResponse = new MonoStandardResponse<List<PaymentResponseModel>> ();
            _mockDirectPayService.Setup(x => x.GetTransactions(It.IsAny<PaymentRequestQueryOptions>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<List<PaymentResponseModel>>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _directPayService.GetTransactions(options);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockDirectPayService.Verify(x => x.GetTransactions(options, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetTransactions_ShouldHandleErrorResponse()
        {
            // Arrange
            var options = new PaymentRequestQueryOptions ();
            var errorResponse = new ApiResponse<MonoStandardResponse<List<PaymentResponseModel>>>(new HttpResponseMessage(HttpStatusCode.BadRequest), null, new RefitSettings());
            _mockDirectPayService.Setup(x => x.GetTransactions(It.IsAny<PaymentRequestQueryOptions>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(errorResponse);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _directPayService.GetTransactions(options));
            _mockDirectPayService.Verify(x => x.GetTransactions(options, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

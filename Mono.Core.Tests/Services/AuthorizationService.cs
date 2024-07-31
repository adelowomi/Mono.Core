using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Refit;
using Xunit;
using Mono.Core;
using Mono.Core.Authorization;
using Mono.Core.Accounts;
using System.Net.Http.Headers;

namespace Mono.Core.Tests
{
    public class AuthorizationServiceTests
    {
        private readonly Mock<IAuthorizationService> _mockAuthorizationService;
        private readonly AuthorizationService _authorizationService;

        public AuthorizationServiceTests()
        {
            _mockAuthorizationService = new Mock<IAuthorizationService>();
            var refitClientBuilder = new Mock<IRefitClientBuilder<IAuthorizationService>>();
            refitClientBuilder.Setup(x => x.Build()).Returns(_mockAuthorizationService.Object);
            _authorizationService = new AuthorizationService(refitClientBuilder.Object);
        }

        [Fact]
        public async Task InitiateAccountLinking_ShouldReturnExpectedResponse()
        {
            // Arrange
            var accountLinkingModel = new AccountLinkingModel();
            var expectedResponse = new MonoStandardResponse<AccountLinkingResponseModel>();
            _mockAuthorizationService.Setup(x => x.InitiateAccountLinking(accountLinkingModel, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(new ApiResponse<MonoStandardResponse<AccountLinkingResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, null));

            // Act
            var result = await _authorizationService.InitiateAccountLinking(accountLinkingModel);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockAuthorizationService.Verify(x => x.InitiateAccountLinking(accountLinkingModel, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task InitiateAccountLinking_ShouldHandleErrorResponse()
        {
            // Arrange
            var accountLinkingModel = new AccountLinkingModel();
           
            _mockAuthorizationService.Setup(x => x.InitiateAccountLinking(accountLinkingModel, It.IsAny<CancellationToken>()))
                                     .ThrowsAsync(null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authorizationService.InitiateAccountLinking(accountLinkingModel));
        }

        [Fact]
        public async Task AuthorizeAccount_ShouldReturnExpectedResponse()
        {
            // Arrange
            var authorizationAccountRequestModel = new AuthorizationAccountRequestModel { Code = "test-code" };
            var expectedResponse = new MonoStandardResponse<AuthorizationAccountResponseModel>();
            _mockAuthorizationService.Setup(x => x.AuthorizeAccount(authorizationAccountRequestModel, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(new ApiResponse<MonoStandardResponse<AuthorizationAccountResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, null));

            // Act
            var result = await _authorizationService.AuthorizeAccount(authorizationAccountRequestModel);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockAuthorizationService.Verify(x => x.AuthorizeAccount(authorizationAccountRequestModel, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AuthorizeAccount_ShouldHandleErrorResponse()
        {
            // Arrange
            var authorizationAccountRequestModel = new AuthorizationAccountRequestModel { Code = "test-code" };
            
            _mockAuthorizationService.Setup(x => x.AuthorizeAccount(authorizationAccountRequestModel, It.IsAny<CancellationToken>()))
                                     .ThrowsAsync(null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authorizationService.AuthorizeAccount(authorizationAccountRequestModel));
        }

        [Fact]
        public async Task SyncAccount_ShouldReturnExpectedResponse()
        {
            // Arrange
            var accountId = "test-account-id";
            var expectedResponse = new MonoStandardResponse<ManualDataSyncResponseModel>();
            _mockAuthorizationService.Setup(x => x.SyncAccount(accountId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(new ApiResponse<MonoStandardResponse<ManualDataSyncResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, null));

            // Act
            var result = await _authorizationService.SyncAccount(accountId);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockAuthorizationService.Verify(x => x.SyncAccount(accountId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task SyncAccount_ShouldHandleErrorResponse()
        {
            // Arrange
            var accountId = "test-account-id";
            _mockAuthorizationService.Setup(x => x.SyncAccount(accountId, It.IsAny<CancellationToken>()))
                                     .ThrowsAsync(null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authorizationService.SyncAccount(accountId));
        }

        [Fact]
        public async Task ReauthorizeAccount_ShouldReturnExpectedResponse()
        {
            // Arrange
            var accountId = "test-account-id";
            var expectedResponse = new MonoStandardResponse<ReAuthorizationCodeResponseModel>();
            _mockAuthorizationService.Setup(x => x.ReauthorizeAccount(accountId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(new ApiResponse<MonoStandardResponse<ReAuthorizationCodeResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, null));

            // Act
            var result = await _authorizationService.ReauthorizeAccount(accountId);

            // Assert
            Assert.Equal(expectedResponse, result);
            _mockAuthorizationService.Verify(x => x.ReauthorizeAccount(accountId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ReauthorizeAccount_ShouldHandleErrorResponse()
        {
            // Arrange
            var accountId = "test-account-id";
            
            _mockAuthorizationService.Setup(x => x.ReauthorizeAccount(accountId, It.IsAny<CancellationToken>()))
                                     .ThrowsAsync(null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _authorizationService.ReauthorizeAccount(accountId));
        }
    }
}

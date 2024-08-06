using Mono.Core.DirectPay;
using Moq;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Mono.Core.LookUp.Tests
{
    public class LookUpServiceTests
    {
        private readonly Mock<ILookUpService> _mockLookUpService;
        private readonly Mock<ILookUpService> _mockLookUpServiceV3;
        private readonly LookUpService _lookUpService;

        public LookUpServiceTests()
        {
            _mockLookUpService = new Mock<ILookUpService>();
            _mockLookUpServiceV3 = new Mock<ILookUpService>();
            var mockBuilder = new Mock<IRefitClientBuilder<ILookUpService>>();
            mockBuilder.Setup(x => x.Build()).Returns(_mockLookUpService.Object);
            mockBuilder.Setup(x => x.BuildV3()).Returns(_mockLookUpServiceV3.Object);

            _lookUpService = new LookUpService(mockBuilder.Object);
        }

        private MonoStandardResponse<T> CreateResponse<T>(T data, bool success = true, string message = "Success")
        {
            return new MonoStandardResponse<T>
            {
                Data = data,
                Success = success,
                Message = message
            };
        }

        // InitiateBvnLookUp Tests
        [Fact]
        public async Task InitiateBvnLookUp_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new InitiateBvnLookUpModel();
            var expectedResponse = CreateResponse(new InitiateBvnLookUpResponseModel());

            _mockLookUpService.Setup(x => x.InitiateBvnLookUp(requestModel, It.IsAny<CancellationToken>()))
               .ReturnsAsync(new ApiResponse<MonoStandardResponse<InitiateBvnLookUpResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.InitiateBvnLookUp(requestModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task InitiateBvnLookUp_ShouldHandleNullResponse()
        {
            // Arrange
            var requestModel = new InitiateBvnLookUpModel();
           
            _mockLookUpService.Setup(x => x.InitiateBvnLookUp(requestModel, It.IsAny<CancellationToken>()))
                .ThrowsAsync(null);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _lookUpService.InitiateBvnLookUp(requestModel, It.IsAny<CancellationToken>()));

        }

        [Fact]
        public async Task InitiateBvnLookUp_ShouldHandleException()
        {
            // Arrange
            var requestModel = new InitiateBvnLookUpModel();
            _mockLookUpService.Setup(x => x.InitiateBvnLookUp(requestModel, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _lookUpService.InitiateBvnLookUp(requestModel));
        }

        // VerifyBvnLookUp Tests
        [Fact]
        public async Task VerifyBvnLookUp_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new VerifyBvnLookUpOtpModel();
            var sessionId = requestModel.SessionId;
            var expectedResponse = CreateResponse(true);

            _mockLookUpService.Setup(x => x.VerifyBvnLookUp(requestModel, sessionId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<MonoStandardResponse<bool>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));
               
            // Act
            var result = await _lookUpService.VerifyBvnLookUp(requestModel, sessionId);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task VerifyBvnLookUp_ShouldHandleException()
        {
            // Arrange
            var requestModel = new VerifyBvnLookUpOtpModel();
            var sessionId = "sessionId";
            _mockLookUpService.Setup(x => x.VerifyBvnLookUp(requestModel, sessionId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _lookUpService.VerifyBvnLookUp(requestModel, sessionId));
        }

        // GetBvnDetails Tests
        [Fact]
        public async Task GetBvnDetails_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new BvnDetailsModel();
            var expectedResponse = CreateResponse(new BvnDetailsResponse());

            _mockLookUpService.Setup(x => x.GetBvnDetails(requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<BvnDetailsResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetBvnDetails(requestModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetBvnDetails_ShouldHandleException()
        {
            // Arrange
            var requestModel = new BvnDetailsModel();
            _mockLookUpService.Setup(x => x.GetBvnDetails(requestModel, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _lookUpService.GetBvnDetails(requestModel));
        }

        // GetCacLookUp Tests
        [Fact]
        public async Task GetCacLookUp_ShouldReturnSuccessResponse()
        {
            // Arrange
            var search = "searchTerm";
            var expectedResponse = CreateResponse(new List<BusinessDetails>());

            _mockLookUpServiceV3.Setup(x => x.GetCacLookUp(search, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<List<BusinessDetails>>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetCacLookUp(search);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetCacLookUp_ShouldHandleException()
        {
            // Arrange
            var search = "searchTerm";
            _mockLookUpServiceV3.Setup(x => x.GetCacLookUp(search, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _lookUpService.GetCacLookUp(search));
        }

        // GetCacCompany Tests
        [Fact]
        public async Task GetCacCompany_ShouldReturnSuccessResponse()
        {
            // Arrange
            var businessId = "businessId";
            var expectedResponse = CreateResponse(new List<OfficialDetails>());

            _mockLookUpServiceV3.Setup(x => x.GetCacCompany(businessId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<List<OfficialDetails>>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetCacCompany(businessId);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetCacCompany_ShouldHandleException()
        {
            // Arrange
            var businessId = "businessId";
            _mockLookUpServiceV3.Setup(x => x.GetCacCompany(businessId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _lookUpService.GetCacCompany(businessId));
        }

        [Fact]
        public async Task GetPreviousAddress_ShouldReturnSuccessResponse()
        {
            // Arrange
            var businessId = "businessId";
            var expectedResponse = CreateResponse(new PreviousAddressResponse());

            _mockLookUpServiceV3.Setup(x => x.GetPreviousAddress(businessId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponse<MonoStandardResponse<PreviousAddressResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetPreviousAddress(businessId);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetPreviousAddress_ShouldHandleException()
        {
            // Arrange
            var businessId = "businessId";
            _mockLookUpServiceV3.Setup(x => x.GetPreviousAddress(businessId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _lookUpService.GetPreviousAddress(businessId));
        }

        [Fact]
        public async Task GetChangeOfName_ShouldReturnSuccessResponse()
        {
            // Arrange
            var businessId = "businessId";
            var expectedResponse = CreateResponse(new ChangeOfNameResponse());

            _mockLookUpServiceV3.Setup(x => x.GetChangeOfName(businessId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ApiResponse<MonoStandardResponse<ChangeOfNameResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings())); ;

            // Act
            var result = await _lookUpService.GetChangeOfName(businessId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetSecretary_ShouldReturnSuccessResponse()
        {
            // Arrange
            var businessId = "businessId";
            var expectedResponse = CreateResponse(new List<OfficialDetails>());

            _mockLookUpServiceV3.Setup(x => x.GetSecretary(businessId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<List<OfficialDetails>>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetSecretary(businessId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetDirectors_ShouldReturnSuccessResponse()
        {
            // Arrange
            var businessId = "businessId";
            var expectedResponse = CreateResponse(new List<OfficialDetails>());

            _mockLookUpServiceV3.Setup(x => x.GetDirectors(businessId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<List<OfficialDetails>>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetDirectors(businessId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetBanks_ShouldReturnSuccessResponse()
        {
            // Arrange
            var expectedResponse = CreateResponse(new BanksResponse());

            _mockLookUpServiceV3.Setup(x => x.GetBanks(It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<BanksResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetBanks();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetAddress_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new AddressLookUpRequestModel();
            var expectedResponse = CreateResponse(new AddressLookUpResponseModel());

            _mockLookUpServiceV3.Setup(x => x.GetAddress(requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<AddressLookUpResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetAddress(requestModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetPassport_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new InternationalPassportRequestModel();
            var expectedResponse = CreateResponse(new InternationalPassportResponse());

            _mockLookUpServiceV3.Setup(x => x.GetPassport(requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<InternationalPassportResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetPassport(requestModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetTin_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new TinRequestModel();
            var expectedResponse = CreateResponse(new TinResponseModel());

            _mockLookUpServiceV3.Setup(x => x.GetTin(requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<TinResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetTin(requestModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetNin_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new NinRequestModel();
            var expectedResponse = CreateResponse(new NinResponseModel());

            _mockLookUpServiceV3.Setup(x => x.GetNin(requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<NinResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetNin(requestModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetDriverLicense_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new DriversLicenseRequestModel();
            var expectedResponse = CreateResponse(new DriversLicenseResponse());

            _mockLookUpServiceV3.Setup(x => x.GetDriverLicense(requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<DriversLicenseResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetDriverLicense(requestModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetAccountNumber_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new AccountRequestModel();
            var expectedResponse = CreateResponse(new AccountResponse());

            _mockLookUpServiceV3.Setup(x => x.GetAccountNumber(requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<AccountResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetAccountNumber(requestModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetCreditHistory_ShouldReturnSuccessResponse()
        {
            // Arrange
            var provider = "provider";
            var requestModel = new CreditHistoryRequestModel();
            var expectedResponse = CreateResponse(new CreditHistoryResponse());

            _mockLookUpServiceV3.Setup(x => x.GetCreditHistory(provider, requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<CreditHistoryResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetCreditHistory(provider, requestModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

        [Fact]
        public async Task GetMashUp_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestModel = new MashUpRequestModel();
            var expectedResponse = CreateResponse(new MashUpResponse());

            _mockLookUpServiceV3.Setup(x => x.GetMashUp(requestModel, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(new ApiResponse<MonoStandardResponse<MashUpResponse>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

            // Act
            var result = await _lookUpService.GetMashUp(requestModel);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(expectedResponse.Data, result.Data);
        }

    }
}

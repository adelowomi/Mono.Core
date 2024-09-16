using System.Threading;
using System.Threading.Tasks;
using Moq;
using Refit;
using Xunit;
using Mono.Core;
using Mono.Core.Miscellaneous;
using System.Net;
using Mono.Core.LookUp;

namespace Mono.Core.Miscellaneous.Tests;

public class MiscellaneousServiceTests
{
    private readonly Mock<IMiscellaneousService> _mockMiscellaneousService;
    private readonly MiscellaneousService _service;

    public MiscellaneousServiceTests()
    {
        _mockMiscellaneousService = new Mock<IMiscellaneousService>();
        var mockBuilder = new Mock<IRefitClientBuilder<IMiscellaneousService>>();
        mockBuilder.Setup(m => m.Build("")).Returns(_mockMiscellaneousService.Object);

        _service = new MiscellaneousService(mockBuilder.Object);
    }

    [Fact]
    public async Task GetCoverage_ShouldReturnSuccessResponse()
    {
        // Arrange
        var expectedResponse = new MonoStandardResponse<InstitutionResponseModel>
        {
            Data = new InstitutionResponseModel { Name = "Test Institution" }
        };
        var apiResponse = new ApiResponse<MonoStandardResponse<InstitutionResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings());
        _mockMiscellaneousService.Setup(s => s.GetCoverage(It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        var result = await _service.GetCoverage();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Data.Name, result.Data.Name);
    }

    [Fact]
    public async Task GetCacLookup_ShouldReturnSuccessResponse()
    {
        // Arrange
        var expectedResponse = new MonoStandardResponse<BusinessLookUpResponseModel>
        {
            Data = new BusinessLookUpResponseModel { Id = "123" }
        };
        var apiResponse = new ApiResponse<MonoStandardResponse<BusinessLookUpResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings());
        _mockMiscellaneousService.Setup(s => s.GetCacLookup(It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        var result = await _service.GetCacLookup();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Data.Id, result.Data.Id);
    }

    [Fact]
    public async Task GetCacCompany_ShouldReturnSuccessResponse()
    {
        // Arrange
        var businessId = "12345";
        var expectedResponse = new MonoStandardResponse<ShareHolderResponseModel>
        {
            Data = new ShareHolderResponseModel { Id = 1 }
        };
        var apiResponse = new ApiResponse<MonoStandardResponse<ShareHolderResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings());
        _mockMiscellaneousService.Setup(s => s.GetCacCompany(businessId, It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        var result = await _service.GetCacCompany(businessId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Data.Id, result.Data.Id);
    }

    [Fact]
    public async Task UnlinkAccount_ShouldReturnSuccessResponse()
    {
        // Arrange
        var accountId = "12345";
        var expectedResponse = new MonoStandardResponse<string>
        {
            Data = "Unlinked"
        };
        var apiResponse = new ApiResponse<MonoStandardResponse<string>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings());
        _mockMiscellaneousService.Setup(s => s.UnlinkAccount(accountId, It.IsAny<CancellationToken>())).ReturnsAsync(apiResponse);

        // Act
        var result = await _service.UnlinkAccount(accountId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResponse.Data, result.Data);
    }

    [Fact]
    public async Task GetCoverage_ShouldHandleCancellation()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        cts.Cancel();
        _mockMiscellaneousService.Setup(s => s.GetCoverage(It.IsAny<CancellationToken>())).ThrowsAsync(new TaskCanceledException());

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(() => _service.GetCoverage(cts.Token));
    }
}

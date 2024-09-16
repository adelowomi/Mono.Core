using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Mono.Core;
using Mono.Core.Accounts;
using Mono.Core.LookUp;
using Moq;
using Refit;
using Xunit;
using Account = Mono.Core.Accounts.Account;
using Institution = Mono.Core.Accounts.Institution;

namespace Mono.AccountServiceTests;

public class AccountServiceTests
{
    private readonly Mock<IAccountService> _mockAccountService;
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _mockAccountService = new Mock<IAccountService>();
        var refitClientBuilderMock = new Mock<IRefitClientBuilder<IAccountService>>();
        refitClientBuilderMock.Setup(x => x.Build("")).Returns(_mockAccountService.Object);
        _accountService = new AccountService(refitClientBuilderMock.Object);
    }

    [Fact]
    public async Task InitiateAccountLinking_ShouldReturnSuccessResponse()
    {
        // Arrange
        var accountLinkingModel = new AccountLinkingModel
        {
            Customer = new Customer { Name = "John Doe", Email = "john@example.com" },
            Meta = new Meta { Ref = "ref123" },
            Scope = "scope",
            RedirectUrl = "https://example.com"
        };

        var expectedResponse = new MonoStandardResponse<AccountLinkingResponseModel>
        {
            Data = new AccountLinkingResponseModel
            {
                MonoUrl = "https://mono.com/link",
                Customer = "John Doe",
                Meta = new Meta { Ref = "ref123" },
                Scope = "scope",
                RedirectUrl = "https://example.com",
                CreatedAt = DateTime.UtcNow
            },
            Status = "success"
        };

        _mockAccountService.Setup(x => x.InitiateAccountLinking(It.IsAny<AccountLinkingModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<MonoStandardResponse<AccountLinkingResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

        // Act
        var response = await _accountService.InitiateAccountLinking(accountLinkingModel);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal(expectedResponse.Data.MonoUrl, response.Data.MonoUrl);
    }

    [Fact]
    public async Task GetAccount_ShouldReturnAccountInformation()
    {
        // Arrange
        var accountId = "12345";
        var expectedResponse = new MonoStandardResponse<InformationResponseModel>
        {
            Data = new InformationResponseModel
            {
                Account = new Account
                {
                    Id = "12345",
                    Name = "John Doe",
                    AccountNumber = "0001234567",
                    Balance = 1000,
                    Institution = new Institution
                    {
                        Name = "Good Bank",
                        BankCode = "11223344",
                        Type = "Commercial"
                    },
                    Type = "Savings",
                    Currency = "NGN",
                    Bvn = "000000000222" 
                },
                Meta = new Meta { Ref = "ref123" }
            },
            Status = "success"
        };

        _mockAccountService.Setup(x => x.GetAccount(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<MonoStandardResponse<InformationResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

        // Act
        var response = await _accountService.GetAccount(accountId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal(expectedResponse.Data.Account.Id, response.Data.Account.Id);
    }

    [Fact]
    public async Task GetPollStatementPdf_ShouldReturnPdfResponse()
    {
        // Arrange
        var accountId = "12345";
        var jobId = "job123";
        var expectedResponse = new MonoStandardResponse<StatementPdfResponseModel>
        {
            Data = new StatementPdfResponseModel
            {
                Total = 5,
                Page = 1,
                Previous = null,
                Next = null
            },
            Status = "success"
        };

        _mockAccountService.Setup(x => x.GetPollStatementPdf(accountId, jobId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<MonoStandardResponse<StatementPdfResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

        // Act
        var response = await _accountService.GetPollStatementPdf(accountId, jobId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal(expectedResponse.Data.Total, response.Data.Total);
    }

    [Fact]
    public async Task GetIncome_ShouldReturnIncomeResponse()
    {
        // Arrange
        var accountId = "12345";
        var expectedResponse = new MonoStandardResponse<IncomeResponseModel>
        {
            Data = new IncomeResponseModel
            {
                Type = "salary",
                Amount = 5000,
                Employer = "Company XYZ",
                Confidence = 0.95
            },
            Status = "success"
        };

        _mockAccountService.Setup(x => x.GetIncome(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<MonoStandardResponse<IncomeResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

        // Act
        var response = await _accountService.GetIncome(accountId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal(expectedResponse.Data.Amount, response.Data.Amount);
    }

    [Fact]
    public async Task GetIdentity_ShouldReturnIdentityResponse()
    {
        // Arrange
        var accountId = "12345";
        var expectedResponse = new MonoStandardResponse<IdentityResponseModel>
        {
            Data = new IdentityResponseModel
            {
                FullName = "John Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                Gender = "male"
            },
            Status = "success"
        };

        _mockAccountService.Setup(x => x.GetIdentity(accountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<MonoStandardResponse<IdentityResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

        // Act
        var response = await _accountService.GetIdentity(accountId);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal(expectedResponse.Data.FullName, response.Data.FullName);
    }

    [Fact]
    public async Task GetStatement_ShouldReturnStatementResponse()
    {
        // Arrange
        var accountId = "12345";
        var statementRequestModels = new StatementRequestModels { Period = "1m", Output = "json" };
        var expectedResponse = new MonoStandardResponse<StatementResponseModel>
        {
            Data = new StatementResponseModel
            {
                Meta = new ResponseMeta { Count = 1 },
                StatementList = new List<Statement>
                {
                    new Statement
                    {
                        Id = "stmt123",
                        Type = "credit",
                        Amount = 1000,
                        Narration = "Salary",
                        Date = DateTimeOffset.UtcNow,
                        Balance = 2000
                    }
                }
            },
            Status = "success"
        };

        _mockAccountService.Setup(x => x.GetStatement(accountId, statementRequestModels, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<MonoStandardResponse<StatementResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

        // Act
        var response = await _accountService.GetStatement(accountId, statementRequestModels);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal(expectedResponse.Data.StatementList.Count, response.Data.StatementList.Count);
    }

    [Fact]
    public async Task GetTransactions_ShouldReturnTransactionResponse()
    {
        // Arrange
        var accountId = "12345";
        var transactionsRequest = new AccountTransactionsOptionsRequest
        {
            Start = "2023-01-01",
            End = "2023-12-31",
            Narration = "Salary",
            Type = "credit",
            Paginate = true,
            Limit = 10
        };

        var expectedResponse = new MonoStandardResponse<TransactionResponseModel>
        {
            Data = new TransactionResponseModel
            {
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        Id = "txn123",
                        Type = "credit",
                        Amount = 1000,
                        Narration = "Salary",
                        Date = DateTimeOffset.UtcNow,
                        Balance = 2000
                    }
                },
                Paging = new MonoStandardPaginatedResponse { }
            },
            Status = "success"
        };

        _mockAccountService.Setup(x => x.GetTransactions(accountId, transactionsRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ApiResponse<MonoStandardResponse<TransactionResponseModel>>(new HttpResponseMessage(HttpStatusCode.OK), expectedResponse, new RefitSettings()));

        // Act
        var response = await _accountService.GetTransactions(accountId, transactionsRequest);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("success", response.Status);
        Assert.Equal(expectedResponse.Data.Transactions.Count, response.Data.Transactions.Count);
    }
}

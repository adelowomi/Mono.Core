using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Mono.Core.Accounts;
using Mono.Core.DirectPay;
using Mono.Core.LookUp;
using Mono.Core.Services.DirectPay.Models;
using Refit;
using Xunit;
using MonoMeta = Mono.Core.Accounts.Meta;

namespace Mono.Core.Tests;

public class AccountServiceTests
{
    public IAccountService _accountService;
    private readonly AccountService _monoAccounts;

    private ILookUpService _lookupService;
    private readonly LookUpService _lookupServiceImplementation;
    private readonly IDirectPayService _directPayService;

    private readonly DirectPayService _directPayServiceImplementation;

    public AccountServiceTests()
    {

        var builder = new RefitClientBuilder<IAccountService>(new OptionsWrapper<MonoInitializationOptions>(new MonoInitializationOptions
        {
            BaseUrl = "https://api.withmono.com/v2",
            SecretKey = "test_sk_kc1d3k7kclzhij7mh7t8"
        }));

        _accountService = builder.Build();
        
        var lookupBuilder = new RefitClientBuilder<ILookUpService>(new OptionsWrapper<MonoInitializationOptions>(new MonoInitializationOptions
        {
            BaseUrl = "https://api.withmono.com/v2",
            SecretKey = "test_sk_kc1d3k7kclzhij7mh7t8"
        }));

        _lookupService = lookupBuilder.Build();

        _monoAccounts = new AccountService(builder);

        var directPayBuilder = new RefitClientBuilder<IDirectPayService>(new OptionsWrapper<MonoInitializationOptions>(new MonoInitializationOptions
        {
            BaseUrl = "https://api.withmono.com/v2",
            SecretKey = "test_sk_kc1d3k7kclzhij7mh7t8"
        }));

        _directPayService = directPayBuilder.BuildV3();
        _directPayServiceImplementation = new DirectPayService(directPayBuilder, directPayBuilder);
    }

    [Fact]
    public async Task InitiateAccountLinking_Should_Return_AccountLinkingResponseModel()
    {
        try
        {
            // Arrange
            var accountLinkingModel = new AccountLinkingModel
            {
                Customer = new Accounts.Customer
                {
                    Name = "Keneeee",
                    Email = "Keneeee@test.com"
                },
                Meta = new MonoMeta
                {
                    Ref = "onfvosfnvoinfvoinfovinsf"
                },
                Scope = "auth",
                RedirectUrl = "https://localhost:3000"
            };
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _monoAccounts.InitiateAccountLinking(accountLinkingModel, cancellationToken);

            // Assert
            Assert.NotNull(result);
        }
        catch (System.Exception ex)
        {

            throw;
        }
    }

    [Fact]
    public async Task GetAccount_Should_Return_Account()
    {

        // Arrange
        var accountId = "669e6f47304f23a8e26edbd1";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _monoAccounts.GetAccount(accountId, cancellationToken);

        // Assert
        Assert.Equal(accountId, result.Data.Account.Id);
    }

    [Fact]
    public async Task GetAccount_Wit_Invalid_Account_Should_Not_Return_Account()
    {

        // Arrange
        var accountId = "669e6f47304f23a8e26edbd";
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _monoAccounts.GetAccount(accountId, cancellationToken);

        // Assert
        Assert.Null(result.Data);
        Assert.False(result.Success);

    }

    // test initiate bvn verification
    [Fact]
    public async Task InitiateBvnVerification_Should_Return_BvnVerificationResponseModel()
    {
        var model = new InitiateBvnLookUpModel
        {
            Bvn = "22183865959",
            Scope = ScopeConstants.Identity.ToLower()
        };

        var cancellationToken = CancellationToken.None;

        var result = await _lookupService.InitiateBvnLookUp(model, cancellationToken);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task VerifyBvnVerification_Should_Return_True()
    {
        var model = new VerifyBvnLookUpOtpModel
        {
            Method = "alternate_phone",
            PhoneNumber = "09066662020"
        };

        var sessionId = "T5mvVrMDApML0H20gNfj";
        var cancellationToken = CancellationToken.None;

        var result = await _lookupService.VerifyBvnLookUp(model, sessionId, cancellationToken);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetBvnDetails_Should_Return_BvnDetailsResponse()
    {
        var model = new BvnDetailsModel
        {
            Otp = "123456"
        };

        var sessionId = "T5mvVrMDApML0H20gNfj";
        var cancellationToken = CancellationToken.None;

        var result = await _lookupService.GetBvnDetails(model, sessionId, cancellationToken);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateMandateShouldReturnError()
    {
        var model = new CreateMandateModel
        {
            Customer = "66db05d644951e4ef76a465d",
            AccountNumber = "0131883462",
            BankCode = "000018",
            Amount = 10000000,
            DebitType = "variable",
            Description = "Chit Wallet Management",
            MandateType = "emandate",
            Reference = "CHMREFTD6X7LSGHVYFW9KOL",
            EndDate = "2025-09-16",
            StartDate = DateTime.Now.ToString("yyyy-MM-dd"),
        };

        var cancellationToken = CancellationToken.None;

        var result = await _directPayServiceImplementation.CreateMandate(model, cancellationToken);

        Assert.NotNull(result);
    }

}

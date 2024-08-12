using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Mono.Core.Accounts;
using Mono.Core.LookUp;
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
    }

    [Fact]
    public async Task InitiateAccountLinking_Should_Return_AccountLinkingResponseModel()
    {
        try
        {
            // Arrange
            var accountLinkingModel = new AccountLinkingModel
            {
                Customer = new Customer
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

}

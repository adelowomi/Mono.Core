using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Mono.Core.Accounts;
using Refit;
using Xunit;
using MonoMeta = Mono.Core.Accounts.Meta;

namespace Mono.Core.Tests;

public class AccountServiceTests
{
    public IAccountService _accountService;
    private readonly AccountService _monoAccounts;

    public AccountServiceTests()
    {

        var builder = new RefitClientBuilder<IAccountService>(new OptionsWrapper<MonoInitializationOptions>(new MonoInitializationOptions
        {
            BaseUrl = "",
            SecretKey = ""
        }));

        _accountService = builder.Build();

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
                    Name = "Kene",
                    Email = "Kene@test.com"
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




}

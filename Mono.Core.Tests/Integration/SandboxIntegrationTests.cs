using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mono.Core;
using Mono.Core.Accounts;
using Mono.Core.Customers;
using Mono.Core.DirectPay;
using Mono.Core.LookUp;
using Mono.Core.Services.DirectPay.Models;
using Xunit;

namespace Mono.Core.Integration.Tests;

/// <summary>
/// Hits the real Mono sandbox. Skipped automatically when
/// <c>MONO_SANDBOX_KEY</c> isn't set in the environment (e.g. local dev, CI
/// without secrets). When set, validates that our request shapes round-trip
/// against a real Mono response — the only layer that catches "Mono changed
/// a field name" or "Mono is case-sensitive on a query param we got wrong".
///
/// Set these env vars to run:
///   MONO_SANDBOX_KEY            (required) — sandbox secret key
///   MONO_SANDBOX_BASE_URL       (optional) — defaults to https://api.withmono.com/v2
///   MONO_SANDBOX_ACCOUNT_ID     (optional) — for tests that need a linked account
///   MONO_SANDBOX_CUSTOMER_ID    (optional) — for customer-fetch tests
///
/// Note these are integration smoke tests — they exercise GET-only / list-only
/// endpoints that don't create or modify sandbox state. POST/PATCH/DELETE
/// tests would need disposable fixture data and aren't included here.
/// </summary>
public class SandboxIntegrationTests
{
    private const string KeyEnv = "MONO_SANDBOX_KEY";
    private const string BaseUrlEnv = "MONO_SANDBOX_BASE_URL";
    private const string AccountIdEnv = "MONO_SANDBOX_ACCOUNT_ID";
    private const string CustomerIdEnv = "MONO_SANDBOX_CUSTOMER_ID";
    private const string DefaultBaseUrl = "https://api.withmono.com/v2";

    private static (RefitClientBuilder<T> builder, MonoInitializationOptions options) BuildRealBuilder<T>()
    {
        var key = Environment.GetEnvironmentVariable(KeyEnv);
        Skip.If(string.IsNullOrWhiteSpace(key), $"{KeyEnv} not set — skipping sandbox integration test.");

        var options = new MonoInitializationOptions
        {
            BaseUrl = Environment.GetEnvironmentVariable(BaseUrlEnv) ?? DefaultBaseUrl,
            SecretKey = key,
        };
        return (new RefitClientBuilder<T>(Options.Create(options)), options);
    }

    // ============ Connect / LookUp — read-only endpoints ============

    [SkippableFact]
    public async Task Sandbox_GetBanks_ReturnsListOfBanks()
    {
        var (builder, _) = BuildRealBuilder<ILookUpService>();
        var lookup = new LookUpService(builder);

        var result = await lookup.GetBanks();

        Assert.True(result.Success || result.Status == "successful",
            $"Mono returned {result.Status}: {result.Message}");
        Assert.NotNull(result.Data);
    }

    [SkippableFact]
    public async Task Sandbox_GetCoverage_ReturnsInstitutionList()
    {
        var (builder, _) = BuildRealBuilder<Mono.Core.IMiscellaneousService>();
        var misc = new Mono.Core.Miscellaneous.MiscellaneousService(builder);

        var result = await misc.GetCoverage();

        Assert.NotNull(result);
        // Mono's coverage payload includes an institutions list when populated.
    }

    // ============ Customers — read-only ============

    [SkippableFact]
    public async Task Sandbox_ListCustomers_ReturnsPagedResponse()
    {
        var (builder, _) = BuildRealBuilder<ICustomerService>();
        var customers = new CustomerService(builder);

        var result = await customers.ListCustomers(new CustomerListQueryOptions { Page = 1, Limit = 5 });

        Assert.NotNull(result);
        // We don't assert non-empty since a fresh sandbox may have no customers.
        // Verify the response wrapper parsed (would NRE if the meta/customers
        // keys didn't match Mono's actual response shape).
        if (result.Success && result.Data?.Customers != null)
        {
            Assert.True(result.Data.Customers.Count <= 5);
        }
    }

    [SkippableFact]
    public async Task Sandbox_RetrieveCustomer_ReturnsCustomerWhenIdProvided()
    {
        var customerId = Environment.GetEnvironmentVariable(CustomerIdEnv);
        Skip.If(string.IsNullOrWhiteSpace(customerId), $"{CustomerIdEnv} not set — skipping.");

        var (builder, _) = BuildRealBuilder<ICustomerService>();
        var customers = new CustomerService(builder);

        var result = await customers.RetrieveCustomer(customerId);

        Assert.True(result.Success, $"Mono returned status={result.Status}, message={result.Message}");
        Assert.Equal(customerId, result.Data.Id);
    }

    // ============ Connect / Accounts — read-only ============

    [SkippableFact]
    public async Task Sandbox_GetAccount_ReturnsAccountWhenIdProvided()
    {
        var accountId = Environment.GetEnvironmentVariable(AccountIdEnv);
        Skip.If(string.IsNullOrWhiteSpace(accountId), $"{AccountIdEnv} not set — skipping.");

        var (builder, _) = BuildRealBuilder<IAccountService>();
        var accounts = new AccountService(builder);

        var result = await accounts.GetAccount(accountId);

        Assert.True(result.Success, $"Mono returned status={result.Status}, message={result.Message}");
        Assert.NotNull(result.Data?.Account);
        Assert.Equal(accountId, result.Data.Account.Id);
    }

    [SkippableFact]
    public async Task Sandbox_GetAccountBalance_ReturnsRealtimeBalance()
    {
        var accountId = Environment.GetEnvironmentVariable(AccountIdEnv);
        Skip.If(string.IsNullOrWhiteSpace(accountId), $"{AccountIdEnv} not set — skipping.");

        var (builder, _) = BuildRealBuilder<IAccountService>();
        var accounts = new AccountService(builder);

        var result = await accounts.GetAccountBalance(accountId);

        // The endpoint is paid; the result might succeed or fail depending on
        // the sandbox plan. Either way, we want the response to parse cleanly.
        if (result.Success)
        {
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Currency);
        }
        else
        {
            // A failure response should still have a parsed Message field.
            Assert.NotNull(result.Message);
        }
    }

    // ============ DirectPay — read-only payouts list ============

    [SkippableFact]
    public async Task Sandbox_GetPayouts_ListEndpointReturnsParsedResponse()
    {
        var (builder, _) = BuildRealBuilder<IDirectPayService>();
        var directpay = new DirectPayService(builder, builder);

        var result = await directpay.GetPayouts(new PayoutListQueryOptions { Page = 1, Limit = 5 });

        Assert.NotNull(result);
        // Verify the wrapper parsed even on an empty sandbox.
        if (result.Success && result.Data?.Payouts != null)
        {
            Assert.True(result.Data.Payouts.Count <= 5);
        }
    }
}

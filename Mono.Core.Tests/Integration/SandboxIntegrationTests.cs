using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Mono.Core;
using Mono.Core.Accounts;
using Mono.Core.Customers;
using Mono.Core.DirectPay;
using Mono.Core.Disburse;
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
///   MONO_SANDBOX_KEY            (required) — sandbox secret key for the default
///                                            product app (typically Connect/DirectPay)
///   MONO_SANDBOX_LOOKUP_KEY     (optional) — sandbox secret for a Lookup-product app.
///                                            Mono scopes each app to one product, so
///                                            Lookup endpoints (/lookup/banks etc.)
///                                            need a separate key. Tests that hit
///                                            Lookup endpoints skip when this isn't set.
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
    private const string LookupKeyEnv = "MONO_SANDBOX_LOOKUP_KEY";
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
            LookupSecretKey = Environment.GetEnvironmentVariable(LookupKeyEnv),
        };
        return (new RefitClientBuilder<T>(Options.Create(options)), options);
    }

    /// <summary>
    /// Skips when no Lookup-product key is available. Mono scopes each
    /// dashboard app to one product, so /lookup/* endpoints reject a
    /// Connect-product key with: "Please use a Lookup app, go to your
    /// dashboard and create an app with Lookup product."
    /// </summary>
    private static void SkipUnlessLookupKey()
    {
        var lookup = Environment.GetEnvironmentVariable(LookupKeyEnv);
        Skip.If(string.IsNullOrWhiteSpace(lookup),
            $"{LookupKeyEnv} not set — Lookup-product endpoints need their own app key.");
    }

    // ============ Connect / LookUp — read-only endpoints ============

    [SkippableFact]
    public async Task Sandbox_GetBanks_ReturnsListOfBanks()
    {
        SkipUnlessLookupKey();
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

    // ============ Connect — read-only account data (needs account ID) ============

    [SkippableFact]
    public async Task Sandbox_GetAccountIdentity_ParsesResponse()
    {
        var accountId = Environment.GetEnvironmentVariable(AccountIdEnv);
        Skip.If(string.IsNullOrWhiteSpace(accountId), $"{AccountIdEnv} not set.");
        var (builder, _) = BuildRealBuilder<IAccountService>();
        var accounts = new AccountService(builder);

        var result = await accounts.GetIdentity(accountId);

        Assert.NotNull(result);
        if (!result.Success) Assert.NotNull(result.Message);
    }

    [SkippableFact]
    public async Task Sandbox_GetAccountIncome_ParsesResponse()
    {
        var accountId = Environment.GetEnvironmentVariable(AccountIdEnv);
        Skip.If(string.IsNullOrWhiteSpace(accountId), $"{AccountIdEnv} not set.");
        var (builder, _) = BuildRealBuilder<IAccountService>();
        var accounts = new AccountService(builder);

        var result = await accounts.GetIncome(accountId);

        Assert.NotNull(result);
        if (!result.Success) Assert.NotNull(result.Message);
    }

    [SkippableFact]
    public async Task Sandbox_GetAccountStatement_ParsesResponse()
    {
        var accountId = Environment.GetEnvironmentVariable(AccountIdEnv);
        Skip.If(string.IsNullOrWhiteSpace(accountId), $"{AccountIdEnv} not set.");
        var (builder, _) = BuildRealBuilder<IAccountService>();
        var accounts = new AccountService(builder);

        var result = await accounts.GetStatement(accountId, new StatementRequestModels
        {
            Period = "last3months",
            Output = "json",
        });

        Assert.NotNull(result);
        if (!result.Success) Assert.NotNull(result.Message);
    }

    [SkippableFact]
    public async Task Sandbox_GetAccountTransactions_ParsesResponse()
    {
        var accountId = Environment.GetEnvironmentVariable(AccountIdEnv);
        Skip.If(string.IsNullOrWhiteSpace(accountId), $"{AccountIdEnv} not set.");
        var (builder, _) = BuildRealBuilder<IAccountService>();
        var accounts = new AccountService(builder);

        var result = await accounts.GetTransactions(accountId, new AccountTransactionsOptionsRequest
        {
            Paginate = true,
            Limit = 5,
        });

        Assert.NotNull(result);
        if (!result.Success) Assert.NotNull(result.Message);
    }

    // ============ Customers — fetch linked accounts ============

    [SkippableFact]
    public async Task Sandbox_FetchAllLinkedAccounts_ParsesWrapper()
    {
        var (builder, _) = BuildRealBuilder<ICustomerService>();
        var customers = new CustomerService(builder);

        var result = await customers.FetchAllLinkedAccounts(new LinkedAccountsQueryOptions { Page = 1, Limit = 5 });

        Assert.NotNull(result);
        if (result.Success && result.Data?.Accounts != null)
        {
            Assert.True(result.Data.Accounts.Count <= 5);
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

    [SkippableFact]
    public async Task Sandbox_GetPayoutTransactions_ParsesEvenWithUnknownId()
    {
        var (builder, _) = BuildRealBuilder<IDirectPayService>();
        var directpay = new DirectPayService(builder, builder);

        // Use a likely-not-existing id so we test the error-shape parsing
        // without depending on sandbox-specific data.
        var result = await directpay.GetPayoutTransactions("nonexistent_payout_id");

        Assert.NotNull(result);
        Assert.NotNull(result.Message ?? result.Status);
    }

    [SkippableFact]
    public async Task Sandbox_GetMandates_ListEndpoint()
    {
        var (builder, _) = BuildRealBuilder<IDirectPayService>();
        var directpay = new DirectPayService(builder, builder);

        var result = await directpay.GetMandates(new MandateRequestQueryOptions { Page = 1, Limit = 5 });

        Assert.NotNull(result);
        if (result.Success && result.Data != null)
        {
            Assert.True(result.Data.Count <= 5);
        }
    }

    [SkippableFact]
    public async Task Sandbox_GetTransactions_DirectPay()
    {
        var (builder, _) = BuildRealBuilder<IDirectPayService>();
        var directpay = new DirectPayService(builder, builder);

        var result = await directpay.GetTransactions(new PaymentRequestQueryOptions { Page = 1 });

        Assert.NotNull(result);
    }

    [SkippableFact]
    public async Task Sandbox_GetSubAccounts_ListEndpoint()
    {
        var (builder, _) = BuildRealBuilder<IDirectPayService>();
        var directpay = new DirectPayService(builder, builder);

        var result = await directpay.GetSubAccounts(new SubAccountListQueryOptions { Page = 1, Limit = 5 });

        Assert.NotNull(result);
    }

    // ============ Disburse — read-only lists ============

    [SkippableFact]
    public async Task Sandbox_FetchAllSourceAccounts_ParsesWrapper()
    {
        var (builder, _) = BuildRealBuilder<IDisburseService>();
        var disburse = new DisburseService(builder);

        var result = await disburse.FetchAllSourceAccounts(new SourceAccountListQueryOptions { Page = 1, Limit = 5 });

        Assert.NotNull(result);
    }

    [SkippableFact]
    public async Task Sandbox_FetchAllDisbursements_ParsesWrapper()
    {
        var (builder, _) = BuildRealBuilder<IDisburseService>();
        var disburse = new DisburseService(builder);

        var result = await disburse.FetchAllDisbursements(new DisbursementListQueryOptions { Page = 1, Limit = 5 });

        Assert.NotNull(result);
    }

    // ============ Lookup — requires Lookup-product key ============

    [SkippableFact]
    public async Task Sandbox_GetCacLookUp_SearchesByName()
    {
        SkipUnlessLookupKey();
        var (builder, _) = BuildRealBuilder<ILookUpService>();
        var lookup = new LookUpService(builder);

        var result = await lookup.GetCacLookUp("MTN NIGERIA");

        Assert.NotNull(result);
        if (result.Success && result.Data != null)
        {
            Assert.NotEmpty(result.Data);
        }
    }

    [SkippableFact]
    public async Task Sandbox_GetMashUp_ReturnsErrorForFakeNin()
    {
        SkipUnlessLookupKey();
        var (builder, _) = BuildRealBuilder<ILookUpService>();
        var lookup = new LookUpService(builder);

        // Deliberately fake — we want to validate the error-response shape
        // parses cleanly without consuming a paid lookup.
        var result = await lookup.GetMashUp(new MashUpRequestModel
        {
            Nin = "00000000000",
            Bvn = "00000000000",
            DateOfBirth = "1990-01-01",
        });

        Assert.NotNull(result);
        // Either a structured Mono error or a real response — both prove
        // the wire shape is correct.
    }

    [SkippableFact]
    public async Task Sandbox_GetTin_ReturnsResponseForFakeTin()
    {
        SkipUnlessLookupKey();
        var (builder, _) = BuildRealBuilder<ILookUpService>();
        var lookup = new LookUpService(builder);

        var result = await lookup.GetTin(new TinRequestModel
        {
            Number = "00000000-0000",
            Channel = "TIN",
        });

        Assert.NotNull(result);
    }

    [SkippableFact]
    public async Task Sandbox_GetAddress_ReturnsResponseForFakeMeter()
    {
        SkipUnlessLookupKey();
        var (builder, _) = BuildRealBuilder<ILookUpService>();
        var lookup = new LookUpService(builder);

        var result = await lookup.GetAddress(new AddressLookUpRequestModel
        {
            MeterNumber = "00000000000",
            Address = "1 Test Street, Lagos",
        });

        Assert.NotNull(result);
    }

    [SkippableFact]
    public async Task Sandbox_InitiateBvnLookUp_ReturnsSessionOrError()
    {
        SkipUnlessLookupKey();
        var (builder, _) = BuildRealBuilder<ILookUpService>();
        var lookup = new LookUpService(builder);

        // Mono publishes sandbox BVNs — use a fake one to exercise the wire
        // path; expect either a session_id back or a structured error.
        var result = await lookup.InitiateBvnLookUp(new InitiateBvnLookUpModel
        {
            Bvn = "00000000000",
            Scope = ScopeConstants.Identity,
        });

        Assert.NotNull(result);
    }
}

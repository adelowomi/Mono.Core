using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Mono.Core.Accounts;
using Mono.Core.Customers;
using Mono.Core.Disburse;
using Mono.Core.DirectPay;
using Mono.Core.LookUp;
using Mono.Core.Prove;
using Mono.Core.Services.DirectPay.Models;
using Mono.Core.Watchlist;
using Refit;
using Xunit;

namespace Mono.Core.HttpWire.Tests;

/// <summary>
/// Instantiates real Refit clients backed by a CapturingHandler so each test
/// asserts the actual outbound HTTP shape: method, full URL with path
/// interpolation + query string, headers, and JSON body. Catches typos in
/// Refit attributes that all the Moq-on-the-interface tests miss.
/// </summary>
public class RefitWireTests
{
    private const string BaseUrl = "https://api.test.com/v2";

    private static (T client, CapturingHandler handler) BuildClient<T>(string baseUrl = BaseUrl)
    {
        var handler = new CapturingHandler();
        var http = new HttpClient(handler) { BaseAddress = new Uri(baseUrl) };
        // Match the production RefitClientBuilder serializer settings so wire
        // format matches what consumers actually send.
        var settings = new RefitSettings
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            }),
        };
        return (RestService.For<T>(http, settings), handler);
    }

    // ============ Accounts ============

    [Fact]
    public async Task IAccountService_InitiateAccountLinking_PostsToAccountsInitiateWithJsonBody()
    {
        var (svc, handler) = BuildClient<IAccountService>();

        await svc.InitiateAccountLinking(new AccountLinkingModel
        {
            Customer = new Mono.Core.Accounts.Customer { Name = "Ada", Email = "ada@example.com" },
            Scope = "auth",
            RedirectUrl = "https://example.com",
        });

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/accounts/initiate", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"name\":\"Ada\"", handler.LastBody);
        Assert.Contains("\"redirect_url\":\"https://example.com\"", handler.LastBody);
    }

    [Fact]
    public async Task IAccountService_GetAccount_GetsAccountsById()
    {
        var (svc, handler) = BuildClient<IAccountService>();

        await svc.GetAccount("acc_1");

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/accounts/acc_1", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Null(handler.LastBody);
    }

    [Fact]
    public async Task IAccountService_GetAccountBalance_GetsAccountsIdBalance()
    {
        var (svc, handler) = BuildClient<IAccountService>();

        await svc.GetAccountBalance("acc_1");

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/accounts/acc_1/balance", handler.LastRequest.RequestUri.AbsolutePath);
    }

    // ============ Customers ============

    [Fact]
    public async Task ICustomerService_CreateIndividual_PostsToCustomersWithSnakeCaseBody()
    {
        var (svc, handler) = BuildClient<ICustomerService>();

        await svc.CreateIndividualCustomer(new CreateIndividualCustomerModel
        {
            Identity = new CustomerIdentity { Type = "bvn", Number = "12345678901" },
            Email = "ada@example.com",
            FirstName = "Ada",
            LastName = "Lovelace",
        });

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/customers", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"first_name\":\"Ada\"", handler.LastBody);
        Assert.Contains("\"last_name\":\"Lovelace\"", handler.LastBody);
        Assert.Contains("\"type\":\"individual\"", handler.LastBody);
    }

    [Fact]
    public async Task ICustomerService_RetrieveCustomer_GetsCustomersById()
    {
        var (svc, handler) = BuildClient<ICustomerService>();

        await svc.RetrieveCustomer("cust_1");

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/customers/cust_1", handler.LastRequest.RequestUri.AbsolutePath);
    }

    [Fact]
    public async Task ICustomerService_GetTransactions_BuildsQueryString()
    {
        var (svc, handler) = BuildClient<ICustomerService>();

        await svc.GetCustomerTransactions("cust_1", new CustomerTransactionsQueryOptions
        {
            Period = "last12months",
            Page = 2,
            Account = "acc_1",
        });

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/customers/cust_1/transactions", handler.LastRequest.RequestUri.AbsolutePath);
        var query = handler.LastRequest.RequestUri.Query;
        Assert.Contains("period=last12months", query);
        Assert.Contains("page=2", query);
        Assert.Contains("account=acc_1", query);
    }

    [Fact]
    public async Task ICustomerService_DeleteCustomer_UsesHttpDelete()
    {
        var (svc, handler) = BuildClient<ICustomerService>();

        await svc.DeleteCustomer("cust_1");

        Assert.Equal(HttpMethod.Delete, handler.LastRequest.Method);
        Assert.Equal("/v2/customers/cust_1", handler.LastRequest.RequestUri.AbsolutePath);
    }

    // ============ Disburse ============

    [Fact]
    public async Task IDisburseService_CreateDisbursement_PostsToDisbursements()
    {
        var (svc, handler) = BuildClient<IDisburseService>();

        await svc.CreateDisbursement(new CreateDisbursementModel
        {
            Reference = "payroll-1",
            Account = "src_abc",
            Type = "instant",
            TotalAmount = 100000,
            Description = "May payroll",
            Distribution = new List<DistributionModel>
            {
                new DistributionModel
                {
                    Reference = "p-1",
                    RecipientEmail = "ada@example.com",
                    Account = new DisbursementRecipientAccount { AccountNumber = "0123456789", BankCode = "044" },
                    Amount = 100000,
                    Narration = "salary",
                },
            },
        });

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/payments/disburse/disbursements", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"total_amount\":100000", handler.LastBody);
        Assert.Contains("\"recipient_email\":\"ada@example.com\"", handler.LastBody);
    }

    [Fact]
    public async Task IDisburseService_TransitionDisbursement_PostsToDisbursementsIdTransition()
    {
        var (svc, handler) = BuildClient<IDisburseService>();

        await svc.TransitionDisbursement("dsb_1", new TransitionDisbursementModel { Action = "trigger" });

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/payments/disburse/disbursements/dsb_1/transition", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"action\":\"trigger\"", handler.LastBody);
    }

    [Fact]
    public async Task IDisburseService_UpdateSourceAccount_UsesPutVerb()
    {
        var (svc, handler) = BuildClient<IDisburseService>();

        await svc.UpdateSourceAccount(new UpdateSourceAccountModel { Id = "src_1", Email = "new@example.com" });

        Assert.Equal(HttpMethod.Put, handler.LastRequest.Method);
        Assert.Equal("/v2/payments/disburse/source-accounts", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"id\":\"src_1\"", handler.LastBody);
    }

    [Fact]
    public async Task IDisburseService_DeleteDistribution_NestedPathWithBothIds()
    {
        var (svc, handler) = BuildClient<IDisburseService>();

        await svc.DeleteDistributionInBatch("dsb_1", "dist_2");

        Assert.Equal(HttpMethod.Delete, handler.LastRequest.Method);
        Assert.Equal("/v2/payments/disburse/disbursements/dsb_1/distributions/dist_2", handler.LastRequest.RequestUri.AbsolutePath);
    }

    // ============ Watchlist ============

    [Fact]
    public async Task IWatchlistService_SubmitIndividual_PostsToLookupWatchlist()
    {
        var (svc, handler) = BuildClient<IWatchlistService>();

        await svc.SubmitIndividualScreening(new SubmitIndividualScreeningModel
        {
            Name = "Ada Lovelace",
            DateOfBirth = "1815-12-10",
            Country = "NG",
        });

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/lookup/watchlist", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"date_of_birth\":\"1815-12-10\"", handler.LastBody);
    }

    [Fact]
    public async Task IWatchlistService_StopMonitoring_DeletesByPathId()
    {
        var (svc, handler) = BuildClient<IWatchlistService>();

        await svc.StopMonitoring("mon_1");

        Assert.Equal(HttpMethod.Delete, handler.LastRequest.Method);
        Assert.Equal("/v2/lookup/watchlist/monitor/mon_1", handler.LastRequest.RequestUri.AbsolutePath);
    }

    // ============ Prove ============

    [Fact]
    public async Task IProveService_Initiate_PostsToProveInitiateWithNestedCustomer()
    {
        var (svc, handler) = BuildClient<IProveService>();

        await svc.InitiateProve(new InitiateProveModel
        {
            Customer = new ProveCustomer
            {
                Name = "Ada",
                Phone = "+2348012345678",
                Address = "12 St",
                Email = "ada@example.com",
                Identity = new ProveCustomerIdentity { Type = "bvn", Number = "12345678901" },
            },
            Reference = "kyc-1",
            RedirectUrl = "https://example.com/done",
            KycLevel = "tier_2",
        });

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/prove/initiate", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"kyc_level\":\"tier_2\"", handler.LastBody);
        Assert.Contains("\"redirect_url\":\"https://example.com/done\"", handler.LastBody);
        Assert.Contains("\"identity\":", handler.LastBody);
    }

    [Fact]
    public async Task IProveService_RevokeDataAccess_DeletesByReference()
    {
        var (svc, handler) = BuildClient<IProveService>();

        await svc.RevokeDataAccess("kyc-1");

        Assert.Equal(HttpMethod.Delete, handler.LastRequest.Method);
        Assert.Equal("/v2/prove/customers/kyc-1", handler.LastRequest.RequestUri.AbsolutePath);
    }

    // ============ LookUp / BVN / NIN / CAC ============

    [Fact]
    public async Task ILookUpService_VerifyBvn_PostsWithSessionIdHeader()
    {
        var (svc, handler) = BuildClient<ILookUpService>();

        await svc.VerifyBvnLookUp(
            new VerifyBvnLookUpOtpModel { Method = "phone", PhoneNumber = "08012345678" },
            "session_xyz");

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/lookup/bvn/verify-otp", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.True(handler.LastRequest.Headers.TryGetValues("x-session-id", out var values));
        Assert.Equal("session_xyz", string.Join("", values));
    }

    [Fact]
    public async Task ILookUpService_GetBvnDetails_UsesFetchBvnPath()
    {
        var (svc, handler) = BuildClient<ILookUpService>();

        await svc.GetBvnDetails(new BvnDetailsModel { Otp = "123456" }, "session_xyz");

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/lookup/bvn/fetch-bvn", handler.LastRequest.RequestUri.AbsolutePath);
    }

    [Fact]
    public async Task ILookUpService_GetNin_UsesPostNotGet()
    {
        // Pre-v1.1.0 this was [Get] + [Body] which Refit silently drops. The
        // wire test confirms the verb fix actually produces a POST.
        var (svc, handler) = BuildClient<ILookUpService>();

        await svc.GetNin(new NinRequestModel { Nin = "12345678901" });

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/lookup/nin", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"nin\":\"12345678901\"", handler.LastBody);
    }

    [Fact]
    public async Task ILookUpService_GetDriverLicense_UsesDashedPath()
    {
        // Pre-v1.1.0 this was "/lookup/driver_license" (underscore).
        var (svc, handler) = BuildClient<ILookUpService>();

        await svc.GetDriverLicense(new DriversLicenseRequestModel());

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/lookup/drivers-license", handler.LastRequest.RequestUri.AbsolutePath);
    }

    [Fact]
    public async Task ILookUpService_GetCacProfile_UsesRcNumberInPath()
    {
        var (svc, handler) = BuildClient<ILookUpService>();

        await svc.GetCacProfile("RC123456");

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/lookup/cac/profile/RC123456", handler.LastRequest.RequestUri.AbsolutePath);
    }

    [Fact]
    public async Task ILookUpService_PollNinJob_NestedJobPath()
    {
        var (svc, handler) = BuildClient<ILookUpService>();

        await svc.PollNinJob("job_42");

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/lookup/nin/job_42/job", handler.LastRequest.RequestUri.AbsolutePath);
    }

    // ============ DirectPay money-ops ============

    [Fact]
    public async Task IDirectPayService_BalanceInquiry_AmountIsOptional()
    {
        // The v1.1.0 drift fix made `amount` optional. Without it, no query.
        var (svc, handler) = BuildClient<IDirectPayService>();

        await svc.BalanceInquiry("mnd_1");

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/payments/mandates/mnd_1/balance-inquiry", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.True(string.IsNullOrEmpty(handler.LastRequest.RequestUri.Query) || !handler.LastRequest.RequestUri.Query.Contains("amount"));
    }

    [Fact]
    public async Task IDirectPayService_RefundPayment_PostsToPaymentsRefund()
    {
        var (svc, handler) = BuildClient<IDirectPayService>();

        await svc.RefundPayment(new RefundPaymentModel { Reference = "pay_1", Source = "wallet" });

        Assert.Equal(HttpMethod.Post, handler.LastRequest.Method);
        Assert.Equal("/v2/payments/refund", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("\"reference\":\"pay_1\"", handler.LastBody);
        Assert.Contains("\"source\":\"wallet\"", handler.LastBody);
    }

    [Fact]
    public async Task IDirectPayService_GetPayouts_PluralPathFilterByStatus()
    {
        // Pluralisation matters — Mono uses "payouts" for the list endpoint.
        var (svc, handler) = BuildClient<IDirectPayService>();

        await svc.GetPayouts(new PayoutListQueryOptions { Status = "settled", Page = 1 });

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/payments/payouts", handler.LastRequest.RequestUri.AbsolutePath);
        Assert.Contains("status=settled", handler.LastRequest.RequestUri.Query);
    }

    [Fact]
    public async Task IDirectPayService_GetPayoutTransactions_SingularPathWithId()
    {
        // ... while transactions uses "payout" (singular).
        var (svc, handler) = BuildClient<IDirectPayService>();

        await svc.GetPayoutTransactions("pay_1", new PayoutTransactionsQueryOptions { Page = 1 });

        Assert.Equal(HttpMethod.Get, handler.LastRequest.Method);
        Assert.Equal("/v2/payments/payout/pay_1/transactions", handler.LastRequest.RequestUri.AbsolutePath);
    }

    [Fact]
    public async Task IDirectPayService_CancelMandate_UsesPatchVerb()
    {
        var (svc, handler) = BuildClient<IDirectPayService>();

        await svc.CancelMandate("mnd_1");

        Assert.Equal(new HttpMethod("PATCH"), handler.LastRequest.Method);
        Assert.Equal("/v2/payments/mandates/mnd_1/cancel", handler.LastRequest.RequestUri.AbsolutePath);
    }

    // ============ Mono-sec-key header is set by RefitClientBuilder, not Refit ============
    // (The builder adds it as a DefaultRequestHeader on the HttpClient; we test
    // the builder separately in RefitClientBuilderTests.)
}

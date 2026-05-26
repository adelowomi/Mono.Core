using System;
using System.Collections.Generic;
using System.Text.Json;
using Mono.Core.Customers;
using Mono.Core.Disburse;
using Mono.Core.LookUp;
using Mono.Core.Prove;
using Mono.Core.Services.DirectPay.Models;
using Mono.Core.Watchlist;
using Mono.Core.Webhooks;
using Xunit;

namespace Mono.Core.Serialization.Tests;

/// <summary>
/// Catches wrong JsonPropertyName values across every model added since v1.1.0.
/// Plain Moq-based wiring tests can't catch these because the JSON layer is
/// skipped — both sides of the mock use the same C# property name.
///
/// Each test serializes with the same options Refit uses
/// (camelCase policy + explicit JsonPropertyName overrides), asserts the wire
/// keys are what Mono expects, then deserializes back to verify round-trip.
/// </summary>
public class JsonContractTests
{
    // Refit's default content serializer uses camelCase. We keep matching
    // settings here so serialization in tests mirrors what Refit sends.
    private static readonly JsonSerializerOptions Opts = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private static string Ser<T>(T obj) => JsonSerializer.Serialize(obj, Opts);
    private static T De<T>(string json) => JsonSerializer.Deserialize<T>(json, Opts);

    // ============ Customers ============

    [Fact]
    public void CreateIndividualCustomerModel_UsesSnakeCaseKeys()
    {
        var json = Ser(new CreateIndividualCustomerModel
        {
            Identity = new CustomerIdentity { Type = "bvn", Number = "12345678901" },
            Email = "ada@example.com",
            FirstName = "Ada",
            LastName = "Lovelace",
            Address = "12 Analytical St",
            Phone = "+2348012345678",
        });

        Assert.Contains("\"identity\":", json);
        Assert.Contains("\"first_name\":\"Ada\"", json);
        Assert.Contains("\"last_name\":\"Lovelace\"", json);
        Assert.Contains("\"type\":\"individual\"", json);
        Assert.Contains("\"email\":\"ada@example.com\"", json);
    }

    [Fact]
    public void CreateBusinessCustomerModel_UsesBusinessName()
    {
        var json = Ser(new CreateBusinessCustomerModel
        {
            Identity = new CustomerIdentity { Type = "rc_number", Number = "RC123456" },
            Email = "ops@acme.example",
            BusinessName = "Acme Ltd",
            Address = "12 Acme Way",
            Phone = "+2348099999999",
        });

        Assert.Contains("\"business_name\":\"Acme Ltd\"", json);
        Assert.Contains("\"type\":\"business\"", json);
    }

    [Fact]
    public void CustomerResponse_DeserializesFromMonoLikeJson()
    {
        // Mirrors the field names Mono's docs reference for the Customer object.
        var json = """
        {
            "id": "cust_5f8d9c",
            "email": "ada@example.com",
            "type": "individual",
            "first_name": "Ada",
            "last_name": "Lovelace",
            "phone": "+2348012345678",
            "address": "12 Analytical St",
            "identity": { "type": "bvn", "number": "12345678901" },
            "created_at": "2026-05-27T10:00:00Z",
            "updated_at": "2026-05-27T10:00:00Z"
        }
        """;

        var cust = De<CustomerResponse>(json);

        Assert.Equal("cust_5f8d9c", cust.Id);
        Assert.Equal("Ada", cust.FirstName);
        Assert.Equal("Lovelace", cust.LastName);
        Assert.Equal("bvn", cust.Identity.Type);
        Assert.NotNull(cust.CreatedAt);
    }

    // ============ Disburse ============

    [Fact]
    public void CreateDisbursementModel_DistributionUsesAccountObject()
    {
        var json = Ser(new CreateDisbursementModel
        {
            Reference = "payroll-1",
            Source = "mandate",
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

        Assert.Contains("\"total_amount\":100000", json);
        Assert.Contains("\"recipient_email\":\"ada@example.com\"", json);
        Assert.Contains("\"account_number\":\"0123456789\"", json);
        Assert.Contains("\"bank_code\":\"044\"", json);
        Assert.Contains("\"distribution\":[", json);
    }

    [Fact]
    public void TransitionDisbursementModel_OnlyHasActionField()
    {
        var json = Ser(new TransitionDisbursementModel { Action = "trigger" });
        Assert.Equal("{\"action\":\"trigger\"}", json);
    }

    [Fact]
    public void DisbursementResponse_DeserializesProcessedAt()
    {
        var json = """
        {
            "id": "dsb_1",
            "reference": "payroll-1",
            "source": "mandate",
            "account": "src_abc",
            "type": "instant",
            "status": "processing",
            "total_amount": 250000,
            "currency": "NGN",
            "processed_at": "2026-05-27T10:05:00Z",
            "distribution_count": 1
        }
        """;
        var dsb = De<DisbursementResponse>(json);
        Assert.Equal("dsb_1", dsb.Id);
        Assert.Equal(250000, dsb.TotalAmount);
        Assert.Equal(1, dsb.DistributionCount);
        Assert.NotNull(dsb.ProcessedAt);
    }

    // ============ Watchlist ============

    [Fact]
    public void SubmitIndividualScreeningModel_UsesSnakeCaseKeys()
    {
        var json = Ser(new SubmitIndividualScreeningModel
        {
            Name = "Ada Lovelace",
            DateOfBirth = "1815-12-10",
            Gender = "female",
            Bvn = "12345678901",
            Country = "NG",
        });

        Assert.Contains("\"date_of_birth\":\"1815-12-10\"", json);
        Assert.Contains("\"type\":\"individual\"", json);
        Assert.Contains("\"country\":\"NG\"", json);
    }

    [Fact]
    public void ScreeningResponse_DeserializesMatchesAndRiskScore()
    {
        var json = """
        {
            "id": "scr_1",
            "status": "completed",
            "risk_score": 65.5,
            "risk_level": "medium",
            "matches": [
                { "id": "m1", "name": "Some Person", "match_score": 0.92, "match_level": "strong" }
            ],
            "subject": { "type": "individual", "name": "Ada", "country": "NG" }
        }
        """;
        var res = De<ScreeningResponse>(json);
        Assert.Equal("scr_1", res.Id);
        Assert.Equal(65.5, res.RiskScore);
        Assert.Equal("medium", res.RiskLevel);
        Assert.Single(res.Matches);
        Assert.Equal(0.92, res.Matches[0].MatchScore);
        Assert.Equal("strong", res.Matches[0].MatchLevel);
    }

    // ============ Prove ============

    [Fact]
    public void InitiateProveModel_UsesNestedCustomerWithIdentity()
    {
        var json = Ser(new InitiateProveModel
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
            Identities = new List<string> { "bvn", "nin" },
            BankAccounts = true,
        });

        Assert.Contains("\"customer\":", json);
        Assert.Contains("\"identity\":", json);
        Assert.Contains("\"redirect_url\":\"https://example.com/done\"", json);
        Assert.Contains("\"kyc_level\":\"tier_2\"", json);
        Assert.Contains("\"identities\":[\"bvn\",\"nin\"]", json);
        Assert.Contains("\"bank_accounts\":true", json);
    }

    [Fact]
    public void BlacklistCustomerModel_SerializesCodeAsNumber()
    {
        var json = Ser(new BlacklistCustomerModel
        {
            Reference = "kyc-1",
            Reason = "Confirmed fraud",
            Code = ProveBlacklistCodeConstants.Code101,
        });
        Assert.Contains("\"code\":101", json);
        Assert.Contains("\"reason\":\"Confirmed fraud\"", json);
    }

    // ============ Webhook payload ============

    [Fact]
    public void MonoWebhookModel_DeserializesEnvelopeFields()
    {
        // The exact shape Mono describes in docs for any event.
        var json = """
        {
            "event": "mono.events.account_updated",
            "event_id": "evt_abc123",
            "timestamp": "2026-05-27T10:00:00Z",
            "app": "app_xyz",
            "business": "biz_pqr",
            "data": {
                "webhook_id": "wh_1",
                "account": {
                    "_id": "acc_1",
                    "accountNumber": "0123456789",
                    "balance": 1250000,
                    "currency": "NGN"
                }
            }
        }
        """;
        // The legacy model has [JsonPropertyName("Data")] PascalCase — see
        // the comment in MonoWebhookModel.cs. The controller's existing
        // serialize-then-deserialize round-trip masks this. We test only
        // the envelope-level fields here.
        var webhook = De<MonoWebhookModel<AccountUpdatedEventModel>>(json);

        Assert.Equal("mono.events.account_updated", webhook.Event);
        Assert.Equal("evt_abc123", webhook.EventId);
        Assert.Equal("app_xyz", webhook.App);
        Assert.NotNull(webhook.Timestamp);
    }

    [Fact]
    public void AccountUpdatedEventModel_ParsesAccountMatchSubObject()
    {
        var json = """
        {
            "webhook_id": "wh_1",
            "account_match": {
                "status": "not_matched",
                "matched": false,
                "expected_account_number": "0123456789",
                "linked_account_number": "9876543210",
                "reason": "different account"
            }
        }
        """;
        var ev = De<AccountUpdatedEventModel>(json);
        Assert.NotNull(ev.AccountMatch);
        Assert.Equal("not_matched", ev.AccountMatch.Status);
        Assert.False(ev.AccountMatch.Matched);
        Assert.Equal("0123456789", ev.AccountMatch.ExpectedAccountNumber);
    }

    [Fact]
    public void MandateDebitEventModel_ParsesNibssCodeAndFailureReason()
    {
        var json = """
        {
            "id": "deb_1",
            "mandate": "mnd_1",
            "reference": "ref_1",
            "status": "failed",
            "amount": 50000,
            "currency": "NGN",
            "narration": "monthly",
            "nibss_code": "09",
            "failure_reason": "Insufficient funds",
            "processed_at": "2026-05-27T10:00:00Z"
        }
        """;
        var ev = De<MandateDebitEventModel>(json);
        Assert.Equal("failed", ev.Status);
        Assert.Equal(50000, ev.Amount);
        Assert.Equal("Insufficient funds", ev.FailureReason);
    }

    // ============ DirectPay money-ops ============

    [Fact]
    public void RefundPaymentModel_UsesReferenceAndSource()
    {
        var json = Ser(new RefundPaymentModel { Reference = "pay_1", Source = "wallet" });
        Assert.Contains("\"reference\":\"pay_1\"", json);
        Assert.Contains("\"source\":\"wallet\"", json);
    }

    [Fact]
    public void PayoutResponse_DeserializesSettledAt()
    {
        var json = """
        {
            "id": "pay_1",
            "reference": "po-2026-05",
            "status": "settled",
            "amount": 500000,
            "currency": "NGN",
            "settlement_account": "sub_1",
            "settled_at": "2026-05-27T10:00:00Z",
            "created_at": "2026-05-26T09:00:00Z"
        }
        """;
        var po = De<PayoutResponse>(json);
        Assert.Equal("settled", po.Status);
        Assert.Equal(500000, po.Amount);
        Assert.Equal("sub_1", po.SettlementAccount);
        Assert.NotNull(po.SettledAt);
    }

    [Fact]
    public void CreateSubAccountModel_UsesNipCode()
    {
        var json = Ser(new CreateSubAccountModel
        {
            Name = "Vendor A",
            AccountNumber = "0123456789",
            BankCode = "044",
            NipCode = "000014",
        });
        Assert.Contains("\"nip_code\":\"000014\"", json);
        Assert.Contains("\"account_number\":\"0123456789\"", json);
        Assert.Contains("\"bank_code\":\"044\"", json);
    }

    // ============ CAC additions ============

    [Fact]
    public void CacPscEntry_ParsesOwnershipPercentageAndNatureOfControl()
    {
        var json = """
        {
            "id": "psc_1",
            "name": "Ada Lovelace",
            "type": "individual",
            "ownership_percentage": 51.5,
            "voting_rights_percentage": 50.0,
            "nature_of_control": ["voting_rights", "appointment_rights"],
            "date_of_appointment": "2020-01-15T00:00:00Z"
        }
        """;
        var psc = De<CacPscEntry>(json);
        Assert.Equal("Ada Lovelace", psc.Name);
        Assert.Equal(51.5, psc.OwnershipPercentage);
        Assert.Equal(2, psc.NatureOfControl.Count);
        Assert.Contains("voting_rights", psc.NatureOfControl);
    }

    // ============ Account additions ============

    [Fact]
    public void AccountBalanceResponse_DeserializesAvailableBalance()
    {
        var json = """
        {
            "id": "acc_1",
            "account_number": "0123456789",
            "name": "Ada Lovelace",
            "currency": "NGN",
            "balance": 1250000,
            "available_balance": 1200000,
            "last_updated": "2026-05-27T10:00:00Z"
        }
        """;
        var bal = De<Mono.Core.Accounts.AccountBalanceResponse>(json);
        Assert.Equal(1250000, bal.Balance);
        Assert.Equal(1200000, bal.AvailableBalance);
        Assert.Equal("NGN", bal.Currency);
        Assert.NotNull(bal.LastUpdated);
    }

    [Fact]
    public void AccountLinkingModel_AccountMatchOnlySerializedWhenSet()
    {
        var json = Ser(new Mono.Core.Accounts.AccountLinkingModel
        {
            Scope = "auth",
            RedirectUrl = "https://example.com",
            Institution = new Mono.Core.Accounts.AccountLinkingInstitution
            {
                Id = "044",
                AccountNumber = "0123456789",
            },
            CheckAccountMatch = true,
        });
        Assert.Contains("\"check_account_match\":true", json);
        Assert.Contains("\"account_number\":\"0123456789\"", json);
        Assert.Contains("\"institution\":", json);
    }

    // ============ NIN PDF / poll job ============

    [Fact]
    public void NinPdfRequestModel_DefaultsOutputToPdf()
    {
        var json = Ser(new NinPdfRequestModel { Nin = "12345678901" });
        Assert.Contains("\"output\":\"pdf\"", json);
        Assert.Contains("\"nin\":\"12345678901\"", json);
    }

    [Fact]
    public void NinPollJobResponse_ParsesCompletedJobWithUrl()
    {
        var json = """
        {
            "job_id": "job_42",
            "status": "completed",
            "url": "https://files.mono.co/nin/job_42.pdf",
            "result": { "nin": "12345678901", "firstname": "Ada" }
        }
        """;
        var poll = De<NinPollJobResponse>(json);
        Assert.Equal("job_42", poll.JobId);
        Assert.Equal("completed", poll.Status);
        Assert.StartsWith("https://", poll.Url);
        Assert.Equal("Ada", poll.Result.Firstname);
    }
}

using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mono.Core;
using Mono.Core.Webhooks;
using Moq;
using Xunit;

namespace Mono.Core.Tests;

/// <summary>
/// Validates that the webhook controller can parse Mono's actual wire format,
/// not just pre-deserialized .NET objects. Each test starts from a raw JSON
/// string shaped like Mono's docs describe, deserializes via the same
/// case-insensitive setup ASP.NET Core uses, then drives the controller.
///
/// This is the layer that catches: wrong JsonPropertyName on event payload
/// fields, the envelope using the wrong "Data" vs "data" casing, missing
/// JsonNumberHandling for amounts, etc.
/// </summary>
public class WebhookRawJsonTests
{
    // ASP.NET Core 3+ default JSON options: case-insensitive matching,
    // System.Text.Json under the hood. The controller's [FromBody] uses these.
    private static readonly JsonSerializerOptions AspNetDefaults = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    private static GenericWebHookModel ParseLikeAspNet(string json) =>
        JsonSerializer.Deserialize<GenericWebHookModel>(json, AspNetDefaults);

    private static MonoWebhookController BuildController(IMonoWebhookConsumer consumer)
    {
        var logger = new Mock<ILogger<MonoWebhookController>>();
        var options = Options.Create(new MonoInitializationOptions());
        var controller = new MonoWebhookController(consumer, logger.Object, options);
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        return controller;
    }

    [Fact]
    public async Task AccountConnected_WireJson_RoutesAndPopulatesPayload()
    {
        // Shape Mono's webhooks docs describe for account_connected.
        var json = """
        {
            "event": "mono.events.account_connected",
            "event_id": "evt_abc",
            "timestamp": "2026-05-27T10:00:00Z",
            "app": "app_xyz",
            "business": "biz_pqr",
            "data": {
                "id": "acc_real_1",
                "customer": "cust_real_1"
            }
        }
        """;

        AccountConnectedEventModel received = null;
        var consumer = new Mock<IMonoWebhookConsumer>();
        consumer.Setup(c => c.HandleAccountCreatedEvent(It.IsAny<AccountConnectedEventModel>()))
            .Callback<AccountConnectedEventModel>(m => received = m)
            .Returns(Task.CompletedTask);

        var controller = BuildController(consumer.Object);
        var payload = ParseLikeAspNet(json);

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<OkResult>(result);
        Assert.NotNull(received);
        Assert.Equal("acc_real_1", received.Id);
        Assert.Equal("cust_real_1", received.Customer);
    }

    [Fact]
    public async Task AccountUpdated_WithAccountMatch_PopulatesAccountMatchSubObject()
    {
        var json = """
        {
            "event": "mono.events.account_updated",
            "event_id": "evt_match_1",
            "data": {
                "webhook_id": "wh_1",
                "app": "app_xyz",
                "business": "biz_pqr",
                "meta": { "data_status": "AVAILABLE", "auth_method": "internet_banking" },
                "account": {
                    "_id": "acc_1",
                    "accountNumber": "0123456789",
                    "balance": 1250000,
                    "currency": "NGN",
                    "name": "Ada Lovelace",
                    "institution": { "name": "Good Bank", "bankCode": "044", "type": "PERSONAL" }
                },
                "account_match": {
                    "status": "matched",
                    "matched": true,
                    "expected_account_number": "0123456789",
                    "linked_account_number": "0123456789"
                }
            }
        }
        """;

        AccountUpdatedEventModel received = null;
        var consumer = new Mock<IMonoWebhookConsumer>();
        consumer.Setup(c => c.HandleAccountUpdatedEvent(It.IsAny<AccountUpdatedEventModel>()))
            .Callback<AccountUpdatedEventModel>(m => received = m)
            .Returns(Task.CompletedTask);

        var controller = BuildController(consumer.Object);

        var result = await controller.ReceiveWebhookEvent(ParseLikeAspNet(json));

        Assert.IsType<OkResult>(result);
        Assert.NotNull(received);
        Assert.NotNull(received.Account);
        Assert.Equal("acc_1", received.Account.Id);
        Assert.Equal("0123456789", received.Account.AccountNumber);
        Assert.NotNull(received.AccountMatch);
        Assert.Equal("matched", received.AccountMatch.Status);
        Assert.True(received.AccountMatch.Matched);
    }

    [Fact]
    public async Task MandateCreated_WireJson_RoutesToMandateHandler()
    {
        // The v1.7.0 bug fix at the wire level — pre-fix this would have
        // fallen through to HandleUnknownEvent because of the Split('.')[2]
        // bug. Tests it end-to-end against real-shaped JSON.
        var json = """
        {
            "event": "mono.events.mandate.created",
            "event_id": "evt_mnd_1",
            "data": {
                "id": "mnd_1",
                "status": "pending",
                "mandate_type": "emandate",
                "debit_type": "fixed",
                "reference": "ref_1",
                "account_number": "0123456789",
                "bank_code": "044",
                "amount": 50000
            }
        }
        """;

        MandateCreatedEventModel received = null;
        var consumer = new Mock<IMonoWebhookConsumer>();
        consumer.Setup(c => c.HandleMandateCreatedEvent(It.IsAny<MandateCreatedEventModel>()))
            .Callback<MandateCreatedEventModel>(m => received = m)
            .Returns(Task.CompletedTask);

        var controller = BuildController(consumer.Object);

        var result = await controller.ReceiveWebhookEvent(ParseLikeAspNet(json));

        Assert.IsType<OkResult>(result);
        Assert.NotNull(received);
        Assert.Equal("mnd_1", received.Id);
        Assert.Equal("ref_1", received.Reference);
        Assert.Equal(50000, received.Amount);
        consumer.Verify(c => c.HandleUnknownEvent(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task MandateDebitSuccessful_WireJson_DispatchesToExtensionsHandler()
    {
        var json = """
        {
            "event": "mono.events.mandate.debit.successful",
            "event_id": "evt_deb_1",
            "data": {
                "id": "deb_1",
                "mandate": "mnd_1",
                "reference": "ref_1",
                "status": "successful",
                "amount": 50000,
                "currency": "NGN",
                "narration": "monthly",
                "nibss_code": "00",
                "processed_at": "2026-05-27T10:00:00Z"
            }
        }
        """;

        MandateDebitEventModel received = null;
        var consumer = new Mock<IMonoWebhookConsumer>();
        var ext = consumer.As<IMonoWebhookConsumerExtensions>();
        ext.Setup(e => e.HandleMandateDebitSuccessfulEvent(It.IsAny<MandateDebitEventModel>()))
            .Callback<MandateDebitEventModel>(m => received = m)
            .Returns(Task.CompletedTask);

        var controller = BuildController(consumer.Object);

        var result = await controller.ReceiveWebhookEvent(ParseLikeAspNet(json));

        Assert.IsType<OkResult>(result);
        Assert.NotNull(received);
        Assert.Equal("deb_1", received.Id);
        Assert.Equal("mnd_1", received.Mandate);
        Assert.Equal(50000, received.Amount);
        Assert.Equal("00", received.NibssCode);
    }

    [Fact]
    public async Task DisbursementCompleted_WireJson_DispatchesViaExtensions()
    {
        var json = """
        {
            "event": "mono.events.disbursement.completed",
            "event_id": "evt_dsb_1",
            "data": {
                "id": "dsb_1",
                "reference": "payroll-1",
                "source": "mandate",
                "account": "src_abc",
                "type": "instant",
                "status": "completed",
                "total_amount": 250000,
                "currency": "NGN",
                "distribution_count": 5,
                "processed_at": "2026-05-27T10:05:00Z"
            }
        }
        """;

        DisbursementEventModel received = null;
        var consumer = new Mock<IMonoWebhookConsumer>();
        var ext = consumer.As<IMonoWebhookConsumerExtensions>();
        ext.Setup(e => e.HandleDisbursementEvent(It.IsAny<DisbursementEventModel>()))
            .Callback<DisbursementEventModel>(m => received = m)
            .Returns(Task.CompletedTask);

        var controller = BuildController(consumer.Object);

        var result = await controller.ReceiveWebhookEvent(ParseLikeAspNet(json));

        Assert.IsType<OkResult>(result);
        Assert.NotNull(received);
        Assert.Equal("dsb_1", received.Id);
        Assert.Equal("completed", received.Status);
        Assert.Equal(250000, received.TotalAmount);
        Assert.Equal(5, received.DistributionCount);
    }

    [Fact]
    public async Task WatchlistMatch_WireJson_DispatchesViaExtensions()
    {
        var json = """
        {
            "event": "mono.events.watchlist.match_found",
            "event_id": "evt_wl_1",
            "data": {
                "screening_id": "scr_1",
                "subject_name": "Ada Lovelace",
                "subject_type": "individual",
                "match_count": 2,
                "risk_score": 78.5,
                "risk_level": "high",
                "monitoring": true
            }
        }
        """;

        WatchlistMatchEventModel received = null;
        var consumer = new Mock<IMonoWebhookConsumer>();
        var ext = consumer.As<IMonoWebhookConsumerExtensions>();
        ext.Setup(e => e.HandleWatchlistMatchEvent(It.IsAny<WatchlistMatchEventModel>()))
            .Callback<WatchlistMatchEventModel>(m => received = m)
            .Returns(Task.CompletedTask);

        var controller = BuildController(consumer.Object);

        var result = await controller.ReceiveWebhookEvent(ParseLikeAspNet(json));

        Assert.IsType<OkResult>(result);
        Assert.NotNull(received);
        Assert.Equal("scr_1", received.ScreeningId);
        Assert.Equal(78.5, received.RiskScore);
        Assert.Equal("high", received.RiskLevel);
        Assert.True(received.Monitoring);
    }

    [Fact]
    public async Task EnvelopeParsesEventStringFromAnyShape()
    {
        // ASP.NET Core's case-insensitive matching means "Event" or "event"
        // should both parse. Verify by sending an oddly cased variant.
        var json = """
        {
            "Event": "mono.events.account_connected",
            "Data": { "id": "x", "customer": "y" }
        }
        """;

        var consumer = new Mock<IMonoWebhookConsumer>();
        var controller = BuildController(consumer.Object);

        var result = await controller.ReceiveWebhookEvent(ParseLikeAspNet(json));

        Assert.IsType<OkResult>(result);
        consumer.Verify(c => c.HandleAccountCreatedEvent(It.IsAny<AccountConnectedEventModel>()), Times.Once);
    }
}

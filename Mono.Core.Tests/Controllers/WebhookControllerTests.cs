using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mono.Core;
using Mono.Core.Webhooks;
using Moq;
using Xunit;
using WebhookAccount = Mono.Core.Webhooks.Account;
using WebhookInstitution = Mono.Core.Webhooks.Institution;

namespace Mono.Core.Tests;

public class WebhookControllerTests
{
    private readonly Mock<IMonoWebhookConsumer> _consumer;
    private readonly Mock<ILogger<MonoWebhookController>> _logger;

    public WebhookControllerTests()
    {
        _consumer = new Mock<IMonoWebhookConsumer>();
        _logger = new Mock<ILogger<MonoWebhookController>>();
    }

    private MonoWebhookController BuildController(string webhookSecret = null, string headerValue = null)
    {
        var options = Options.Create(new MonoInitializationOptions { WebhookSecret = webhookSecret });
        var controller = new MonoWebhookController(_consumer.Object, _logger.Object, options);

        var httpContext = new DefaultHttpContext();
        if (headerValue != null)
        {
            httpContext.Request.Headers["mono-webhook-secret"] = headerValue;
        }
        controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        return controller;
    }

    // ============ Signature verification ============

    [Fact]
    public async Task ReceiveWebhookEvent_WithMatchingSignature_Returns200()
    {
        var controller = BuildController(webhookSecret: "shh", headerValue: "shh");
        var payload = new GenericWebHookModel
        {
            Event = "mono.events.account_connected",
            Data = new AccountConnectedEventModel { Id = "acc_1", Customer = "cust_1" },
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<OkResult>(result);
        _consumer.Verify(c => c.HandleAccountCreatedEvent(It.IsAny<AccountConnectedEventModel>()), Times.Once);
    }

    [Fact]
    public async Task ReceiveWebhookEvent_WithMismatchedSignature_Returns401()
    {
        var controller = BuildController(webhookSecret: "shh", headerValue: "wrong");
        var payload = new GenericWebHookModel
        {
            Event = "mono.events.account_connected",
            Data = new AccountConnectedEventModel(),
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<UnauthorizedResult>(result);
        _consumer.Verify(c => c.HandleAccountCreatedEvent(It.IsAny<AccountConnectedEventModel>()), Times.Never);
    }

    [Fact]
    public async Task ReceiveWebhookEvent_WithMissingSignatureHeader_Returns401_WhenSecretConfigured()
    {
        var controller = BuildController(webhookSecret: "shh"); // no header set
        var payload = new GenericWebHookModel
        {
            Event = "mono.events.account_connected",
            Data = new AccountConnectedEventModel(),
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task ReceiveWebhookEvent_WithUnconfiguredSecret_AllowsRequestThrough()
    {
        var controller = BuildController(webhookSecret: null);
        var payload = new GenericWebHookModel
        {
            Event = "mono.events.account_connected",
            Data = new AccountConnectedEventModel(),
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<OkResult>(result);
    }

    // ============ Routing — the v1.7.0 bug fix ============

    [Fact]
    public async Task ReceiveWebhookEvent_MandateCreated_RoutesToMandateHandler_NotUnknown()
    {
        // Pre-v1.7.0 this fell through to HandleUnknownEvent because
        // Split('.')[2] returned "mandate" not "created".
        var controller = BuildController();
        var payload = new GenericWebHookModel
        {
            Event = "mono.events.mandate.created",
            Data = new MandateCreatedEventModel { Id = "mnd_1", Reference = "ref_1" },
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<OkResult>(result);
        _consumer.Verify(c => c.HandleMandateCreatedEvent(It.IsAny<MandateCreatedEventModel>()), Times.Once);
        _consumer.Verify(c => c.HandleUnknownEvent(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ReceiveWebhookEvent_AccountUpdated_RoutesCorrectly()
    {
        var controller = BuildController();
        var payload = new GenericWebHookModel
        {
            Event = "mono.events.account_updated",
            Data = new AccountUpdatedEventModel
            {
                WebhookId = "wh_1",
                Meta = new Meta { DataStatus = "AVAILABLE", AuthMethod = "internet_banking" },
                Account = new WebhookAccount
                {
                    Id = "acc_1",
                    AccountNumber = "0123456789",
                    Institution = new WebhookInstitution { Name = "Good Bank", BankCode = "044" },
                },
            },
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<OkResult>(result);
        _consumer.Verify(c => c.HandleAccountUpdatedEvent(It.IsAny<AccountUpdatedEventModel>()), Times.Once);
    }

    [Fact]
    public async Task ReceiveWebhookEvent_UnknownEventType_FallsThroughToHandleUnknown()
    {
        var controller = BuildController();
        var payload = new GenericWebHookModel
        {
            Event = "mono.events.nonexistent_thing",
            Data = new { foo = "bar" },
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<OkResult>(result);
        _consumer.Verify(c => c.HandleUnknownEvent(It.IsAny<string>()), Times.Once);
    }

    // ============ Optional IMonoWebhookConsumerExtensions dispatch ============

    [Fact]
    public async Task ReceiveWebhookEvent_DebitSuccessful_DispatchesToExtensionsWhenImplemented()
    {
        var combined = new Mock<IMonoWebhookConsumer>();
        var ext = combined.As<IMonoWebhookConsumerExtensions>();

        var options = Options.Create(new MonoInitializationOptions());
        var controller = new MonoWebhookController(combined.Object, _logger.Object, options);
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };

        var payload = new GenericWebHookModel
        {
            Event = "mono.events.mandate.debit.successful",
            Data = new MandateDebitEventModel { Id = "deb_1", Reference = "ref_1", Status = "successful" },
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<OkResult>(result);
        ext.Verify(e => e.HandleMandateDebitSuccessfulEvent(It.IsAny<MandateDebitEventModel>()), Times.Once);
        combined.Verify(c => c.HandleUnknownEvent(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task ReceiveWebhookEvent_DebitSuccessful_FallsThroughToUnknown_WhenExtensionsNotImplemented()
    {
        // Default consumer mock doesn't implement IMonoWebhookConsumerExtensions
        var controller = BuildController();
        var payload = new GenericWebHookModel
        {
            Event = "mono.events.mandate.debit.successful",
            Data = new MandateDebitEventModel(),
        };

        var result = await controller.ReceiveWebhookEvent(payload);

        Assert.IsType<OkResult>(result);
        _consumer.Verify(c => c.HandleUnknownEvent(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task ReceiveWebhookEvent_NullPayload_Returns400()
    {
        var controller = BuildController();

        var result = await controller.ReceiveWebhookEvent(null);

        Assert.IsType<BadRequestResult>(result);
    }
}

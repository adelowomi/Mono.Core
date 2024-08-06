using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mono.Core.Webhooks;
using Moq;
using Xunit;
using WebhookAccount = Mono.Core.Webhooks.Account;
using WebhookInstitution = Mono.Core.Webhooks.Institution;

namespace Mono.Core.Tests;

public class WebhookControllerTests
{
    private readonly MonoWebhookController _webhookController;
    private readonly Mock<IMonoWebhookConsumer> _webhookConsumerMock;
    private readonly Mock<ILogger<MonoWebhookController>> _loggerMock;

    public WebhookControllerTests()
    {
        _webhookConsumerMock = new Mock<IMonoWebhookConsumer>();
        _loggerMock = new Mock<ILogger<MonoWebhookController>>();
        _webhookController = new MonoWebhookController(_webhookConsumerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task HandleWebhook_WhenAccountCreatedEvent_ShouldReturnOk()
    {
        // Arrange
        var accountConnectedEventModel = new GenericWebHookModel
        {
            Event = "mono.events.account_connected",
            Data = new AccountConnectedEventModel
            {
                Id = "account_id",
                Customer = "customer_id"
            }
        };

        // Act
        var result = await _webhookController.ReceiveWebhookEvent(accountConnectedEventModel);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task HandleWebhook_WhenAccountUpdatedEvent_ShouldReturnOk()
    {
        // Arrange
        var accountUpdatedEventModel = new GenericWebHookModel
        {
            Event = "mono.events.webhook_test",
            Data = new AccountUpdatedEventModel
            {
                WebhookId = "webhook_id",
                App = "app",
                Business = "business",
                Meta = new Meta
                {
                    DataStatus = "data_status",
                    AuthMethod = "auth_method"
                },
                Account = new WebhookAccount
                {
                    Institution = new WebhookInstitution
                    {
                        Name = "name",
                        //BankCode = "bank_code",
                        Type = "type"
                    }
                }
            }
        };

        // Act
        var result = await _webhookController.ReceiveWebhookEvent(accountUpdatedEventModel);

        // Assert
        Assert.IsType<OkResult>(result);
    }
}

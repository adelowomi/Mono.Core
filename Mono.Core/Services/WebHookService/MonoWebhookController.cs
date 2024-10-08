﻿using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mono.Core.Webhooks;

namespace Mono.Core.Webhooks
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonoWebhookController : ControllerBase
    {
        private readonly IMonoWebhookConsumer _webhookConsumer;
        private readonly ILogger<MonoWebhookController> _logger;
        private string _eventType;

        public MonoWebhookController(IMonoWebhookConsumer webhookConsumer, ILogger<MonoWebhookController> logger)
        {
            _webhookConsumer = webhookConsumer;
            _logger = logger;
        }

        [HttpPost("receive", Name = nameof(ReceiveWebhookEvent))]
        public async Task<IActionResult> ReceiveWebhookEvent([FromBody] GenericWebHookModel payload)
        {
            var serializedPayload = JsonSerializer.Serialize(payload);
            Console.WriteLine("Webhook received");
            Console.WriteLine($"Webhook data: {serializedPayload}");
            Console.WriteLine($"Event type: {payload.Event}");
            _eventType = payload.Event;
            if (string.IsNullOrEmpty(serializedPayload)) return BadRequest();
            var webhook = JsonSerializer.Deserialize<MonoWebhookModel<dynamic>>(serializedPayload);

            var eventType = GetEventType();
            switch (eventType)
            {
                case MonoEventTypes.WebhookTestEvent:
                    HandleTestEvent(webhook);
                    break;
                case MonoEventTypes.AccountConnected:
                    Console.WriteLine($"Webhook data before it is parsed: {serializedPayload}");
                    var accountConnectEvent = JsonSerializer.Deserialize<MonoWebhookModel<AccountConnectedEventModel>>(serializedPayload);
                    Console.WriteLine($"Deserialized webhook data: {accountConnectEvent}");
                    Console.WriteLine($"Deserialized webhook data: {JsonSerializer.Serialize(accountConnectEvent)}");
                    await _webhookConsumer.HandleAccountCreatedEvent(accountConnectEvent.Data);
                    break;
                case MonoEventTypes.AccountUpdated:
                    var accountUpdatedEvent = JsonSerializer.Deserialize<MonoWebhookModel<AccountUpdatedEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleAccountUpdatedEvent(accountUpdatedEvent.Data);
                    break;
                case MonoEventTypes.MandateCreated:
                    var mandateCreatedEvent = JsonSerializer.Deserialize<MonoWebhookModel<MandateCreatedEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleMandateCreatedEvent(mandateCreatedEvent.Data);
                    break;
                case MonoEventTypes.MandateApproved:
                    var mandateApprovedEvent = JsonSerializer.Deserialize<MonoWebhookModel<MandateApprovedEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleMandateApprovedEvent(mandateApprovedEvent.Data);
                    break;
                case MonoEventTypes.MandateReady:
                    var mandateReadyEvent = JsonSerializer.Deserialize<MonoWebhookModel<MandateReadyEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleMandateReadyEvent(mandateReadyEvent.Data);
                    break;
                default:
                    await _webhookConsumer.HandleUnknownEvent(serializedPayload);
                    break;
            }
            return Ok();
        }

        private void HandleTestEvent(MonoWebhookModel<dynamic> webhook)
        {
            Console.WriteLine("Hurray! Webhook test event received");
            Console.WriteLine($"Event type: {GetEventType()}");
            Console.WriteLine($"Webhook data: {JsonSerializer.Serialize(webhook)}");
            Console.WriteLine("Webhook test event received");
        }

        private string GetEventType()
        {
            var checker = _eventType.Split('.')[2];
            if (_eventType.Contains("mandate"))
            {
                switch (checker)
                {
                    case MonoEventTypes.WebhookTestEvent:
                        return MonoEventTypes.WebhookTestEvent;
                    case MonoEventTypes.MandateCreated:
                        return MonoEventTypes.MandateCreated;
                    case MonoEventTypes.MandateApproved:
                        return MonoEventTypes.MandateApproved;
                    case MonoEventTypes.MandateReady:
                        return MonoEventTypes.MandateReady;
                    default:
                        return MonoEventTypes.Unknown;
                }
            }

            switch (checker)
            {
                case MonoEventTypes.WebhookTestEvent:
                    return MonoEventTypes.WebhookTestEvent;
                case MonoEventTypes.AccountConnected:
                    return MonoEventTypes.AccountConnected;
                case MonoEventTypes.AccountUpdated:
                    return MonoEventTypes.AccountUpdated;
                default:
                    return MonoEventTypes.Unknown;
            }
        }
    }

    public class GenericWebHookModel
    {
        public string Event { get; set; }
        public object Data { get; set; }
    }
}

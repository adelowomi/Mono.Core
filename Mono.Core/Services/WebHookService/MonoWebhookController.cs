using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mono.Core.Webhooks;

namespace Mono.Core.Webhooks
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonoWebhookController : ControllerBase
    {
        private const string SignatureHeader = "mono-webhook-secret";
        private const string EventPrefix = "mono.events.";

        private readonly IMonoWebhookConsumer _webhookConsumer;
        private readonly ILogger<MonoWebhookController> _logger;
        private readonly MonoInitializationOptions _options;

        public MonoWebhookController(
            IMonoWebhookConsumer webhookConsumer,
            ILogger<MonoWebhookController> logger,
            IOptions<MonoInitializationOptions> options)
        {
            _webhookConsumer = webhookConsumer;
            _logger = logger;
            _options = options?.Value ?? new MonoInitializationOptions();
        }

        [HttpPost("receive", Name = nameof(ReceiveWebhookEvent))]
        public async Task<IActionResult> ReceiveWebhookEvent([FromBody] GenericWebHookModel payload)
        {
            if (!IsAuthenticated())
            {
                _logger.LogWarning("Rejected Mono webhook: mono-webhook-secret header missing or mismatched.");
                return Unauthorized();
            }

            if (payload == null || string.IsNullOrEmpty(payload.Event))
            {
                return BadRequest();
            }

            var serializedPayload = JsonSerializer.Serialize(payload);
            var eventType = ParseEventType(payload.Event);
            _logger.LogInformation("Mono webhook received: event={Event} eventType={EventType}", payload.Event, eventType);

            var extensions = _webhookConsumer as IMonoWebhookConsumerExtensions;

            switch (eventType)
            {
                case MonoEventTypes.WebhookTestEvent:
                    _logger.LogInformation("Mono webhook test event received.");
                    break;

                case MonoEventTypes.AccountConnected:
                {
                    var ev = JsonSerializer.Deserialize<MonoWebhookModel<AccountConnectedEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleAccountCreatedEvent(ev?.Data);
                    break;
                }

                case MonoEventTypes.AccountUpdated:
                {
                    var ev = JsonSerializer.Deserialize<MonoWebhookModel<AccountUpdatedEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleAccountUpdatedEvent(ev?.Data);
                    break;
                }

                case MonoEventTypes.MandateCreated:
                {
                    var ev = JsonSerializer.Deserialize<MonoWebhookModel<MandateCreatedEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleMandateCreatedEvent(ev?.Data);
                    break;
                }

                case MonoEventTypes.MandateApproved:
                {
                    var ev = JsonSerializer.Deserialize<MonoWebhookModel<MandateApprovedEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleMandateApprovedEvent(ev?.Data);
                    break;
                }

                case MonoEventTypes.MandateReady:
                {
                    var ev = JsonSerializer.Deserialize<MonoWebhookModel<MandateReadyEventModel>>(serializedPayload);
                    await _webhookConsumer.HandleMandateReadyEvent(ev?.Data);
                    break;
                }

                case MonoEventTypes.MandateDebitSuccessful:
                    await DispatchExtension(extensions, serializedPayload, (e, p) =>
                    {
                        var ev = JsonSerializer.Deserialize<MonoWebhookModel<MandateDebitEventModel>>(p);
                        return e.HandleMandateDebitSuccessfulEvent(ev?.Data);
                    });
                    break;

                case MonoEventTypes.MandateDebitFailed:
                    await DispatchExtension(extensions, serializedPayload, (e, p) =>
                    {
                        var ev = JsonSerializer.Deserialize<MonoWebhookModel<MandateDebitEventModel>>(p);
                        return e.HandleMandateDebitFailedEvent(ev?.Data);
                    });
                    break;

                case MonoEventTypes.AccountIncome:
                    await DispatchExtension(extensions, serializedPayload, (e, p) =>
                    {
                        var ev = JsonSerializer.Deserialize<MonoWebhookModel<AccountIncomeEventModel>>(p);
                        return e.HandleAccountIncomeEvent(ev?.Data);
                    });
                    break;

                case MonoEventTypes.Creditworthiness:
                    await DispatchExtension(extensions, serializedPayload, (e, p) =>
                    {
                        var ev = JsonSerializer.Deserialize<MonoWebhookModel<CreditworthinessEventModel>>(p);
                        return e.HandleCreditworthinessEvent(ev?.Data);
                    });
                    break;

                case MonoEventTypes.DisbursementInitiated:
                case MonoEventTypes.DisbursementProcessing:
                case MonoEventTypes.DisbursementCompleted:
                case MonoEventTypes.DisbursementFailed:
                    await DispatchExtension(extensions, serializedPayload, (e, p) =>
                    {
                        var ev = JsonSerializer.Deserialize<MonoWebhookModel<DisbursementEventModel>>(p);
                        return e.HandleDisbursementEvent(ev?.Data);
                    });
                    break;

                case MonoEventTypes.WatchlistMatchFound:
                case MonoEventTypes.WatchlistMonitoringUpdate:
                    await DispatchExtension(extensions, serializedPayload, (e, p) =>
                    {
                        var ev = JsonSerializer.Deserialize<MonoWebhookModel<WatchlistMatchEventModel>>(p);
                        return e.HandleWatchlistMatchEvent(ev?.Data);
                    });
                    break;

                case MonoEventTypes.ProveCompleted:
                    await DispatchExtension(extensions, serializedPayload, (e, p) =>
                    {
                        var ev = JsonSerializer.Deserialize<MonoWebhookModel<ProveEventModel>>(p);
                        return e.HandleProveCompletedEvent(ev?.Data);
                    });
                    break;

                case MonoEventTypes.ProveFailed:
                    await DispatchExtension(extensions, serializedPayload, (e, p) =>
                    {
                        var ev = JsonSerializer.Deserialize<MonoWebhookModel<ProveEventModel>>(p);
                        return e.HandleProveFailedEvent(ev?.Data);
                    });
                    break;

                default:
                    await _webhookConsumer.HandleUnknownEvent(serializedPayload);
                    break;
            }

            return Ok();
        }

        private bool IsAuthenticated()
        {
            var configured = _options.WebhookSecret;
            if (string.IsNullOrEmpty(configured))
            {
                _logger.LogWarning(
                    "MonoInitializationOptions.WebhookSecret is not configured; webhook signature verification is disabled. " +
                    "Set the WebhookSecret to enforce verification of the mono-webhook-secret header.");
                return true;
            }

            if (!Request.Headers.TryGetValue(SignatureHeader, out var header))
            {
                return false;
            }

            return FixedTimeEquals(header.ToString(), configured);
        }

        /// <summary>
        /// Strips the <c>mono.events.</c> prefix from the raw event string so the
        /// remainder matches the <see cref="MonoEventTypes"/> constants.
        /// </summary>
        private static string ParseEventType(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return MonoEventTypes.Unknown;
            return raw.StartsWith(EventPrefix, StringComparison.Ordinal)
                ? raw.Substring(EventPrefix.Length)
                : raw;
        }

        private async Task DispatchExtension(
            IMonoWebhookConsumerExtensions extensions,
            string serializedPayload,
            Func<IMonoWebhookConsumerExtensions, string, Task> handler)
        {
            if (extensions == null)
            {
                await _webhookConsumer.HandleUnknownEvent(serializedPayload);
                return;
            }

            await handler(extensions, serializedPayload);
        }

        /// <summary>
        /// Constant-time string comparison. netstandard2.0 doesn't have
        /// <c>CryptographicOperations.FixedTimeEquals</c>, so we roll our own.
        /// </summary>
        private static bool FixedTimeEquals(string a, string b)
        {
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;
            int diff = 0;
            for (int i = 0; i < a.Length; i++)
            {
                diff |= a[i] ^ b[i];
            }
            return diff == 0;
        }
    }

    public class GenericWebHookModel
    {
        public string Event { get; set; }
        public object Data { get; set; }
    }
}

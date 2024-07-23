using System;
using System.Threading.Tasks;
using Mono.Core.Webhooks;

namespace Mono.Core
{
    public interface IMonoWebhookConsumer
    {
        Task HandleAccountCreatedEvent(AccountConnectedEventModel webhook);
        Task HandleAccountUpdatedEvent(AccountUpdatedEventModel webhook);
        Task HandleUnknownEvent(string json);
    }
}

using System;
using System.Threading.Tasks;
using Mono.Core.Webhooks;

namespace Mono.Core
{
    public interface IMonoWebhookConsumer
    {
        Task HandleAccountCreatedEvent(AccountConnectedEventModel webhook);
        Task HandleAccountUpdatedEvent(AccountUpdatedEventModel webhook);
        Task HandleMandateCreatedEvent(MandateCreatedEventModel webhook);
        Task HandleMandateApprovedEvent(MandateApprovedEventModel webhook);
        Task HandleMandateReadyEvent(MandateReadyEventModel webhook); 

        Task HandleUnknownEvent(string json);
    }
}

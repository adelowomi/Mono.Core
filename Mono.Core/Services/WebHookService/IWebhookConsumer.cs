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

    /// <summary>
    /// Optional handler surface for the newer Mono webhook events
    /// (debit.successful / debit.failed, account_income, creditworthiness,
    /// disbursement.*, watchlist.match_found, prove.completed / prove.failed).
    ///
    /// Implement this interface on the same class as <see cref="IMonoWebhookConsumer"/>;
    /// the controller will dispatch automatically. Consumers that don't implement
    /// it receive these events via <see cref="IMonoWebhookConsumer.HandleUnknownEvent"/>
    /// — exactly the same behavior as before this interface existed.
    /// </summary>
    public interface IMonoWebhookConsumerExtensions
    {
        Task HandleMandateDebitSuccessfulEvent(MandateDebitEventModel webhook);
        Task HandleMandateDebitFailedEvent(MandateDebitEventModel webhook);
        Task HandleAccountIncomeEvent(AccountIncomeEventModel webhook);
        Task HandleCreditworthinessEvent(CreditworthinessEventModel webhook);
        Task HandleDisbursementEvent(DisbursementEventModel webhook);
        Task HandleWatchlistMatchEvent(WatchlistMatchEventModel webhook);
        Task HandleProveCompletedEvent(ProveEventModel webhook);
        Task HandleProveFailedEvent(ProveEventModel webhook);
    }
}

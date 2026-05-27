namespace Mono.Core
{
    public class MonoInitializationOptions
    {
        public string SecretKey { get; set; }
        public string BaseUrl { get; set; }

        /// <summary>
        /// Secret for the Connect product app — used by IMonoAccounts and
        /// IMonoCustomers. Falls back to <see cref="SecretKey"/> if unset.
        /// </summary>
        public string ConnectSecretKey { get; set; }

        /// <summary>
        /// Secret for the Lookup product app — used by IMonoLookUp and
        /// IMonoWatchlist. Falls back to <see cref="SecretKey"/> if unset.
        /// </summary>
        public string LookupSecretKey { get; set; }

        /// <summary>
        /// Secret for the DirectPay product app — used by IMonoDirectPay.
        /// Falls back to <see cref="SecretKey"/> if unset.
        /// </summary>
        public string DirectPaySecretKey { get; set; }

        /// <summary>
        /// Secret for the Disburse product app — used by IMonoDisburse.
        /// Falls back to <see cref="SecretKey"/> if unset.
        /// </summary>
        public string DisburseSecretKey { get; set; }

        /// <summary>
        /// Secret for the Prove product app — used by IMonoProve.
        /// Falls back to <see cref="SecretKey"/> if unset.
        /// </summary>
        public string ProveSecretKey { get; set; }

        /// <summary>
        /// Shared secret configured on the Mono webhook dashboard. When set,
        /// <see cref="Mono.Core.Webhooks.MonoWebhookController"/> rejects any
        /// inbound POST whose <c>mono-webhook-secret</c> header doesn't match
        /// (HTTP 401). When null or empty the controller logs a warning and
        /// processes the request anyway — opt-in for backward compatibility,
        /// but you should set this in production.
        /// </summary>
        public string WebhookSecret { get; set; }
    }
}

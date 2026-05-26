namespace Mono.Core
{
    public class MonoInitializationOptions
    {
        public string SecretKey { get; set; }
        public string BaseUrl { get; set; }
        public string ConnectSecretKey { get; set; }
        public string LookupSecretKey { get; set; }

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

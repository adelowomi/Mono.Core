using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Refit;

namespace Mono.Core
{
    public class RefitClientBuilder<T> : IRefitClientBuilder<T>
    {
        private readonly MonoInitializationOptions _options;

        public RefitClientBuilder(IOptions<MonoInitializationOptions> options)
        {
            _options = options.Value;
        }

        public T Build(string serviceType = null) => BuildForBaseUrl(_options.BaseUrl, serviceType);

        // change the url of the client to replace "v2" with "v3"
        public T BuildV3(string serviceType = null) => BuildForBaseUrl(_options.BaseUrl?.Replace("v2", "v3"), serviceType);

        // change the url of the client to replace "v2" with "v1" — used by
        // the Prove API, which still lives under /v1/
        public T BuildV1(string serviceType = null) => BuildForBaseUrl(_options.BaseUrl?.Replace("v2", "v1"), serviceType);

        private T BuildForBaseUrl(string baseUrl, string serviceType)
        {
            var secretKey = ResolveSecretKey(serviceType);
            var client = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = new Uri(baseUrl),
            };
            client.DefaultRequestHeaders.Add("mono-sec-key", secretKey);
            var builder = RequestBuilder.ForType<T>(new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                        WriteIndented = true,
                    }),
            });
            return RestService.For(client, builder);
        }

        /// <summary>
        /// Picks the right secret key for the given service type. Each named
        /// type falls back to <see cref="MonoInitializationOptions.SecretKey"/>
        /// when its product-scoped override is unset. Unknown service types
        /// throw — callers should use one of the <see cref="ServiceTypes"/>
        /// constants or pass null/empty for the default.
        /// </summary>
        private string ResolveSecretKey(string serviceType)
        {
            if (string.IsNullOrEmpty(serviceType)) return _options.SecretKey;
            switch (serviceType)
            {
                case ServiceTypes.Connect:    return _options.ConnectSecretKey ?? _options.SecretKey;
                case ServiceTypes.Lookup:     return _options.LookupSecretKey ?? _options.SecretKey;
                case ServiceTypes.DirectPay:  return _options.DirectPaySecretKey ?? _options.SecretKey;
                case ServiceTypes.Disburse:   return _options.DisburseSecretKey ?? _options.SecretKey;
                case ServiceTypes.Prove:      return _options.ProveSecretKey ?? _options.SecretKey;
                default: throw new ArgumentException($"Invalid service type: '{serviceType}'", nameof(serviceType));
            }
        }
    }

    public interface IRefitClientBuilder<T>
    {
        T Build(string serviceType = null);
        T BuildV3(string serviceType = null);
        T BuildV1(string serviceType = null);
    }

    public class ServiceTypes
    {
        public const string Connect = "connect";
        public const string Lookup = "lookup";
        public const string DirectPay = "directpay";
        public const string Disburse = "disburse";
        public const string Prove = "prove";
    }
}

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

        public T Build(string serviceType = null)
        {
            //  if the service type is not specified, use the default secret key else check that the dedicated key has a value and use it instead
            var secretKey = string.IsNullOrEmpty(serviceType) ? _options.SecretKey : 
                            serviceType == "connect" ? _options.ConnectSecretKey ?? _options.SecretKey : 
                            serviceType == "lookup" ? _options.LookupSecretKey ?? _options.SecretKey : 
                            throw new ArgumentException("Invalid service type");

            var client = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = new Uri(_options.BaseUrl)
            };
            client.DefaultRequestHeaders.Add("mono-sec-key", secretKey);
            var buider = RequestBuilder.ForType<T>(new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    }
                )
            });
            return RestService.For(client, buider);
        }

        // change the url of the client to replace "v2" with "v3"
        public T BuildV3(string serviceType = null)
        {
            var secretKey = string.IsNullOrEmpty(serviceType) ? _options.SecretKey : 
                            serviceType == "connect" ? _options.ConnectSecretKey ?? _options.SecretKey : 
                            serviceType == "lookup" ? _options.LookupSecretKey ?? _options.SecretKey : 
                            throw new ArgumentException("Invalid service type");

            var client = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = new Uri(_options.BaseUrl.Replace("v2", "v3"))
            };
            client.DefaultRequestHeaders.Add("mono-sec-key", secretKey);
            var buider = RequestBuilder.ForType<T>(new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(
                    new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
                        WriteIndented = true
                    }
                )
            });
            return RestService.For(client, buider);
        }
    }

    public interface IRefitClientBuilder<T>
    {
        T Build(string serviceType = null);
        T BuildV3(string serviceType = null);
    }

    public class ServiceTypes
    {
        public const string Connect = "connect";
        public const string Lookup = "lookup";
    }
}

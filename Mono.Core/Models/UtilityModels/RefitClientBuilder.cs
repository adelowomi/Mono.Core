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

        public T Build()
        {
            var client = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = new Uri(_options.BaseUrl)
            };
            client.DefaultRequestHeaders.Add("mono-sec-key", _options.SecretKey);
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
        public T BuildV3()
        {
            var client = new HttpClient(new HttpClientHandler())
            {
                BaseAddress = new Uri(_options.BaseUrl.Replace("v2", "v3"))
            };
            client.DefaultRequestHeaders.Add("mono-sec-key", _options.SecretKey);
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
        T Build();
        T BuildV3();
    }
}

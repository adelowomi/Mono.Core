using System;
using System.Text.Json;
using Refit;

namespace Mono.Core
{
    public static class RefitExtensions
    {
        public static MonoStandardResponse<T> HandleResponse<T>(this IApiResponse<MonoStandardResponse<T>> response)
        {
            if (!response.IsSuccessStatusCode)
            {
                // deserialize error response and populate the response details
                var JsonResponse = JsonSerializer.Deserialize<MonoStandardResponse<T>>(response.Error.Content);
                return JsonResponse;
            }

            if (response.IsSuccessStatusCode)
            {
                return response.Content;
            }

            return MonoStandardResponse<T>.Error(response.Content.Message);
        }
    }
}

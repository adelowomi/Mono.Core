using System;
using System.Text.Json;
using Refit;

namespace Mono.Core
{
    public static class RefitExtensions
    {
        /// <summary>
        /// Maps a Refit <see cref="IApiResponse{T}"/> into our normalised
        /// <see cref="MonoStandardResponse{T}"/> envelope. Tolerant of every
        /// shape Mono can return — empty bodies, malformed error envelopes,
        /// 5xx with no JSON, network failures — so consumers never see a
        /// <see cref="NullReferenceException"/> bubble up from this layer.
        /// </summary>
        public static MonoStandardResponse<T> HandleResponse<T>(this IApiResponse<MonoStandardResponse<T>> response)
        {
            if (response == null)
                return MonoStandardResponse<T>.Error("No response from Mono");

            if (!response.IsSuccessStatusCode)
            {
                // Refit captures the body in response.Error.Content. It can be
                // null when the server sent no body (e.g. 504 from a proxy) and
                // it can be a non-JSON string (HTML error page from an upstream).
                // Try to parse; on any failure fall through to a typed error
                // envelope so the caller still gets a usable response.
                var errorBody = response.Error?.Content;
                if (!string.IsNullOrEmpty(errorBody))
                {
                    try
                    {
                        var parsed = JsonSerializer.Deserialize<MonoStandardResponse<T>>(errorBody);
                        if (parsed != null)
                        {
                            parsed.Success = false;
                            // Surface the raw body in Message when the parsed
                            // envelope didn't include one — keeps debugging cheap.
                            if (string.IsNullOrEmpty(parsed.Message))
                                parsed.Message = $"{(int)response.StatusCode} {response.StatusCode}";
                            return parsed;
                        }
                    }
                    catch (JsonException)
                    {
                        // Body was non-JSON. Fall through.
                    }
                }

                var message = response.Error?.Message
                    ?? $"Mono returned {(int)response.StatusCode} {response.StatusCode} with no body";
                return MonoStandardResponse<T>.Error(message);
            }

            // Success path — but the body can still be null if Mono returned
            // 200 No Content or an empty JSON `null`. Refuse to NRE on .Content.
            if (response.Content == null)
                return MonoStandardResponse<T>.Error("Mono returned an empty response body");

            response.Content.Success = true;
            return response.Content;
        }
    }
}

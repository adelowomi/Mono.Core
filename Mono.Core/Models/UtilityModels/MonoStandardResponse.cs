﻿using System;
using System.Text.Json.Serialization;

namespace Mono.Core
{
    public class MonoStandardResponse<T>
    {
        public bool Success { get; set; }
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("data")]
        public T Data { get; set; }
        public Exception InAppErrors { get; set; }
        [JsonPropertyName("errors")]
        public MonoErrors[] MonoErrors { get; set; }

        public static MonoStandardResponse<T> Ok(T data)
        {
            return new MonoStandardResponse<T>
            {
                Success = true,
                Data = data
            };
        }

        public static MonoStandardResponse<T> Error(string message)
        {
            return new MonoStandardResponse<T>
            {
                Success = false,
                Message = message
            };
        }

        public static MonoStandardResponse<T> Error(Exception exception)
        {
            return new MonoStandardResponse<T>
            {
                Success = false,
                InAppErrors = exception
            };
        }

        public static MonoStandardResponse<T> Error(MonoErrors[] errors, string message)
        {
            return new MonoStandardResponse<T>
            {
                Success = false,
                MonoErrors = errors,
                Message = message
            };
        }

        public static MonoStandardResponse<T> Error(string message, T data)
        {
            return new MonoStandardResponse<T>
            {
                Success = false,
                Message = message,
                Data = data
            };
        }

        public static MonoStandardResponse<T> Error(string message, DateTime timestamp, string status)
        {
            return new MonoStandardResponse<T>
            {
                Success = false,
                Message = message,
                Timestamp = timestamp,
                Status = status
            };
        }
    }

    public class MonoStandardPaginatedResponse
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("page")]
        public long Page { get; set; }

        [JsonPropertyName("previous")]
        public string Previous { get; set; }

        [JsonPropertyName("next")]
        public string Next { get; set; }
    }

     public class MonoErrors
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}

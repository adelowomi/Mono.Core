using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mono.Core.Watchlist
{
    // ============ Submit screening ============

    public class SubmitIndividualScreeningModel
    {
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = WatchlistSubjectTypeConstants.Individual;

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [Required]
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    public class SubmitEntityScreeningModel
    {
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; } = WatchlistSubjectTypeConstants.Entity;

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [Required]
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    /// <summary>
    /// Flat shape used inside batch requests. Populate the fields that match
    /// the <see cref="Type"/> ("individual" or "entity") — irrelevant fields can
    /// remain null and will be omitted from the serialized JSON if the calling
    /// serializer skips nulls, or sent as nulls otherwise.
    /// </summary>
    public class WatchlistScreeningSubject
    {
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("country")]
        public string Country { get; set; }

        // Individual-only
        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        // Entity-only
        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public class BatchScreeningModel
    {
        [Required]
        [JsonPropertyName("entries")]
        public List<WatchlistScreeningSubject> Entries { get; set; }
    }

    // ============ Monitoring ============

    public class StartMonitoringModel
    {
        [Required]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [Required]
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    // ============ Responses ============

    public class WatchlistMatchSource
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    public class WatchlistMatch
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("match_score")]
        public double? MatchScore { get; set; }

        [JsonPropertyName("match_level")]
        public string MatchLevel { get; set; }

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; }

        [JsonPropertyName("aliases")]
        public List<string> Aliases { get; set; }

        [JsonPropertyName("source")]
        public WatchlistMatchSource Source { get; set; }

        [JsonPropertyName("listed_on")]
        public DateTime? ListedOn { get; set; }
    }

    public class ScreeningSubjectSummary
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public class ScreeningResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("subject")]
        public ScreeningSubjectSummary Subject { get; set; }

        [JsonPropertyName("risk_score")]
        public double? RiskScore { get; set; }

        [JsonPropertyName("risk_level")]
        public string RiskLevel { get; set; }

        [JsonPropertyName("matches")]
        public List<WatchlistMatch> Matches { get; set; }

        [JsonPropertyName("monitoring")]
        public bool? Monitoring { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class BatchScreeningResponse
    {
        [JsonPropertyName("results")]
        public List<ScreeningResponse> Results { get; set; }
    }

    public class AuditLogEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("actor")]
        public string Actor { get; set; }

        [JsonPropertyName("details")]
        public object Details { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }
    }

    public class AuditLogResponse
    {
        [JsonPropertyName("entries")]
        public List<AuditLogEntry> Entries { get; set; }
    }

    public class MonitoringResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("subject")]
        public ScreeningSubjectSummary Subject { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("monitoring")]
        public bool? Monitoring { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

    // ============ Constants ============

    public class WatchlistSubjectTypeConstants
    {
        public const string Individual = "individual";
        public const string Entity = "entity";
    }

    public class ScreeningStatusConstants
    {
        public const string Processing = "processing";
        public const string Completed = "completed";
        public const string Failed = "failed";
    }

    public class RiskLevelConstants
    {
        public const string Low = "low";
        public const string Medium = "medium";
        public const string High = "high";
    }
}

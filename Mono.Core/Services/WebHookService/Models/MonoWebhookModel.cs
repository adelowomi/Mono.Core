﻿using System;
using System.Text.Json.Serialization;

namespace Mono.Core.Webhooks
{
    public class Account
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("institution")]
        public Institution Institution { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bvn")]
        public string Bvn { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("balance")]
        public int Balance { get; set; }

        [JsonPropertyName("liveMode")]
        public bool LiveMode { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

    public class AccountUpdatedEventModel
    {
        [JsonPropertyName("webhook_id")]
        public string WebhookId { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }

        [JsonPropertyName("meta")]
        public Meta Meta { get; set; }

        [JsonPropertyName("account")]
        public Account Account { get; set; }
    }

    public class Institution
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("bankCode")]
        public string BankCode { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class Meta
    {
        [JsonPropertyName("data_status")]
        public string DataStatus { get; set; }
        [JsonPropertyName("auth_method")]
        public string AuthMethod { get; set; }
    }

    public class MonoWebhookModel<T>
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }
        [JsonPropertyName("Data")]
        public T Data { get; set; }
    }

    public class AccountConnectedEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("customer")]
        public string Customer { get; set; }
    }

    public class MonoEventTypes
    {
        public const string WebhookTestEvent = "webhook_test";
        public const string AccountConnected = "account_connected";
        public const string AccountUpdated = "account_updated";
        public const string Unknown = "unknown";
        public const string MandateCreated = "created";
        public const string MandateReady = "ready";
        public const string MandateApproved = "approved";

    }

    public class MandateCreatedEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("ready_to_debit")]
        public bool? ReadyToDebit { get; set; }

        [JsonPropertyName("nibss_code")]
        public string NibssCode { get; set; }

        [JsonPropertyName("approved")]
        public bool? Approved { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank")]
        public string Bank { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }
    }
    public class MandateApprovedEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("ready_to_debit")]
        public bool? ReadyToDebit { get; set; }

        [JsonPropertyName("nibss_code")]
        public string NibssCode { get; set; }

        [JsonPropertyName("approved")]
        public bool? Approved { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank")]
        public string Bank { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("live_mode")]
        public bool? LiveMode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("date")]
        public DateTime? Date { get; set; }

        [JsonPropertyName("amount")]
        public int? Amount { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }
    }
    public class MandateReadyEventModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("mandate_type")]
        public string MandateType { get; set; }

        [JsonPropertyName("debit_type")]
        public string DebitType { get; set; }

        [JsonPropertyName("ready_to_debit")]
        public bool ReadyToDebit { get; set; }

        [JsonPropertyName("nibss_code")]
        public string NibssCode { get; set; }

        [JsonPropertyName("approved")]
        public bool Approved { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("account_name")]
        public string AccountName { get; set; }

        [JsonPropertyName("account_number")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("bank")]
        public string Bank { get; set; }

        [JsonPropertyName("bank_code")]
        public string BankCode { get; set; }

        [JsonPropertyName("customer")]
        public string Customer { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("live_mode")]
        public bool LiveMode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("app")]
        public string App { get; set; }

        [JsonPropertyName("business")]
        public string Business { get; set; }
    }
}
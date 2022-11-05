/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;
using System.Collections.Generic;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookPaginationResultResponse<T> where T : class
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("path")]
        public string[] Path { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("extensions")]
        public FacebookPageResultExtensionsResponse Extensions { get; set; }
        [JsonProperty("errors")]
        public List<FacebookErrorResult> Errors = new List<FacebookErrorResult>();
    }
    public class FacebookPaginationResultResponse
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("path")]
        public string[] Path { get; set; }
        [JsonProperty("data")]
        public FacebookPageResultDataResponse Data { get; set; }
        [JsonProperty("extensions")]
        public FacebookPageResultExtensionsResponse Extensions { get; set; }
    }

    public class FacebookPageResultDataResponse
    {
        [JsonProperty("page_info")]
        public FacebookPageInfoResponse PageInfo { get; set; }
    }

    public class FacebookPageInfoResponse
    {
        [JsonProperty("start_cursor")]
        public string StartCursor { get; set; }
        [JsonProperty("end_cursor")]
        public string EndCursor { get; set; }
        [JsonProperty("has_previous_page")]
        public bool HasPreviousPage { get; set; }
        [JsonProperty("has_next_page")]
        public bool HasNextPage { get; set; }
    }

    public class FacebookPageResultExtensionsResponse
    {
        [JsonProperty("is_final")]
        public bool IsFinal { get; set; }
        [JsonProperty("fulfilled_payloads")]
        public FacebookPageResultExtensionsFulfilledPayloadsResponse[] FulfilledPayloads { get; set; }
        [JsonProperty("server_metadata")]
        public FacebookPageResultExtensionsServerMetadataResponse ServerMetadata { get; set; }
    }

    public class FacebookPageResultExtensionsServerMetadataResponse
    {
        [JsonProperty("request_start_time_ms")]
        public long RequestStartTimeMs { get; set; }
        [JsonProperty("time_at_flush_ms")]
        public long TimeAtFlushMs { get; set; }
    }

    public class FacebookPageResultExtensionsFulfilledPayloadsResponse
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("path")]
        public object[] Path { get; set; }
    }

    public class FacebookErrorResultContainer
    {
        [JsonProperty("error")]
        public FacebookErrorResult Error { get; set; }
    }
    public class FacebookErrorResult
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("code")]
        public int? Code { get; set; }

        [JsonProperty("api_error_code")]
        public int? ApiErrorCode { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        //[JsonProperty("description_raw")]
        //public DescriptionRaw DescriptionRaw { get; set; }

        [JsonProperty("is_silent")]
        public bool? IsSilent { get; set; }

        [JsonProperty("is_transient")]
        public bool? IsTransient { get; set; }

        [JsonProperty("requires_reauth")]
        public bool? RequiresReauth { get; set; }

        [JsonProperty("allow_user_retry")]
        public bool? AllowUserRetry { get; set; }

        [JsonProperty("debug_info")]
        public object DebugInfo { get; set; }

        [JsonProperty("query_path")]
        public object QueryPath { get; set; }

        [JsonProperty("fbtrace_id")]
        public string FbtraceId { get; set; }

        [JsonProperty("www_request_id")]
        public string WwwRequestId { get; set; }

        [JsonProperty("sentry_block_user_info")]
        public FacebookErrortSentryBlockUserInfo SentryBlockUserInfo { get; set; }

        [JsonProperty("path")]
        public List<string> Path { get; set; } = new List<string>();
        [JsonProperty("type")]
        public string Type { get; set; }
    }
    public class FacebookErrortSentryBlockUserInfo
    {
        [JsonProperty("sentry_block_data")]
        public string SentryBlockData { get; set; }
    }
    //public class DescriptionRaw
    //{
    //    [JsonProperty("__html")]
    //    public string Html { get; set; }
    //}


}

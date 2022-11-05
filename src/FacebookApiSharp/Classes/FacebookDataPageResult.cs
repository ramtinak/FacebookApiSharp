/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookDataPageResultList : List<FacebookDataPageResult> { }

    public class FacebookDataPageResult
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("path")]
        public object[] Path { get; set; }
        [JsonProperty("data")]
        public Data Data { get; set; }
        [JsonProperty("extensions")]
        public FacebookPageResultExtensionsResponse Extensions { get; set; }
    }

    public class Data
    {
        [JsonProperty("logging_unit_id")]
        public string LoggingUnitId { get; set; }
        [JsonProperty("native_template_view")]
        public FacebookNativeTemplateView NativeTemplateView { get; set; }
        [JsonProperty("native_template_result_ids")]
        public string[] NativeTemplateResultIds { get; set; }
    }

    public class FacebookNativeTemplateView
    {
        [JsonProperty("logging_id")]
        public string LoggingId { get; set; }
        [JsonProperty("data_diff_item_id")]
        public string DataDiffItemId { get; set; }
        [JsonProperty("data_diff_content_id")]
        public string DataDiffContentId { get; set; }
        [JsonProperty("unique_id")]
        public string UniqueId { get; set; }
        [JsonProperty("native_template_bundles")]
        public FacebookNativeTemplateBundles[] NativeTemplateBundles { get; set; }
    }

    public class FacebookNativeTemplateBundles
    {
        [JsonProperty("nt_bundle_tree")]
        public string NtBundleTree { get; set; }
        [JsonProperty("nt_bundle_attributes")]
        public FacebookNtBundleAttributes[] NtBundleAttributes { get; set; }
    }

    public class FacebookNtBundleAttributes
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }
        [JsonProperty("story_value")]
        public FacebookStoryValue StoryValue { get; set; }
    }

    public class FacebookStoryValue
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("cache_id")]
        public string CacheId { get; set; }
        [JsonProperty("creation_time")]
        public long CreationTime { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("actors")]
        public FacebookActorResponse[] Actors { get; set; }
        [JsonProperty("tracking")]
        public string Tracking { get; set; }
    }

}

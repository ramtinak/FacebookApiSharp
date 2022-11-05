/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using FacebookApiSharp.Helpers;


namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookSearchData
    {
        [JsonProperty("search_query")]
        public FacebookSearchDataQuery SearchQuery { get; set; }
    }

    public class FacebookSearchDataQueryNtBundleAttribute
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("strong_id__")]
        public object StrongId { get; set; }

        [JsonProperty("recent_search_entity_value")]
        public FacebookActorResponse RecentSearchEntityValue { get; set; }
    }

    public class FacebookSearchDataQueryNativeTemplateBundle
    {
        [JsonProperty("nt_bundle_tree")]
        public string NtBundleTree { get; set; }

        [JsonProperty("nt_bundle_attributes")]
        public List<FacebookSearchDataQueryNtBundleAttribute> NtBundleAttributes { get; set; }
    }

    public class FacebookSearchDataQueryNativeTemplateView
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
        public List<FacebookSearchDataQueryNativeTemplateBundle> NativeTemplateBundles { get; set; }
    }

    public class FacebookSearchDataQueryEdge
    {
        [JsonProperty("logging_unit_id")]
        public string LoggingUnitId { get; set; }

        [JsonProperty("module_role")]
        public string ModuleRole { get; set; }

        [JsonProperty("result_role")]
        public string ResultRole { get; set; }

        [JsonProperty("native_template_view")]
        public FacebookSearchDataQueryNativeTemplateView NativeTemplateView { get; set; }

        [JsonProperty("native_template_result_ids")]
        public List<string> NativeTemplateResultIds { get; set; }
    }

    public class FacebookSearchDataQueryCombinedResults
    {
        [JsonProperty("has_hcm")]
        public bool HasHcm { get; set; }

        [JsonProperty("has_iem_triggered")]
        public bool HasIemTriggered { get; set; }

        [JsonProperty("edges")]
        public List<FacebookSearchDataQueryEdge> Edges { get; set; }
    }

    public class FacebookSearchDataQuery
    {
        [JsonProperty("query_function")]
        public string QueryFunction { get; set; }

        [JsonProperty("logging_unit_id")]
        public string LoggingUnitId { get; set; }

        [JsonProperty("combined_results")]
        public FacebookSearchDataQueryCombinedResults CombinedResults { get; set; }

        [JsonProperty("cache_id")]
        public string CacheId { get; set; }
    }


}

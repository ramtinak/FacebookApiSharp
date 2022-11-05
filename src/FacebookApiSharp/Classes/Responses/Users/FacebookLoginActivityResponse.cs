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
    public class FacebookSummary
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class FacebookLoginActivityNodeResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("creation_time")]
        public long CreationTimeAtUnix { get; set; }
        public DateTime CreationTime => CreationTimeAtUnix.FromUnixTimeSeconds();

        [JsonProperty("summary")]
        public FacebookSummary Summary { get; set; }

        [JsonProperty("message")]
        public FacebookSummary Message { get; set; }

        [JsonProperty("__typename")]
        public string Typename { get; set; }
    }

    public class FacebookLoginActivityEdgeResponse
    {
        [JsonProperty("ent_fbid")]
        public string EntFbid { get; set; }

        [JsonProperty("node")]
        public FacebookLoginActivityNodeResponse Node { get; set; }

        [JsonProperty("do_not_link_actor")]
        public bool DoNotLinkActor { get; set; }

        [JsonProperty("cursor")]
        public string Cursor { get; set; }
    }

    public class FacebookLoginActivityStoriesResponse
    {
        [JsonProperty("edges")]
        public List<FacebookLoginActivityEdgeResponse> Edges { get; set; } = new List<FacebookLoginActivityEdgeResponse>();

        [JsonProperty("page_info")]
        public FacebookPageInfoResponse PageInfo { get; set; }
    }

    public class FacebookActivityLogActorResponse
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("__isActivityLogActor")]
        public string IsActivityLogActor { get; set; }

        [JsonProperty("stories")]
        public FacebookLoginActivityStoriesResponse Stories { get; set; }

        [JsonProperty("__isNode")]
        public string IsNode { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class FacebookLoginActivityDataViewerResponse
    {
        [JsonProperty("activity_log_actor")]
        public FacebookActivityLogActorResponse ActivityLogActor { get; set; }
    }

    public class FacebookLoginActivityDataResponse
    {
        [JsonProperty("viewer")]
        public FacebookLoginActivityDataViewerResponse Viewer { get; set; }
    }
}

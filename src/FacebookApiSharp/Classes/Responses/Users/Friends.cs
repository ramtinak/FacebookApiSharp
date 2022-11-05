/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookFriendsDataResponse
    {
        [JsonProperty("viewer")]
        public FacebookFriendsViewerResponse Viewer { get; set; }
    }

    public class FacebookFriendsViewerResponse
    {
        [JsonProperty("dynamic_friending_tab")]
        public FacebookFriendsDynamicFriendingResponse DynamicFriendingTab { get; set; }
        [JsonProperty("account_user")]
        public FacebookInboxFriendsUserAccountResponse AccountUser { get; set; }
        [JsonProperty("can_import_contacts")]
        public bool CanImportContacts { get; set; }
    }

    public class FacebookFriendsDynamicFriendingResponse
    {
        [JsonProperty("friend_request_count")]
        public int FriendRequestCount { get; set; }
        [JsonProperty("edges")]
        public FacebookFriendsDynamicEdgeResponse[] Edges { get; set; }
        [JsonProperty("page_info")]
        public FacebookPageInfoResponse PageInfo { get; set; }
    }

    public class FacebookFriendsDynamicEdgeResponse
    {
        [JsonProperty("node")]
        public FacebookFriendsDynamicEdgeNodeResponse Node { get; set; }
    }

    public class FacebookFriendsDynamicEdgeNodeResponse
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }
        [JsonProperty("show_see_all")]
        public bool ShowSeeAll { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("user")]
        public FacebookActorResponse User { get; set; }
    }
}

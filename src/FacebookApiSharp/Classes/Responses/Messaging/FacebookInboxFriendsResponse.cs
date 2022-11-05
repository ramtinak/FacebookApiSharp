/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookInboxFriendsResponse
    {
        [JsonProperty("data")]
        public FacebookInboxFriendsDataResponse Data { get; set; }
        [JsonProperty("extensions")]
        public FacebookPageResultExtensionsResponse Extensions { get; set; }
    }

    public class FacebookInboxFriendsDataResponse
    {
        [JsonProperty("viewer")]
        public FacebookInboxFriendsViewerResponse Viewer { get; set; }
    }

    public class FacebookInboxFriendsViewerResponse
    {
        [JsonProperty("account_user")]
        public FacebookInboxFriendsUserAccountResponse AccountUser { get; set; }
    }

    public class FacebookInboxFriendsUserAccountResponse
    {
        [JsonProperty("friends")]
        public FacebookInboxFriendsUserResponse Friends { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("top_friends")]
        public FacebookInboxFriendsUserResponse TopFriends { get; set; }
        [JsonProperty("top_friends_with_messenger")]
        public FacebookInboxFriendsUserResponse TopFriendsWithMessenger { get; set; }
    }

    public class FacebookInboxFriendsUserResponse
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("nodes")]
        public FacebookInboxFriendsUserNodeResponse[] Nodes { get; set; }
    }

    public class FacebookInboxFriendsUserNodeResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        [JsonProperty("profile_picture")]
        public FacebookProfilePictureResponse ProfilePicture { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("last_active_messages_status")]
        public FacebookInboxFriendsUserNodeLastActivityResponse LastActiveMessagesStatus { get; set; }
    }
    public class FacebookInboxFriendsUserNodeLastActivityResponse
    {
        [JsonProperty("is_currently_active")]
        public string IsCurrentlyActive { get; set; }
        [JsonProperty("time")]
        public long? Time { get; set; }
    }

}

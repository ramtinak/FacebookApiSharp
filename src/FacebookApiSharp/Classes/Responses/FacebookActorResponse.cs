/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookActorResponse
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("friendship_status")]
        public string FriendshipStatus { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("profile_picture")]
        public FacebookProfilePictureResponse ProfilePicture { get; set; }
        //public FacebookStoryBucketResponse story_bucket { get; set; }
        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }
        [JsonProperty("is_work_user")]
        public bool IsWorkUser { get; set; }
        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }
        [JsonProperty("subscribe_status")]
        public string SubscribeStatus { get; set; }
        [JsonProperty("secondary_subscribe_status")]
        public string SecondarySubscribeStatus { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("can_viewer_block")]
        public bool? CanViewerBlock { get; set; }
        [JsonProperty("profile_action")]
        public FacebookProfileActionResponse ProfileAction { get; set; }
    }

    public class FacebookProfilePictureResponse
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class FacebookProfileActionResponse
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }
        [JsonProperty("should_show_ap_disclaimer")]
        public bool ShouldShowApDisclaimer { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }
    }

    //public class FacebookStoryBucketResponse
    //{
    //    public FacebookStoryBucketNodeResponse[] nodes { get; set; }
    //}

    //public class FacebookStoryBucketNodeResponse
    //{
    //    public string id { get; set; }
    //    public bool is_bucket_seen_by_viewer { get; set; }
    //    public bool is_bucket_owned_by_viewer { get; set; }
    //    public string camera_post_type { get; set; }
    //    public Threads threads { get; set; }
    //    public int latest_thread_creation_time { get; set; }
    //}

    //public class Threads
    //{
    //    public bool is_empty { get; set; }
    //}

}

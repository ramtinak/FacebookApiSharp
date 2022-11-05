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
    public class FacebookEntityResponse
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Ranx
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("entity")]
        public FacebookEntityResponse Entity { get; set; }
    }

    public class Message
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("ranges")]
        public List<Ranx> Ranges { get; set; }
    }

    public class FacebookImage
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class FacebookCreationStory
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("cache_id")]
        public string CacheId { get; set; }
    }

    public class FacebookOwner
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }
    }

    public class FacebookTarget
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }

        [JsonProperty("image")]
        public FacebookImage Image { get; set; }

        [JsonProperty("creation_story")]
        public FacebookCreationStory CreationStory { get; set; }

        [JsonProperty("owner")]
        public FacebookOwner Owner { get; set; }
    }

    public class Media
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }

        [JsonProperty("image")]
        public FacebookImage Image { get; set; }

        [JsonProperty("imageLow")]
        public FacebookImage ImageLow { get; set; }

        [JsonProperty("imageMedium")]
        public FacebookImage ImageMedium { get; set; }

        [JsonProperty("imageHigh")]
        public FacebookImage ImageHigh { get; set; }

        [JsonProperty("owner")]
        public FacebookOwner Owner { get; set; }
    }

    public class FacebookAttachment
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("media_reference_token")]
        public string MediaReferenceToken { get; set; }

        [JsonProperty("target")]
        public FacebookTarget Target { get; set; }

        [JsonProperty("deduplication_key")]
        public string DeduplicationKey { get; set; }

        [JsonProperty("use_carousel_infinite_scroll")]
        public bool UseCarouselInfiniteScroll { get; set; }

        [JsonProperty("is_mute_music_ad")]
        public bool IsMuteMusicAd { get; set; }

        [JsonProperty("media")]
        public Media Media { get; set; }
    }

    public class FacebookFeedbackStory
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        public override string ToString()
        {
            return Id;
        }
    }

    public class FacebookStory
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("cache_id")]
        public string CacheId { get; set; }

        [JsonProperty("creation_time")]
        public int CreationTime { get; set; }
        [JsonProperty("feedback")]
        public FacebookFeedbackStory Feedback { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }

        [JsonProperty("is_fox_sharable")]
        public bool IsFoxSharable { get; set; }

        [JsonProperty("post_id")]
        public string PostId { get; set; }

        [JsonProperty("actors")]
        public List<FacebookActorResponse> Actors { get; set; } = new List<FacebookActorResponse>();

        [JsonProperty("tracking")]
        public string Tracking { get; set; }

        [JsonProperty("privacy_label")]
        public string PrivacyLabel { get; set; }

        [JsonProperty("legacy_api_story_id")]
        public string LegacyApiStoryId { get; set; }

        [JsonProperty("hideable_token")]
        public string HideableToken { get; set; }

        [JsonProperty("can_viewer_append_photos")]
        public bool CanViewerAppendPhotos { get; set; }

        [JsonProperty("can_viewer_edit")]
        public bool CanViewerEdit { get; set; }

        [JsonProperty("can_viewer_edit_metatags")]
        public bool CanViewerEditMetatags { get; set; }

        [JsonProperty("can_viewer_edit_post_media")]
        public bool CanViewerEditPostMedia { get; set; }

        [JsonProperty("can_viewer_edit_post_privacy")]
        public bool CanViewerEditPostPrivacy { get; set; }

        [JsonProperty("can_viewer_edit_link_attachment")]
        public bool CanViewerEditLinkAttachment { get; set; }

        [JsonProperty("can_viewer_delete")]
        public bool CanViewerDelete { get; set; }

        [JsonProperty("can_viewer_end_qna")]
        public bool CanViewerEndQna { get; set; }

        [JsonProperty("can_viewer_see_community_popular_waist")]
        public bool CanViewerSeeCommunityPopularWaist { get; set; }

        [JsonProperty("can_viewer_reshare_to_story")]
        public bool CanViewerReshareToStory { get; set; }

        [JsonProperty("can_viewer_reshare_to_story_now")]
        public bool CanViewerReshareToStoryNow { get; set; }

        [JsonProperty("substory_count")]
        public int SubstoryCount { get; set; }

        [JsonProperty("is_automatically_translated")]
        public bool IsAutomaticallyTranslated { get; set; }

        [JsonProperty("is_eligible_for_affiliate_commission")]
        public bool IsEligibleForAffiliateCommission { get; set; }

        [JsonProperty("has_comprehensive_title")]
        public bool HasComprehensiveTitle { get; set; }

        [JsonProperty("viewer_edit_post_feature_capabilities")]
        public List<string> ViewerEditPostFeatureCapabilities { get; set; }

        [JsonProperty("is_anonymous")]
        public bool IsAnonymous { get; set; }

        [JsonProperty("can_viewer_approve_post")]
        public bool CanViewerApprovePost { get; set; }

        [JsonProperty("should_show_easy_hide")]
        public bool ShouldShowEasyHide { get; set; }

        [JsonProperty("attachments")]
        public List<FacebookAttachment> Attachments { get; set; }

        [JsonProperty("is_marked_as_spam_by_admin_assistant")]
        public bool IsMarkedAsSpamByAdminAssistant { get; set; }
    }

    public class FacebookStoryCreate
    {
        [JsonProperty("story")]
        public FacebookStory Story { get; set; }

        [JsonProperty("logging_token")]
        public string LoggingToken { get; set; }
    }

    public class FacebookStoryCreateData
    {
        [JsonProperty("story_create")]
        public FacebookStoryCreate StoryCreate { get; set; }
    }


}

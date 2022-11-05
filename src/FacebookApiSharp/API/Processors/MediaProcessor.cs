/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes;
using FacebookApiSharp.Classes.DeviceInfo;
using FacebookApiSharp.Classes.Models;
using FacebookApiSharp.Classes.Responses;
using FacebookApiSharp.Classes.SessionHandlers;
using FacebookApiSharp.Converters;
using FacebookApiSharp.Enums;
using FacebookApiSharp.Helpers;
using FacebookApiSharp.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace FacebookApiSharp.API.Processors
{
    internal class MediaProcessor : IMediaProcessor
    {
        private readonly HttpHelper _httpHelper;
        private readonly UserSessionData User;
        private readonly IFacebookLogger _logger;
        private readonly AndroidDevice _deviceInfo;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly FacebookApi _facebookApi;
        public MediaProcessor(AndroidDevice deviceInfo,
            UserSessionData user,
            IHttpRequestProcessor httpRequestProcessor,
            IFacebookLogger logger,
            FacebookApi facebookApi,
            HttpHelper httpHelper)
        {
            _deviceInfo = deviceInfo;
            _facebookApi = facebookApi;
            User = user;
            _logger = logger;
            _httpHelper = httpHelper;
            _httpRequestProcessor = httpRequestProcessor;
        }

        public async Task<IResult<bool>> CommentMediaAsync(string feedbackId, string text, params string[] mentions)
        {
            var ntContext = new JObject
            {
                {"using_white_navbar", true},
                {"styles_id", FacebookApiConstants.FACEBOOK_STYLES_ID},
                {"pixel_ratio", 3},
                {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
            };
            var msg = new JObject
            {
                {"text", text},
            };
            if (mentions?.Length > 0)
            {
                //"ranges": [
                //  {
                //    "offset": 0,
                //    "length": 12,
                //    "entity": { "id": "100007855531564" }
                //  },
                //  {
                //    "offset": 19,
                //    "length": 15,
                //    "entity": { "id": "100034137818552" }
                //  }
                //]
                int offset = 0;
                var jARR = new JArray();
                foreach (var item in mentions)
                {
                    jARR.Add(new JObject
                    {
                        { "offset", offset },
                        { "length", 6},
                       { "entity", new JObject { { "id", item } } },
                    });
                    offset += 7;
                }
                msg.Add("ranges", jARR);
            }
            var input = new JObject()
            {
    //            "vod_video_timestamp": 0,
    //"privacy_type": "DEFAULT_PRIVACY",
    //"tracking": [ "{\"mf_story_key\":\"694995648315023\",\"top_level_post_id\":\"694995648315023\",\"tl_objid\":\"694995648315023\",\"content_owner_id_new\":\"100034137818552\",\"throwback_story_fbid\":\"694995648315023\",\"story_location\":4,\"app_id\":\"350685531728\",\"profile_id\":\"100034137818552\",\"profile_relationship_type\":1,\"actrs\":\"100034137818552\",\"ftmd_400706\":\"111112l\"}", "{\"image_loading_state\":0,\"radio_type\":\"wifi-none\",\"client_viewstate_position\":0,\"feed_unit_trace_info\":\"{\\\"groups_tab_feed_unit_type_name\\\":\\\"Story\\\",\\\"groups_tab_story_is_crf\\\":2}\"}", "{\"conversation_guide_shown\":\"none\"}" ],
    //"mentions_logging_metadata": { "explicitly_added_mentionee_ids": [ "100007855531564", "100034137818552" ] },
    //"entry_point": "TAP_FOOTER_COMMENT",
    //"idempotence_token": "0386b881-44b0-4557-8b02-ff374f99fc9d",
    //"feedback_referrer": "timeline",
                {"vod_video_timestamp", 0},
                {"privacy_type", "DEFAULT_PRIVACY"},
                {"entry_point", "TAP_FOOTER_COMMENT"},
                {"idempotence_token", Guid.NewGuid().ToString()},
                {"feedback_referrer", "timeline"},
                {"message", msg},





    //"feedback_source": "feedback_comments",
    //"feedback_id": "ZmVlZGJhY2s6Njk0OTk1NjQ4MzE1MDIz",
    //"client_mutation_id": "eb9dc1a8-511d-41a2-8df3-90b7c9fd894c",
    //"nectar_module": "newsfeed_ufi",
        //"actor_id": "100034137818552",
    //"action_timestamp": 1649982231
                {"feedback_source", "feedback_comments"},
                {"feedback_id", feedbackId},
                {"client_mutation_id", Guid.NewGuid().ToString()},
                {"nectar_module", "newsfeed_ufi"},
                //"attribution_id_v2": "SimpleUFIPopoverFragment,story_feedback_flyout,tap_footer_comment,1649982180.295,22630825,,;ProfileFragment,timeline,tap_bookmark,1649982164.700,203422939,User,;BookmarkComponentsFragment,bookmarks,tap_top_jewel_bar,1649982161.779,238861851,281710865595635,",
                {"actor_id", User.LoggedInUser.UId.ToString()},
                {"action_timestamp", DateTime.UtcNow.ToUnixTime()},
            };
            if (mentions?.Length > 0)
            {
                //    "mentions_logging_metadata": { "explicitly_added_mentionee_ids": [ "100007855531564", "100034137818552" ] },
                //input.Add("mentions_logging_metadata", new JObject
                //{
                //    {"explicitly_added_mentionee_ids", new JArray(mentions)}
                //});
                input.Add(new JProperty("mentions_logging_metadata", new JObject
                {
                    {"explicitly_added_mentionee_ids", new JArray(mentions)}
                }));
                //input.Add("mentions_logging_metadata", new JObject { "explicitly_added_mentionee_ids", new JArray(mentions) });
            }
            var variables = new JObject
            {
  //"enable_comment_reactions": true,
  //"enable_comment_reactions_icons": true,
  //"enable_ranked_replies": "true",
  //"include_comment_depth": true,
  //"enable_comment_replies_most_recent": "true",
                {"enable_comment_reactions", true},
                {"enable_comment_reactions_icons", true},
                {"enable_ranked_replies", "true"},
                {"include_comment_depth", true},
                {"enable_comment_replies_most_recent", "true"},
                {"scale", "3"},
                {"nt_context", ntContext},
                {"input", input},
  //"profile_image_size": 110,
  //"max_comment_replies": 3,
  //"enable_comment_reputation_system_community_awards": true,
  //"fetch_privacy_value_for_declined_comment": true,
  //"enable_comment_reputation_system_inline_vote": true,
  //"enable_comment_shares": true,
  //"group_member_action_source": "GROUP_COMMENT",
  //"feedback_source": "USER_TIMELINE",
  //"enable_comment_voting": true,
  //"enable_user_signals_in_comments": true,
  //"fetch_presence_eligible": true,
  //"fetch_privacy_value_for_pending_approval_comment": true
                {"profile_image_size", 110},
                {"max_comment_replies", 3},
                {"enable_comment_reputation_system_community_awards", true},
                {"fetch_privacy_value_for_declined_comment", true},
                {"enable_comment_reputation_system_inline_vote", true},
                {"enable_comment_shares", true},
                {"group_member_action_source", "GROUP_COMMENT"},
                {"feedback_source", "USER_TIMELINE"},
                {"enable_comment_voting", true},
                {"enable_user_signals_in_comments", true},
                {"fetch_presence_eligible", true},
                {"fetch_privacy_value_for_pending_approval_comment", true},
            };
            //client_doc_id=8474489812640157321370321948&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //variables={"enable_comment_reactions":true,"enable_comment_reactions_icons":true,"enable_ranked_replies":"true","include_comment_depth":true,"enable_comment_replies_most_recent":"true","nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"800cb86569d1fc1615bd1576caae8172","bloks_version":"fcb188fb4c97a862b85d875824832dd3e42ead2a17102661b4bed0825f260514"},"input":{"vod_video_timestamp":0,"privacy_type":"DEFAULT_PRIVACY","tracking":["{\"mf_story_key\":\"694995648315023\",\"top_level_post_id\":\"694995648315023\",\"tl_objid\":\"694995648315023\",\"content_owner_id_new\":\"100034137818552\",\"throwback_story_fbid\":\"694995648315023\",\"story_location\":4,\"app_id\":\"350685531728\",\"profile_id\":\"100034137818552\",\"profile_relationship_type\":1,\"actrs\":\"100034137818552\",\"ftmd_400706\":\"111112l\"}","{\"image_loading_state\":0,\"radio_type\":\"wifi-none\",\"client_viewstate_position\":0,\"feed_unit_trace_info\":\"{\\\"groups_tab_feed_unit_type_name\\\":\\\"Story\\\",\\\"groups_tab_story_is_crf\\\":2}\"}","{\"conversation_guide_shown\":\"none\"}"],"mentions_logging_metadata":{"explicitly_added_mentionee_ids":["100007855531564","100034137818552"]},"entry_point":"TAP_FOOTER_COMMENT","idempotence_token":"0386b881-44b0-4557-8b02-ff374f99fc9d","feedback_referrer":"timeline","message":{"text":"Ramtin Jokar hey u Nasrollah Jokar rmtx","ranges":[{"offset":0,"length":12,"entity":{"id":"100007855531564"}},{"offset":19,"length":15,"entity":{"id":"100034137818552"}}]},"feedback_source":"feedback_comments","feedback_id":"ZmVlZGJhY2s6Njk0OTk1NjQ4MzE1MDIz","client_mutation_id":"eb9dc1a8-511d-41a2-8df3-90b7c9fd894c","nectar_module":"newsfeed_ufi","attribution_id_v2":"SimpleUFIPopoverFragment,story_feedback_flyout,tap_footer_comment,1649982180.295,22630825,,;ProfileFragment,timeline,tap_bookmark,1649982164.700,203422939,User,;BookmarkComponentsFragment,bookmarks,tap_top_jewel_bar,1649982161.779,238861851,281710865595635,","actor_id":"100034137818552","action_timestamp":1649982231},"profile_image_size":110,"max_comment_replies":3,"enable_comment_reputation_system_community_awards":true,"fetch_privacy_value_for_declined_comment":true,"enable_comment_reputation_system_inline_vote":true,"enable_comment_shares":true,"group_member_action_source":"GROUP_COMMENT","feedback_source":"USER_TIMELINE","enable_comment_voting":true,"enable_user_signals_in_comments":true,"fetch_presence_eligible":true,"fetch_privacy_value_for_pending_approval_comment":true}


            // [
            //  "nav_attribution_id={\"0\":{\"bookmark_id\":\"190055527696468\",\"session\":\"cae2a\",\"subsession\":1,\"timestamp\":\"1649982164.700\",\"tap_point\":\"tap_bookmark\",\"most_recent_tap_point\":\"tap_bookmark\",\"bookmark_type_name\":\"User\",\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"},\"extras\":{\"surface_type\":\"plazaSurface\"}},\"1\":{\"bookmark_id\":\"281710865595635\",\"session\":\"09db3\",\"subsession\":1,\"timestamp\":\"1649982161.778\",\"tap_point\":\"tap_top_jewel_bar\",\"most_recent_tap_point\":\"tap_top_jewel_bar\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}",
            //  "nav_attribution_id_v2=SimpleUFIPopoverFragment,story_feedback_flyout,tap_footer_comment,1649982180.295,22630825,,;ProfileFragment,timeline,tap_bookmark,1649982164.700,203422939,User,;BookmarkComponentsFragment,bookmarks,tap_top_jewel_bar,1649982161.779,238861851,281710865595635,",
            //  "visitation_id=190055527696468:cae2a:1:1649982164.700",
            //  "surface_hierarchy=FeedbackFragment,story_feedback_flyout,null;SimpleUFIPopoverFragment,story_feedback_flyout,null;FbMainTabActivity,unknown,null",
            //  "session_id=UFS-00f34f48-7ef0-4421-b0cc-883e8fd8c133-fg-2",
            //  "GraphServices"
            //]

            var time = DateTime.UtcNow.ToUnixTimeAsDouble().ToString();
            var graphService = new JArray(
            "nav_attribution_id={\"0\":{\"bookmark_id\":\"190055527696468\",\"session\":\"%SESSION%\",\"subsession\":1,\"timestamp\":\"%TIME%\",\"tap_point\":\"cold_start\",\"most_recent_tap_point\":\"cold_start\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}"
            .Replace("%SESSION%", ExtensionsHelper.GetGuid(5)).Replace("%TIME%", time),
                "surface_hierarchy=FeedbackFragment,story_feedback_flyout,null;SimpleUFIPopoverFragment,story_feedback_flyout,null;FbMainTabActivity,unknown,null",
                $"session_id=UFS-{Guid.NewGuid()}-fg-2",
            $"visitation_id=190055527696468:{ExtensionsHelper.GetGuid(5)}:1:{time}",
            "GraphServices"
            );

            var result = await _facebookApi.SendRequestAsync<FacebookPaginationResultResponse<object>,
                   FacebookPaginationResultResponse<object>>
                   ("8474489812640157321370321948",
                   "post",
                   "CommentCreateMutation",
                   graphService.ToString(Formatting.None),
                   variables.ToString(Formatting.None),
                   null,
                   null,
                   null,
                   null,
                   true,
                   true,
                   "graphservice",
                   true);

            return Result.Success(result.Succeeded);
        }
        public async Task<IResult<bool>>  GetCommentOptionsAsync(string id)
        {
            // head
            // X-FB-QPL-ACTIVE-FLOWS-JSON: {"schema_version":"v1","inprogress_qpls":[{"marker_id":25952257,"annotations":{"current_endpoint":"ProfileFragment:timeline"}}]}

            var variables = new JObject
            {
                {"feedback_id", id }
            };
            //client_doc_id=6690188199119365973013345052&
            //method=post&locale=en_US&
            //pretty=false&format=json&
            //variables={"feedback_id":"ZmVlZGJhY2s6MTAyMjcxODk5MTIzNzUy"}
            //&
            //fb_api_req_friendly_name=CommentModerationFiltersProducerQuery&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["GraphServices"]&
            //server_timestamps=true

            var result = await _facebookApi.SendRequestAsync<FacebookPaginationResultResponse<object>,
                   FacebookPaginationResultResponse<object>>
                   ("6690188199119365973013345052",
                   "post",
                   "CommentModerationFiltersProducerQuery",
                   "[\"GraphServices\"]",
                   variables.ToString(Formatting.None),
                   null,
                   null,
                   null,
                   null,
                   true,
                   true,
                   "graphservice",
                   true);

            return Result.Success(result.Succeeded);
        }
        public async Task<IResult<bool>> DisableCommentsAsync(string postId, string id)
        {
            await GetCommentOptionsAsync(id);
            // head
            // X-FB-QPL-ACTIVE-FLOWS-JSON: {"schema_version":"v1","inprogress_qpls":[{"marker_id":25952257,"annotations":{"current_endpoint":"ProfileFragment:timeline"}}]}


            //client_doc_id=64155661311890268434775788301&
            //method=post&locale=en_US&pretty=false&format=json&
            //variables={
            //  "input": {
            //    "post_with_comment_moderation_filters_id": "102271899123752",
            //    "filter": "TAGGED",
            //    "client_mutation_id": "628e2172-0665-4d56-a460-b8a43bd7a39a",
            //    "actor_id": "100080228257209"
            //  }
            //}

            var variables = new JObject
            {
                {"input", new JObject
                    {
                        {"post_with_comment_moderation_filters_id", postId},
                        {"filter", "TAGGED"},
                        {"client_mutation_id", Guid.NewGuid().ToString()},
                        {"actor_id", User.LoggedInUser.UId.ToString()},
                    }
                }
            };
            //&fb_api_req_friendly_name=SetCommentModerationFilter&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["nav_attribution_id={\"0\":{\"bookmark_id\":\"4748854339\",\"session\":\"0d365\",\"subsession\":1,\"timestamp\":\"1649198132.344\",\"tap_point\":\"cold_start\",\"most_recent_tap_point\":\"cold_start\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}","visitation_id=190055527696468:aa74e:1:1649198141.713","GraphServices"]
            //&server_timestamps=true

            //[
            //  "nav_attribution_id={\"0\":{\"bookmark_id\":\"4748854339\",\"session\":\"0d365\",\"subsession\":1,\"timestamp\":\"1649198132.344\",\"tap_point\":\"cold_start\",\"most_recent_tap_point\":\"cold_start\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}",
            //  "visitation_id=190055527696468:aa74e:1:1649198141.713",
            //  "GraphServices"
            //]
            var time = DateTime.UtcNow.ToUnixTimeAsDouble().ToString();
            var graphService = new JArray(
            "nav_attribution_id={\"0\":{\"bookmark_id\":\"4748854339\",\"session\":\"%SESSION%\",\"subsession\":1,\"timestamp\":\"%TIME%\",\"tap_point\":\"cold_start\",\"most_recent_tap_point\":\"cold_start\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}"
            .Replace("%SESSION%", ExtensionsHelper.GetGuid(5)).Replace("%TIME%", time),
            $"visitation_id=4748854339:{ExtensionsHelper.GetGuid(5)}:4:{time}",
            "GraphServices"
            );

            var result = await _facebookApi.SendRequestAsync<FacebookPaginationResultResponse<object>,
                   FacebookPaginationResultResponse<object>>
                   ("64155661311890268434775788301",
                   "post",
                   "SetCommentModerationFilter",
                   graphService.ToString(Formatting.None),
                   variables.ToString(Formatting.None),
                   null,
                   null,
                   null,
                   null,
                   true,
                   true,
                   "graphservice",
                   true);

            return Result.Success(result.Succeeded);
        }
        public async Task<IResult<FacebookStoryCreate>> UploadPhotoAsync(string caption, byte[] imageBytes, bool disableComments)
        {
            string responseContent = null;

            try
            {
                var boundary = ExtensionsHelper.GetNewGuid();

                var guid = Guid.NewGuid().ToString();
                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="published"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //false

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="audience_exp"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //true


                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="qn"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //7ca952a9-9c98-45ec-8133-f05d0c66626f

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="composer_session_id"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //7ca952a9-9c98-45ec-8133-f05d0c66626f

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="idempotence_token"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //7ca952a9-9c98-45ec-8133-f05d0c66626f_457971145_0

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="source_type"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //newsfeed

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="locale"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //en_US

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="client_country_code"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //IR

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="fb_api_req_friendly_name"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //upload-photo

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="fb_api_caller_class"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit
                //MultiPhotoUploader

                var requestContent = new MultipartFormDataContent(boundary)
                {
                   {new StringContent("false"), "\"published\""},
                   {new StringContent("true"), "\"audience_exp\""},
                   {new StringContent(guid), "\"qn\""},
                   {new StringContent(guid), "\"composer_session_id\""},
                   {new StringContent($"{guid}_{guid.GetHashCode()}_0"), "\"idempotence_token\""},
                   {new StringContent("newsfeed"), "\"source_type\""},
                   {new StringContent(_facebookApi.AppLocale), "\"locale\""},
                   {new StringContent(_facebookApi.ClientCountryCode), "\"client_country_code\""},
                   {new StringContent("upload-photo"), "\"fb_api_req_friendly_name\""},
                   {new StringContent("MultiPhotoUploader"), "\"fb_api_caller_class\""},
                };

                var imageContent = new ByteArrayContent(imageBytes);

                //--3_qhDPohWr1-3dc4u-__R20VYKcPKvd
                //Content-Disposition: form-data; name="source"; filename="1b4c15c9-69974b88e848ba84.tmp"
                //Content-Type: image/jpeg
                //Content-Transfer-Encoding: binary
                imageContent.Headers.AddHeader("Content-Transfer-Encoding", "binary");
                imageContent.Headers.AddHeader("Content-Type", "image/jpeg");
                //imageContent.Headers.AddHeader("Content-Length", fileBytes.Length.ToString(), true);
                //requestContent.Headers.AddHeader("Content-Length", fileBytes.Length.ToString(), true);
                requestContent.Add(imageContent, "source", $"\"{ExtensionsHelper.GetNewGuid()}.tmp\"");

                var request = /*GetDefaultRequest(HttpMethod.Post)*/ _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    new Uri("https://graph.facebook.com/me/photos"), _deviceInfo);
                request.Headers.AddHeader("family_device_id", _deviceInfo.PhoneGuid.ToString(), true);
                request.Headers.AddHeader("device_id", _deviceInfo.DeviceGuid.ToString(), true);
                request.Headers.AddHeader("request_token", Guid.NewGuid().ToString(), true);
                request.Headers.AddHeader("Priority", "u=3, i", true);
                //request.Headers.AddHeader("user_id", "100007855531564"/*User.LoggedInUser.UId.ToString()*/, true);
                request.Headers.AddHeader("X-FB-HTTP-Engine", "Liger", true);
                request.Headers.AddHeader("X-FB-Client-IP", "True", true);
                request.Headers.AddHeader("X-FB-Server-Cluster", "True", true);
                request.Headers.AddHeader("X-FB-Friendly-Name", "upload-photo", true);
                request.Headers.AddHeader("X-FB-Request-Analytics-Tags", "MULTIMEDIA", true);

                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_RMD, "cached=0;state=NO_MATCH", true);
                //request.Headers.AddHeader("X-ZERO-EH", "2,,"+ User.LoggedInUser.AnalyticsClaim.Replace("hmac.", ""), true);
                //requestContent.Headers.AddHeader("x-fb-connection-token", Guid.NewGuid().ToString().Replace("-",""), true);
                request.Headers.Remove("X-FB-Connection-Quality");
                request.Headers.Remove("X-FB-Connection-Type");
                request.Headers.AddHeader("X-MSGR-Region", "FRC", true);

                //x-fb-session-id: nid=dNB0r62O8F9m;tid=73;nc=1;fc=1;bc=0;cid=7e06ada14958b602edf892b7546f6d1a
                request.Headers.AddHeader("x-fb-session-id", $"nid={User.LoggedInUser.SessionKey.Split('.')[1]};" +
                    $"tid=73;nc=1;fc=1;bc=0;cid=" +
                    Guid.NewGuid().ToString().Replace("-", ""), true);

                request.Content = requestContent;
                var response = await _httpRequestProcessor.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookStoryCreate>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookStoryCreate));

                if (string.IsNullOrEmpty(json) || json?.Length == 0)
                    return Result.Fail<FacebookStoryCreate>("It seems your account is blocked by facebook", ResponseType.AccountBlocked, default);

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookStoryCreate>(response, json);

                //{"id":"3196727743932379"}
                var jObj = JsonConvert.DeserializeObject<JObject>(json);
                var id = jObj["id"]?.Value<string>();




                var result = await ConfigureUploadPhoto(caption, id, guid, true, disableComments);

                //{"data":{"story_create":{"story":{"__typename":"Story","strong_id__":"-1911695493556300750","id":"UzpfSTEwMDAzNDEzNzgxODU1Mjo2OTAyMjQ1Mzg3OTIxMzQ=","cache_id":"-1911695493556300750","anonymous_profile_header_nt_action":null,"header_accent_color":null,"paid_partnership_label_tooltip":null,"to":null,"subtitle":null,"creation_time":1648510465,"backdated_time":null,"url":"https:\/\/m.facebook.com\/story.php?story_fbid=690224538792134&id=100034137818552","display_time_block_info":null,"titleFromRenderLocation":null,"feed_mobile_title":null,"message_markdown_html":null,"message_richtext":[],"message":{"text":"Cover me https:\/\/30nama.com\/anime\/92392\/Your-Name-2016\npekh","ranges":[{"offset":9,"length":45,"entity":{"__typename":"ExternalUrl","id":"NjQyMTgzOTU5MjA4MTA3Omh0dHBzXGEvLzMwbmFtYS5jb20vYW5pbWUvOTIzOTIvWW91ci1OYW1lLTIwMTY6OkRlZmF1bHQ6Og==","strong_id__":"2780734226133869401","name":"","url":"https:\/\/lm.facebook.com\/l.php?u=https\u00253A\u00252F\u00252F30nama.com\u00252Fanime\u00252F92392\u00252FYour-Name-2016&h=AT0fkMD8yTB4v2_fYlsEI0pyhUTJTHJzGMd9rtkZAC0qPDZY5sREx8913Cl0i8phq7rro6_Zh7X4-tYIjQ3foy3QD7C_HNZt0-d-OaOj2NNYKFK5XGVN5i_tMVA79TURPP8&s=1","android_urls":[]}}],"delight_ranges":[],"color_ranges":[],"image_ranges":[]},"suffix":null,"is_fox_sharable":false,"post_id":"690224538792134","actors":[{"__typename":"User","id":"100034137818552","friendship_status":"CANNOT_REQUEST","gender":"MALE","name":"Nasrollah Jokar","profile_picture":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t1.6435-1\/51998503_103731534108107_2613954363776827392_n.jpg?stp=c0.12.111.111a_cp0_dst-jpg_e15_p111x111_q65&_nc_cat=105&ccb=1-5&_nc_sid=7206a8&_nc_ohc=dY4x2D9lDKgAX9YEL_T&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-bEeAGtcrvv7yOIy5Vpiqj4Lf0QopeC77kSZtuEVUbTA&oe=62663279","width":111,"height":111},"story_bucket":{"nodes":[{"id":"502317554249501","is_bucket_seen_by_viewer":true,"is_bucket_owned_by_viewer":true,"camera_post_type":"STORY","threads":{"is_empty":true},"latest_thread_creation_time":null}]},"strong_id__":"100034137818552","is_work_user":false,"work_foreign_entity_info":null,"work_info":null,"is_verified":false,"profile_badge":null,"subscribe_status":"CANNOT_SUBSCRIBE","secondary_subscribe_status":"REGULAR_FOLLOW","short_name":"Nasrollah","is_favorite":false,"url":"https:\/\/m.facebook.com\/nasrollah.jokar.54"}],"collaborators":[],"tracking":"{\"top_level_post_id\":\"690224538792134\",\"content_owner_id_new\":\"100034137818552\",\"story_location\":9,\"story_attachment_style\":\"photo\",\"ent_attachement_type\":\"MediaAttachment\",\"actrs\":\"100034137818552\"}","privacy_label":"Public","xposted_destination_apps":[],"privacy_scope":{"label":"Public","description":"Public","extended_description":"Anyone on or off Facebook","type":"everyone","icon":{"uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yp\/r\/--soLpMIbaJ.png","name":"everyone","width":10,"height":11},"can_viewer_edit":true,"education_info":{"reshare_education_info":null,"tag_expansion_education":{"eligible_for_education":false,"show_active_education":true,"education_content":null},"fullindex_education_info":{"eligible_for_education":false,"show_active_education":false,"education_content":null,"help_center_url":null}},"privacy_options":{"edges":[{"is_currently_selected":true,"node":{"id":"eed107b8fc03ff218e6a7239b8ef1e06","name":"Public","icon_image":{"name":"everyone"},"legacy_graph_api_privacy_json":"{\"value\":\"EVERYONE\"}","included_members":[],"excluded_members":[],"current_tag_expansion":"NONE","tag_expansion_options":["NONE"]}}]},"icon_image":{"name":"everyone"},"extra_payload":null},"save_info":{"is_offer_like_savable":false,"viewer_save_state":"NOT_SAVED","story_save_type":"POST","story_save_nux_type":null,"story_save_nux_min_consume_duration":null,"story_save_nux_max_consume_duration":null,"savable":{"__typename":"Photo","id":"690224498792138","viewer_saved_state":"NOT_SAVED","savable_default_category":"LINK","is_spherical":false,"strong_id__":"690224498792138"}},"legacy_api_story_id":"100034137818552_690224538792134","hideable_token":"M7M0MDIyMTW2MLc0MjQ2qXPNKwkuSSwpLXYuSk0syczPCy7JL6qsqzM0MDAwNjE0NrcwtDA1NaqrM6gDAA","negative_feedback_actions":{"num_actions_above_fold":null,"num_actions_folded":null,"edges":[{"node":{"title":{"text":"Move to archive"},"subtitle":null,"negative_feedback_action_type":"MOVE_TO_ARCHIVE","negative_feedback_action_icon_name":"box","native_template_view":{"unique_id":"ckj2xts1c:0","logging_id":"{\"callsite\":\"{\\\"product\\\":\\\"cix\\\",\\\"feature\\\":\\\"nfx_actions\\\",\\\"oncall\\\":\\\"nfx_actions\\\"}\",\"push_phase\":\"C3\",\"version\":1}","data_diff_item_id":"EMPTY:0197f54ac4422c0fc2b420044bb38b75","data_diff_content_id":"EMPTY:0197f54ac4422c0fc2b420044bb38b75","native_template_bundles":[{"nt_bundle_tree":"{\"\\\"\":\"\\ucd64\",\";\":\"__ntattrp__:1\",\"1\":\"box\",\"9\":[{\"\\\"\":\"(\",\" \":[{\"\\\"\":\"X\",\"$\":\"nt\\\/content_access_and_control\\\/archive?story_id=S\\u00253A_I100034137818552\\u00253A690224538792134&loading_overlay_id=\\u00255B\\u002522ckj2wy\\u00253A0\\u002522\\u00252Cnull\\u00255D\",\",\":[{\"\\\"\":\"\\uc60c\",\" \":[{\"\\\"\":\"8\",\"6\":\"FF000000\",\":\":\"center\",\" \":[{\"\\\"\":\"4\",\"8\":\"LARGE\"}]}],\"$\":1,\"!\":\"ckj2wy:0\"}],\"(\":[{\"\\\"\":\"(\",\" \":[{\"\\\"\":\"\\uc60f\",\"$\":\"ckj2wy:0\"},{\"\\\"\":\"\\uca71\"},{\"\\\"\":\"\\uc3d3\",\")\":\"We couldn't move this to your archive at this time\",\"$\":\"\",\"&\":\"LONG\",\"*\":\"ERROR\"}]}]},{\"\\\"\":\"X\",\"$\":\"nt\\\/content_access_and_control\\\/typed_logger?control=archive&event_type=content_access_and_control_click_begin&is_access=0&is_bulk=0&is_control=1&surface=post_chevron_menu_news_feed\"}]}],\"$\":\"menu_action_archive\"}","nt_bundle_attributes":[{"__typename":"NTTextWithEntitiesAttribute","name":"1","strong_id__":null,"twe_value":{"text":"Move to archive","ranges":[]},"twe_for_feed_message_value":null}],"nt_bundle_state":null,"nt_bundle_default_state":null,"nt_bundle_referenced_states":[],"nt_bundle_client_defined_states":null,"nt_follow_up_map":[]}],"bloks_root_component":null},"target_entity":null,"target_entity_type":null,"feedback_tags":[],"url":null,"cache_id":"-7438422956653992981"}},{"node":{"title":{"text":"Move to trash"},"subtitle":{"text":"Items in your trash are deleted after 30 days."},"negative_feedback_action_type":"MOVE_TO_TRASH","negative_feedback_action_icon_name":"trash","native_template_view":{"unique_id":"ckj2xts1c:1","logging_id":"{\"callsite\":\"{\\\"product\\\":\\\"cix\\\",\\\"feature\\\":\\\"nfx_actions\\\",\\\"oncall\\\":\\\"nfx_actions\\\"}\",\"push_phase\":\"C3\",\"version\":1}","data_diff_item_id":"EMPTY:f41105d948b24dd30b10a618a577fdf9","data_diff_content_id":"EMPTY:f41105d948b24dd30b10a618a577fdf9","native_template_bundles":[{"nt_bundle_tree":"{\"\\\"\":\"\\ucd64\",\";\":\"__ntattrp__:1\",\"8\":\"__ntattrp__:2\",\"1\":\"trash\",\"9\":[{\"\\\"\":\"(\",\" \":[{\"\\\"\":\" \",\" \":[{\"\\\"\":\"!\",\"&\":\"CANCEL\",\"(\":\"Cancel\"},{\"\\\"\":\"!\",\" \":[{\"\\\"\":\"X\",\"$\":\"nt\\\/content_access_and_control\\\/trash?story_id=S\\u00253A_I100034137818552\\u00253A690224538792134&loading_overlay_id=\\u00255B\\u002522ckj2wy\\u00253A1\\u002522\\u00252Cnull\\u00255D\",\",\":[{\"\\\"\":\"\\uc60c\",\" \":[{\"\\\"\":\"8\",\"6\":\"FF000000\",\":\":\"center\",\" \":[{\"\\\"\":\"4\",\"8\":\"LARGE\"}]}],\"$\":1,\"!\":\"ckj2wy:1\"}],\"(\":[{\"\\\"\":\"(\",\" \":[{\"\\\"\":\"\\uc60f\",\"$\":\"ckj2wy:1\"},{\"\\\"\":\"\\uca71\"},{\"\\\"\":\"\\uc3d3\",\")\":\"We couldn't move this to your trash at this time\",\"$\":\"\",\"&\":\"LONG\",\"*\":\"ERROR\"}]}]}],\"(\":\"Move\"}],\"&\":\"Items in your trash will be automatically deleted after 30 days. You can delete them earlier from your Trash by going to Activity Log in Settings. If your live video was added to your story, it will be removed from your story immediately.\",\"$\":\"ALERT\",\"(\":\"Move to your trash?\"},{\"\\\"\":\"X\",\"$\":\"nt\\\/content_access_and_control\\\/typed_logger?control=move_to_trash&event_type=content_access_and_control_click_begin&is_access=0&is_bulk=0&is_control=1&surface=post_chevron_menu_news_feed\"}]}],\"$\":\"menu_action_trash\"}","nt_bundle_attributes":[{"__typename":"NTTextWithEntitiesAttribute","name":"1","strong_id__":null,"twe_value":{"text":"Move to trash","ranges":[]},"twe_for_feed_message_value":null},{"__typename":"NTTextWithEntitiesAttribute","name":"2","strong_id__":null,"twe_value":{"text":"Items in your trash are deleted after 30 days.","ranges":[]},"twe_for_feed_message_value":null}],"nt_bundle_state":null,"nt_bundle_default_state":null,"nt_bundle_referenced_states":[],"nt_bundle_client_defined_states":null,"nt_follow_up_map":[]}],"bloks_root_component":null},"target_entity":null,"target_entity_type":null,"feedback_tags":[],"url":null,"cache_id":"590993376658054336"}}]},"rapid_reporting_prompt":null,"message_truncation_line_limit":7,"frx_content_overlay_prompt":null,"multilingual_author_dialects":[],"author_translations":[],"translatability_for_viewer":{"source_dialect":"en_XX","source_dialect_name":"English","target_dialect":"en_XX","target_dialect_name":"English","translation_type":"NO_TRANSLATION","translation":null},"can_viewer_append_photos":true,"can_viewer_edit":true,"can_viewer_edit_metatags":true,"can_viewer_edit_post_media":true,"can_viewer_edit_post_privacy":true,"can_viewer_edit_link_attachment":true,"can_viewer_delete":true,"can_viewer_end_qna":false,"can_viewer_see_community_popular_waist":false,"can_viewer_reshare_to_story":true,"can_viewer_reshare_to_story_now":true,"substories_grouping_reasons":[],"group_post_topic_tags":[],"via":null,"with_tags":{"nodes":[]},"application":null,"substory_count":0,"implicit_place":null,"explicit_place":null,"page_rec_info":null,"sponsored_data":null,"is_automatically_translated":false,"is_eligible_for_affiliate_commission":false,"sponsor_relationship":0,"sponsor_bumper_format":null,"sponsor_bumper_logo":null,"action_links":[{"__typename":"ShareActionLink","strong_id__":null,"page":null,"url":null,"feed_cta_type":"UNKNOWN","link_type":null,"title":"Share"}],"attached_action_links":[],"bumpers":null,"shareable":{"__typename":"Story","id":"UzpfSTEwMDAzNDEzNzgxODU1Mjo2OTAyMjQ1Mzg3OTIxMzQ=","strong_id__":"-1911695493556300750"},"integrity_context_image_context_trigger":null,"has_comprehensive_title":false,"edit_history":{"count":0},"inline_activities":{"nodes":[]},"display_explanation":null,"story_attribution":null,"story_header":null,"crisis_listing":null,"blood_request":null,"actions":[],"supplemental_social_story":null,"viewer_edit_post_feature_capabilities":["FEED_TOPICS","STICKER","CONTAINED_MEDIA","CONTAINED_LINK","PRODUCT_ITEM","FUNDRAISER_FOR_STORY","POST_CONTAINER"],"copyright_block_info":null,"copyright_banner_info":null,"copyright_attribution_native_template_view":null,"camera_post_info":{"shareable_id":"690224538792134"},"page_exclusive_post_info":null,"newsfeed_user_education_items":[],"underlying_admin_creator":null,"identity_badges":[],"user_signals_info":null,"is_anonymous":false,"ask_admin_to_post_accept_dialog":null,"can_viewer_approve_post":false,"ask_admin_to_post_author":null,"post_subscription_status_info":null,"profile_location_transparency_subtitle_text_with_entities":null,"profile_location_transparency_subtitle_tap_action":null,"soft_impersonation_subtitle_string":null,"soft_impersonation_subtitle_tap_action":null,"reshare_warning_action":null,"reshare_warning_action_module_type":null,"attachment_link_url":"","follow_intervention_action":null,"can_show_upsell_header":true,"friend_deep_dive_availability":{"has_single_friend_stories":false},"ongoing_crisis_info":null,"lightweight_negative_feedback_tooltip":null,"should_show_easy_hide":false,"sharesheet_type_override":"DEFAULT","product_match_info":{"can_visual_search":false,"admin_toggle_settings":{"show_toggle":false,"toggle_status":null}},"send_from_workplace_to_whatsapp":null,"if_viewer_can_see_commerce_profile":null,"page_seller_rating_and_review_info":null,"narrative_thread_metadata":{"is_eligible_for_thread":false,"position":null,"thread":null},"whatsapp_ad_context":null,"referenced_sticker":null,"text_format_metadata":null,"album":null,"verified_voice_context":null,"branded_content_integrity_context_trigger":null,"subscribed_label_integrity_context_trigger":null,"attachments":[{"title":"","subtitle":null,"snippet":null,"title_with_entities":{"text":"","ranges":[],"delight_ranges":[],"color_ranges":[]},"description":{"image_ranges":[],"text":"Cover me https:\/\/30nama.com\/anime\/92392\/Your-Name-2016\npekh","ranges":[{"offset":9,"length":45,"entity":{"__typename":"ExternalUrl","id":"NjQyMTgzOTU5MjA4MTA3Omh0dHBzXGEvLzMwbmFtYS5jb20vYW5pbWUvOTIzOTIvWW91ci1OYW1lLTIwMTY6Ojo6","strong_id__":"-7943896191095328721","name":"","url":"https:\/\/lm.facebook.com\/l.php?u=https\u00253A\u00252F\u00252F30nama.com\u00252Fanime\u00252F92392\u00252FYour-Name-2016&h=AT18HelLKtCaabW5SovMc4OG8YttGFbHEzsptsX_0tJUEXxDPPJBD3jf4GfxdCVW45_iPkyI8DyubgYDr8G0hnWeP_VZ5m3kdQAE9Bc-IfbidSXOhtOUVLel4WuRacAoD9k&s=1","android_urls":[]}}],"delight_ranges":[]},"xy_tags_label":null,"url":"https:\/\/m.facebook.com\/photo.php?fbid=690224498792138&id=100034137818552&set=a.690223215458933","source":null,"style_list":["photo","games_app","fallback"],"media_reference_token":"pcb.690224538792134","attachment_properties":[{"key":"photoset_reference_token","title":null,"value":{"text":"pcb.690224538792134","ranges":[],"delight_ranges":[],"color_ranges":[],"aggregated_ranges":[]},"type":"string"},{"key":"layout_x","title":null,"value":{"text":"0","ranges":[],"delight_ranges":[],"color_ranges":[],"aggregated_ranges":[]},"type":"int"},{"key":"layout_y","title":null,"value":{"text":"0","ranges":[],"delight_ranges":[],"color_ranges":[],"aggregated_ranges":[]},"type":"int"},{"key":"layout_w","title":null,"value":{"text":"1","ranges":[],"delight_ranges":[],"color_ranges":[],"aggregated_ranges":[]},"type":"int"},{"key":"layout_h","title":null,"value":{"text":"1","ranges":[],"delight_ranges":[],"color_ranges":[],"aggregated_ranges":[]},"type":"int"}],"tracking":"{}","action_links":[],"target":{"__typename":"Photo","id":"690224498792138","feedback":{"id":"ZmVlZGJhY2s6NjkwMjI0NTM4NzkyMTM0","if_viewer_can_see_voice_switcher":null,"can_page_viewer_invite_post_likers":null,"can_viewer_react":true,"can_viewer_comment":true,"can_viewer_comment_in_private":false,"can_viewer_comment_with_gif":true,"can_viewer_comment_with_photo":true,"can_viewer_comment_with_sticker":true,"can_viewer_comment_with_video":true,"can_viewer_comment_with_music_no_user_qe_check":false,"can_viewer_set_comment_moderation_filter":true,"is_hide_transparency_enabled_for_actor":false,"can_viewer_subscribe":true,"can_viewer_comment_as_question":false,"cqa_answer_count":null,"does_viewer_like":false,"feedback_target_type":"OTHER","is_viewer_subscribed":true,"is_community_qa_post":false,"is_community_qna_ish_post":false,"legacy_api_post_id":"690224538792134","viewer_acts_as_page":null,"comments_mirroring_domain":null,"owning_profile":{"__typename":"User","id":"100034137818552","name":"Nasrollah Jokar","strong_id__":"100034137818552"},"can_see_voice_indicator":false,"viewer_current_actor":{"__typename":"User","id":"100034137818552","name":"Nasrollah Jokar","short_name":"Nasrollah","profile_picture":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t1.6435-1\/51998503_103731534108107_2613954363776827392_n.jpg?stp=c0.12.111.111a_cp0_dst-jpg_e15_p111x111_q65&_nc_cat=105&ccb=1-5&_nc_sid=7206a8&_nc_ohc=dY4x2D9lDKgAX9YEL_T&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-bEeAGtcrvv7yOIy5Vpiqj4Lf0QopeC77kSZtuEVUbTA&oe=62663279"},"strong_id__":"100034137818552"},"custom_sticker_pack":null,"custom_sticker_pack_nux_content":null,"if_viewer_can_see_community_awards_one_time_nux":null,"default_comment_ordering":"toplevel","top_level_comments":{"count":0,"total_count":0},"likers":{"count":0},"reshares":{"count":0},"reactors":{"count":0},"top_reactions":{"edges":[]},"viewer_feedback_reaction_key":0,"viewer_feedback_reaction_info":null,"supported_reactions":[{"key":1},{"key":2},{"key":16},{"key":4},{"key":3},{"key":7},{"key":8}],"supported_reaction_infos":[{"id":"1635855486666999"},{"id":"1678524932434102"},{"id":"613557422527858"},{"id":"115940658764963"},{"id":"478547315650144"},{"id":"908563459236466"},{"id":"444813342392137"}],"important_reactors":{"nodes":[]},"viewer_acts_as_person":{"id":"100034137818552","name":"Nasrollah Jokar"}},"strong_id__":"690224498792138","android_urls":[],"application":null,"profile_picture":null,"cover_photo":null,"social_context":null,"media":null,"image":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_p50x50_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-TC3hsEQBum850RkeByRuazMKGEYPp7femZekRZOFbvA&oe=6246ADE6","width":50,"height":76},"school":null,"employer":null,"creation_story":{"id":"UzpfSTEwMDAzNDEzNzgxODU1Mjo2OTAyMjQ1Mzg3OTIxMzQ=","cache_id":"-1911695493556300750"},"owner":{"__typename":"User","id":"100034137818552","name":"Nasrollah Jokar","strong_id__":"100034137818552"}},"associated_application":null,"deduplication_key":"9b0bbf9a1020fb0a96d987902b75a555","instagram_user":null,"use_carousel_infinite_scroll":false,"crawled_static_resources":[],"third_party_media_info":null,"is_mute_music_ad":false,"style_infos":[{"__typename":"CollagePhotoAttachmentStyleInfo","layout_x":0,"layout_y":0,"layout_width":1,"layout_height":1,"strong_id__":null},{"__typename":"GamesAppStoryAttachmentStyleInfo"},{"__typename":"FeedStandardAttachmentStyleInfo","enable_body_text":false,"button_styles":[],"headline_styles":[],"metaline_styles":[],"facepile_styles":[]}],"media":{"__typename":"Photo","id":"690224498792138","strong_id__":"690224498792138","image":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_fr_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-O-xMyowGF01_eCPgQu2quOoiQ9gGLbqBVraj7cJzr_A&oe=6246ADE6","width":400,"height":607},"imageLow":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_p370x247_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT9r0Ys2xL2vDdJFOYUc5YVWGDU3i1Z9g7dFYkLh6oiPBQ&oe=6246ADE6","width":370,"height":561},"imageMedium":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_fr_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-O-xMyowGF01_eCPgQu2quOoiQ9gGLbqBVraj7cJzr_A&oe=6246ADE6","width":400,"height":607},"imageHigh":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_fr_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-O-xMyowGF01_eCPgQu2quOoiQ9gGLbqBVraj7cJzr_A&oe=6246ADE6","width":400,"height":607},"focus":{"x":0.5,"y":0.33},"should_block_screenshot":false,"android_urls":[],"open_graph_action":null,"message":{"text":"Cover me https:\/\/30nama.com\/anime\/92392\/Your-Name-2016\npekh","ranges":[{"offset":9,"length":45,"entity":{"__typename":"ExternalUrl","id":"NjQyMTgzOTU5MjA4MTA3Omh0dHBzXGEvLzMwbmFtYS5jb20vYW5pbWUvOTIzOTIvWW91ci1OYW1lLTIwMTY6Ojo6","strong_id__":"-7943896191095328721","name":"","url":"https:\/\/lm.facebook.com\/l.php?u=https\u00253A\u00252F\u00252F30nama.com\u00252Fanime\u00252F92392\u00252FYour-Name-2016&h=AT18HelLKtCaabW5SovMc4OG8YttGFbHEzsptsX_0tJUEXxDPPJBD3jf4GfxdCVW45_iPkyI8DyubgYDr8G0hnWeP_VZ5m3kdQAE9Bc-IfbidSXOhtOUVLel4WuRacAoD9k&s=1","android_urls":[]}}],"delight_ranges":[],"color_ranges":[]},"imageLargeAspect":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_q65_s600x600&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT9ljvmkX7FqRSMBzrZDdSxIrzI0d9PiPD4PWRya7O670Q&oe=6246ADE6","width":395,"height":600},"imageServerSelected":null,"imageSmallServerSelected":null,"imageCarouselServerSelected":{"image_rendering_style":"largesquare","uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_fr_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-O-xMyowGF01_eCPgQu2quOoiQ9gGLbqBVraj7cJzr_A&oe=6246ADE6","width":400,"height":607},"imageFlexible":null,"has_stickers":false,"can_viewer_share":true,"paired_video":null,"owner":{"__typename":"User","name":"Nasrollah Jokar","id":"100034137818552","strong_id__":"100034137818552"},"xy_tags_label":null,"product_tags":[],"product_matches":[],"photo_product_tags":[],"nt_permalink_chaining_photos_vscroll_intent":null,"cix_screen":null,"copyright_block_info":null,"animated_image":null,"is_playable":false,"playable_url":null,"preferredPlayableUrlString":null,"atom_size":0,"bitrate":0,"is_disturbing":false,"playable_duration_in_ms":0,"feedback":{"id":"ZmVlZGJhY2s6NjkwMjI0NTM4NzkyMTM0","if_viewer_can_see_voice_switcher":null,"can_page_viewer_invite_post_likers":null,"can_viewer_react":true,"can_viewer_comment":true,"can_viewer_comment_in_private":false,"can_viewer_comment_with_gif":true,"can_viewer_comment_with_photo":true,"can_viewer_comment_with_sticker":true,"can_viewer_comment_with_video":true,"can_viewer_comment_with_music_no_user_qe_check":false,"can_viewer_set_comment_moderation_filter":true,"is_hide_transparency_enabled_for_actor":false,"can_viewer_subscribe":true,"can_viewer_comment_as_question":false,"cqa_answer_count":null,"does_viewer_like":false,"feedback_target_type":"OTHER","is_viewer_subscribed":true,"is_community_qa_post":false,"is_community_qna_ish_post":false,"legacy_api_post_id":"690224538792134","viewer_acts_as_page":null,"comments_mirroring_domain":null,"owning_profile":{"__typename":"User","id":"100034137818552","name":"Nasrollah Jokar","strong_id__":"100034137818552"},"can_see_voice_indicator":false,"viewer_current_actor":{"__typename":"User","id":"100034137818552","name":"Nasrollah Jokar","short_name":"Nasrollah","profile_picture":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t1.6435-1\/51998503_103731534108107_2613954363776827392_n.jpg?stp=c0.12.111.111a_cp0_dst-jpg_e15_p111x111_q65&_nc_cat=105&ccb=1-5&_nc_sid=7206a8&_nc_ohc=dY4x2D9lDKgAX9YEL_T&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-bEeAGtcrvv7yOIy5Vpiqj4Lf0QopeC77kSZtuEVUbTA&oe=62663279"},"strong_id__":"100034137818552"},"custom_sticker_pack":null,"custom_sticker_pack_nux_content":null,"if_viewer_can_see_community_awards_one_time_nux":null,"default_comment_ordering":"toplevel","top_level_comments":{"count":0,"total_count":0},"likers":{"count":0},"reshares":{"count":0},"reactors":{"count":0},"top_reactions":{"edges":[]},"viewer_feedback_reaction_key":0,"viewer_feedback_reaction_info":null,"supported_reactions":[{"key":1},{"key":2},{"key":16},{"key":4},{"key":3},{"key":7},{"key":8}],"supported_reaction_infos":[{"id":"1635855486666999"},{"id":"1678524932434102"},{"id":"613557422527858"},{"id":"115940658764963"},{"id":"478547315650144"},{"id":"908563459236466"},{"id":"444813342392137"}],"important_reactors":{"nodes":[]},"viewer_acts_as_person":{"id":"100034137818552","name":"Nasrollah Jokar"}},"is_spherical":false,"photo_encodings":[],"immersive_photo_encodings":[],"attribution_app":null,"attribution_app_metadata":null,"show_objectionable_warning_in_feed":false,"objectionable_content_info":null,"render_product_tagging_new_format":true,"xy_tags":{"edges":[]},"tags":{"nodes":[]}},"subattachments":[],"is_video_deep_links":false,"is_animated_deep_links":false,"poll_sticker":null}],"multiShareAttachmentWithImageFields":[],"feedback":{"id":"ZmVlZGJhY2s6NjkwMjI0NTM4NzkyMTM0","if_viewer_can_see_voice_switcher":null,"can_page_viewer_invite_post_likers":null,"can_viewer_react":true,"can_viewer_comment":true,"can_viewer_comment_in_private":false,"can_viewer_comment_with_gif":true,"can_viewer_comment_with_photo":true,"can_viewer_comment_with_sticker":true,"can_viewer_comment_with_video":true,"can_viewer_comment_with_music_no_user_qe_check":false,"can_viewer_set_comment_moderation_filter":true,"is_hide_transparency_enabled_for_actor":false,"can_viewer_subscribe":true,"can_viewer_comment_as_question":false,"cqa_answer_count":null,"does_viewer_like":false,"feedback_target_type":"OTHER","is_viewer_subscribed":true,"is_community_qa_post":false,"is_community_qna_ish_post":false,"legacy_api_post_id":"690224538792134","viewer_acts_as_page":null,"comments_mirroring_domain":null,"owning_profile":{"__typename":"User","id":"100034137818552","name":"Nasrollah Jokar","strong_id__":"100034137818552"},"can_see_voice_indicator":false,"viewer_current_actor":{"__typename":"User","id":"100034137818552","name":"Nasrollah Jokar","short_name":"Nasrollah","profile_picture":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t1.6435-1\/51998503_103731534108107_2613954363776827392_n.jpg?stp=c0.12.111.111a_cp0_dst-jpg_e15_p111x111_q65&_nc_cat=105&ccb=1-5&_nc_sid=7206a8&_nc_ohc=dY4x2D9lDKgAX9YEL_T&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-bEeAGtcrvv7yOIy5Vpiqj4Lf0QopeC77kSZtuEVUbTA&oe=62663279"},"strong_id__":"100034137818552"},"custom_sticker_pack":null,"custom_sticker_pack_nux_content":null,"if_viewer_can_see_community_awards_one_time_nux":null,"viewer_feedback_reaction_key":0,"viewer_feedback_reaction_info":null,"important_reactors":{"nodes":[]},"viewer_acts_as_person":{"id":"100034137818552","name":"Nasrollah Jokar"},"supported_reactions":[{"key":1},{"key":2},{"key":16},{"key":4},{"key":3},{"key":7},{"key":8}],"supported_reaction_infos":[{"id":"1635855486666999"},{"id":"1678524932434102"},{"id":"613557422527858"},{"id":"115940658764963"},{"id":"478547315650144"},{"id":"908563459236466"},{"id":"444813342392137"}],"reactors":{"count":0},"top_reactions":{"edges":[]},"reaction_display_config":{"is_reaction_count_hidden_by_producer":false,"reaction_display_strategy":"NONE","reaction_string_with_viewer":null,"reaction_string_without_viewer":null,"reaction_sheet_explanation_string":null},"like_sentence":null,"viewer_does_not_like_sentence":null,"viewer_likes_sentence":{"text":"You like this.","ranges":[],"delight_ranges":[],"color_ranges":[],"aggregated_ranges":[]},"top_level_comments":{"count":0,"total_count":0},"likers":{"count":0}},"attached_story":null,"attached_story_render_style":"DEFAULT","all_substories":{"remaining_count":0,"nodes":[],"page_info":null},"is_marked_as_spam_by_admin_assistant":false,"followup_feed_units":{"edges":[]}},"story_id":null,"items":[{"story":{"sticker_ads_overlays":[],"id":"UzpfSTEwMDAzNDEzNzgxODU1Mjo2OTAyMjQ1Mzg3OTIxMzQ=","cache_id":"-1911695493556300750","organic_tracking":null,"attachments":[{"style_list":["photo","games_app","fallback"],"media":{"__typename":"Photo","id":"690224498792138","strong_id__":"690224498792138","playable_url":null,"playable_duration_in_ms":0,"stories_photo_uri":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_fr_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-O-xMyowGF01_eCPgQu2quOoiQ9gGLbqBVraj7cJzr_A&oe=6246ADE6"},"stories_photo_size":{"width":400,"height":607},"low_res":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_fr_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-O-xMyowGF01_eCPgQu2quOoiQ9gGLbqBVraj7cJzr_A&oe=6246ADE6"},"message":{"text":"Cover me https:\/\/30nama.com\/anime\/92392\/Your-Name-2016\npekh","ranges":[{"offset":9,"length":45,"entity":{"__typename":"ExternalUrl","id":"NjQyMTgzOTU5MjA4MTA3Omh0dHBzXGEvLzMwbmFtYS5jb20vYW5pbWUvOTIzOTIvWW91ci1OYW1lLTIwMTY6Ojo6","strong_id__":"-7943896191095328721"}}]},"show_objectionable_warning_in_feed":false,"objectionable_content_info":null,"cix_screen":null},"target":{"__typename":"Photo","id":"690224498792138","strong_id__":"690224498792138"},"preview_media":{"__typename":"Photo","id":"690224498792138","strong_id__":"690224498792138","low_res":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_fr_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-O-xMyowGF01_eCPgQu2quOoiQ9gGLbqBVraj7cJzr_A&oe=6246ADE6"}},"action_links":[]}],"actors":[{"__typename":"User","id":"100034137818552","is_viewer_friend":false,"short_name":"Nasrollah","name":"Nasrollah Jokar","structured_name":{"parts":[{"part":"first","offset":0,"length":9},{"part":"last","offset":10,"length":5}],"text":"Nasrollah Jokar"},"profile_picture":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t1.6435-1\/51998503_103731534108107_2613954363776827392_n.jpg?stp=c0.12.111.111a_cp0_dst-jpg_e15_p111x111_q65&_nc_cat=105&ccb=1-5&_nc_sid=7206a8&_nc_ohc=dY4x2D9lDKgAX9YEL_T&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT-bEeAGtcrvv7yOIy5Vpiqj4Lf0QopeC77kSZtuEVUbTA&oe=62663279"},"gender":"MALE","messenger_contact":{"is_employee":false,"wa_connect_status":"WHATSAPP_ADDRESSABLE","restriction_type":"NONE","id":"Y29udGFjdDoxMDAwMzQxMzc4MTg1NTI6MTAwMDM0MTM3ODE4NTUyOjYwMzUzMDI4MzA4MzA4MA=="},"strong_id__":"100034137818552"}],"via":null,"LinkAttachments":[],"message":{"text":"Cover me https:\/\/30nama.com\/anime\/92392\/Your-Name-2016\npekh","ranges":[{"offset":9,"length":45,"entity":{"__typename":"ExternalUrl","id":"NjQyMTgzOTU5MjA4MTA3Omh0dHBzXGEvLzMwbmFtYS5jb20vYW5pbWUvOTIzOTIvWW91ci1OYW1lLTIwMTY6OkRlZmF1bHQ6Og==","strong_id__":"2780734226133869401"}}],"delight_ranges":[]},"can_viewer_delete":true,"creation_time":1648510465,"expiration_time":null,"text_format_metadata":null,"feedback":{"id":"ZmVlZGJhY2s6NjkwMjI0NTM4NzkyMTM0","top_level_comments":{"total_count":0},"top_level_comments_for_preview":{"edges":[]},"comment_count":0,"commenters":{"count":0,"edges":[]},"comments_facepile":[],"has_author_commented":false},"story_overlays":[],"backdrop":{"media":{"__typename":"Photo","id":"690224498792138","strong_id__":"690224498792138","image":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t39.30808-6\/277584686_690224495458805_2284557850467616694_n.jpg?stp=cp0_dst-jpg_e15_fb50_fr_q65&_nc_cat=105&ccb=1-5&_nc_sid=8bfeb9&_nc_ohc=CHLNqgnhj3MAX9U947e&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT9nrP1EVB9IcBaHwRN9eTwh0u7E2qSLiKVxhUX88P2yRQ&oe=6246ADE6"}}},"story_card_seen_state":null,"is_original_story_card_active":false,"thumbnail_card_info":null,"url":"https:\/\/www.facebook.com\/100034137818552\/posts\/690224538792134\/","can_viewer_reshare_from_stories_to_feed":false,"can_viewer_reshare_from_stories_to_sfv":false,"story_card_info":null,"activity_description":null,"story_activity_description":null,"story_default_background":null,"inline_activities":{"edges":[]},"with_tags":{"nodes":[]},"explicit_place":null,"negative_feedback_actions":{"nodes":[]},"action_links":[{"__typename":"ShareActionLink","title":"Share","url":null,"strong_id__":null}],"can_viewer_reshare_to_story":true,"can_viewer_share_to_messenger":true,"is_eligible_for_mention_story_reshare":false,"post_promotion_info":null,"copyright_banner_info":null,"media_attribution_elements":[],"story_inspiration_attribution_type":null,"story_inspiration_attribution_link_metadata":null,"story_inspiration_music_attribution_extra_metadata":null,"story_inspiration_motion_attribution_extra_metadata":null}}],"logging_token":"SU5URVJOQUxfTE9HR0lOR19JRFNfeyJmZWVkX2ZiaWRzIjpbIjY5MDIyNDUzODc5MjEzNCJdfQ=="}},"extensions":{"fulfilled_payloads":[{"label":"NegativeFeedbackActionsFragment","path":["story_create","story"]}],"server_metadata":{"request_start_time_ms":1648510465882,"time_at_flush_ms":1648510469539},"is_final":false}}
                //{"label":"FBStoryDeferredOverlays","path":["story_create","items",0,"story"],"data":{"sticker_ads_overlays":[],"cache_id":"-1911695493556300750"},"extensions":{"is_final":true}}
                return result.Succeeded ? Result.Success(result.Value.Data.StoryCreate) : Result.Fail<FacebookStoryCreate>(result.Info, default);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookStoryCreate), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookStoryCreate>(exception, responseContent);
            }
        }

        public async Task<IResult<FacebookPaginationResultResponse<FacebookLinkPreviewDataResponse>>> 
            CheckLinkAsync(Uri url, string composerId = null)
        {
            if (string.IsNullOrEmpty(composerId))
                composerId = Guid.NewGuid().ToString();

            var ntContext = new JObject
            {
                {"using_white_navbar", true},
                {"styles_id", FacebookApiConstants.FACEBOOK_STYLES_ID},
                {"pixel_ratio", 3},
                {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
            };
            var variables = new JObject
            {
                {"params", new JObject
                    {
                        {"url", url.ToString() },
                        {"composer_session_id", composerId},
                    }
                },
                {"scale", "3"},
                {"nt_context", ntContext},
            };
            //client_doc_id=8959865055601635935340152837&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //variables={"params":{"url":"https://30nama.com/anime/12976/Attack-on-Titan-2013","composer_session_id":"91b4f0b4-1d7d-4292-a8e5-c31ab8a00494"},"scale":"3","nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"24127905fff357472331453e654da67a","bloks_version":"55eee11487f8abefe1258fb33b90535678f9445b1c17ee77645706a2592e95c5"}}

            //&fb_api_req_friendly_name=ComposerLinkPreviewQuery&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=[\"GraphServices\"]&
            //server_timestamps=true
            return await _facebookApi.SendRequestAsync<FacebookPaginationResultResponse<FacebookLinkPreviewDataResponse>,
                   FacebookPaginationResultResponse<FacebookLinkPreviewDataResponse>>
                   ("8959865055601635935340152837",
                   "post",
                   "ComposerLinkPreviewQuery",
                   "[\"GraphServices\"]",
                   variables.ToString(Formatting.None),
                   null,
                   null,
                   null,
                   null,
                   true,
                   true);
        }

        public async Task<IResult<FacebookStoryCreate>> MakeNewPostAsync(string caption, bool disableComments)
        {
            var result = await ConfigureUploadPhoto(caption, null, Guid.NewGuid().ToString(), false, disableComments);

            return result.Succeeded ? Result.Success(result.Value.Data.StoryCreate) : Result.Fail<FacebookStoryCreate>(result.Info, default);
        }

        private async Task<IResult<FacebookPaginationResultResponse<FacebookStoryCreateData>>> ConfigureUploadPhoto(string caption,
            string uploadId,
            string composerId,
            bool hasPhoto = true, 
            bool disableComments = false)
        {
            var ses = 5.GetGuid();
            var time = DateTimeHelper.ToUnixTimeAsDouble(DateTime.UtcNow).ToString();
            var mutationId = Guid.NewGuid().ToString();

            JObject attachment = null;
            if (hasPhoto)
            {
                attachment = new JObject
                {
                    {"photo",  new JObject
                        {
                            {"story_media_audio_data", new JObject { { "raw_media_type", "PHOTO" } } },
                            {"media_source_info", new JObject
                                {
                                    { "source", "UPLOADED" },
                                    { "entry_point", "PUBLISHER_BAR" },
                                }
                            },
                            {"id", uploadId},
                        }
                    }
                };
            }
            else
            {
                string link = null;
                string shareScrapeData = null;
                var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                foreach (Match m in linkParser.Matches(caption))
                { 
                    link = m.Value;
                    if (!string.IsNullOrEmpty(link))
                        break;
                }

                if (!string.IsNullOrEmpty(link))
                {
                    var checkLink = await CheckLinkAsync(new Uri(link));
                    if(checkLink.Succeeded && checkLink.Value?.Data?.LinkPreview != null)
                    {
                        shareScrapeData = checkLink.Value.Data.LinkPreview.ShareScrapeData;
                    }
                    attachment = new JObject
                    {
                        {"link",  new JObject
                            {
                                {"share_scrape_data", shareScrapeData},
                            }
                        }
                    };
                }
            }
            var input = new JObject
            {
                //"video_start_time_ms": 0,
                //"producer_supported_features": [ "LIGHTWEIGHT_REPLY" ],
                //"tag_expansion_metadata": { "tag_expansion_ids": [] },
                //"place_attachment_setting": "SHOW_ATTACHMENT",
                //"past_time": { "time_since_original_post": 5 },
                //"navigation_data": { "attribution_id_v2": "FeedFiltersFragment,native_newsfeed,tap_top_jewel_bar,1648170493.133,185634492,4748854339," },
                //"message": { "text": "Hello\nhttp://microsoft.com\n\n" },
                //"logging": { "composer_session_id": "7ca952a9-9c98-45ec-8133-f05d0c66626f" },
                {"video_start_time_ms", 0},
                {"producer_supported_features", new JArray("LIGHTWEIGHT_REPLY")},
                {"tag_expansion_metadata", new JObject() { { "tag_expansion_ids", new JArray()} } },
                {"place_attachment_setting", "SHOW_ATTACHMENT"},
                {"past_time", new JObject() { { "time_since_original_post", 5} } },
                //{"navigation_data", new JObject() { { "attribution_id_v2", $"FeedFiltersFragment,native_newsfeed,tap_top_jewel_bar,{time},185634492,4748854339," } } },
                {"navigation_data", new JObject() { { "attribution_id_v2", $"FeedFiltersFragment,native_newsfeed,tap_top_jewel_bar,{time},4748854339," } } },
                {"message", new JObject() { { "text", caption} } },
                {"logging", new JObject() { { "composer_session_id", composerId} } },
                {"camera_post_context", new JObject
                    {
                        {"source", "COMPOSER"},
                        {"platform", "FACEBOOK"},
                        {"deduplication_id", composerId},
                    }
                },
                //"connection_class": "EXCELLENT",
                //"is_welcome_to_group_post": false,
                //"is_throwback_post": "NOT_THROWBACK_POST",
                //"is_boost_intended": false,
                //"reshare_original_post": "SHARE_LINK_ONLY",
                //"idempotence_token": "FEED_7ca952a9-9c98-45ec-8133-f05d0c66626f",
                //"composer_type": "status",
                //"composer_source_surface": "newsfeed",
                
                {"connection_class", "EXCELLENT"},
                {"is_welcome_to_group_post", false},
                {"is_throwback_post", "NOT_THROWBACK_POST"},
                {"is_boost_intended", false},
                {"reshare_original_post", "SHARE_LINK_ONLY"},
                {"idempotence_token", $"FEED_{composerId}"},
                {"composer_type", "status"},
                {"composer_source_surface", "newsfeed"},
                //"is_group_linking_post": false,
                //"looking_for_players": { "selected_game": "" },
                //"implicit_with_tags_ids": [],
                //"composer_entry_point": "publisher_bar_photo",
                //"extensible_sprouts_ranker_request": { "RequestID": "fvBvCwABAAAAJDBmYjIwOTUxLTZmY2MtNGUzNS04NzY4LWZkMzg2YWJjYjllYgoAAgAAAABiPP1+CwADAAAAG0NPTlRBQ1RfWU9VUl9SRVBSRVNFTlRBVElWRQYABAAWCwAFAAAAGVVORElSRUNURURfUEFHRVNfQ09NUE9TRVIA" },
                //"is_tags_user_selected": false,
                //"composer_entry_picker": "media_picker",
                //"client_mutation_id": "44796d2d-e860-4dc0-9cc3-d6316f65d2bf",
                {"is_group_linking_post", false},
                {"looking_for_players", new JObject() { { "selected_game", ""} } },
                {"implicit_with_tags_ids", new JArray()},
                {"composer_entry_point", "publisher_bar_photo"},
                {"is_tags_user_selected", false},
                {"composer_entry_picker", "media_picker"},
                {"client_mutation_id", mutationId},
                //"audiences": [
                //  {
                //    "undirected": {
                //      "privacy": {
                //        "tag_expansion_state": "UNSPECIFIED",
                //        "deny": [],
                //        "base_state": "EVERYONE",
                //        "allow": []
                //      }
                //    }
                //  }
                //],
                {"audiences", new JArray(
                    new JObject
                    {
                        {"undirected",  new JObject
                            {
                                 {"privacy",  new JObject()
                                     {
                                        {"tag_expansion_state", "UNSPECIFIED"},
                                        {"deny", new JArray()},
                                        {"base_state", "EVERYONE"},
                                        {"allow", new JArray()},
                                     }
                                 }
                            }
                        }
                    }
                )},
                //"source": "MOBILE",
                //"actor_id": "100007855531564",
                //"audiences_is_complete": true,
                {"source", "MOBILE"},
                {"actor_id", User.LoggedInUser.UId.ToString()},
                {"audiences_is_complete", true},
                //"attachments": [
                //  {
                //    "photo": {
                //      "story_media_audio_data": { "raw_media_type": "PHOTO" },
                //      "media_source_info": {
                //        "source": "UPLOADED",
                //        "entry_point": "PUBLISHER_BAR"
                //      },
                //      "id": "3196727743932379"
                //    }
                //  }
                //],
                //{"attachments", new JArray(attachment)},

                //"action_timestamp": 1648170560,
                //"composer_session_events_log": {
                //"number_of_keystrokes": 60,
                //"number_of_copy_pastes": 0,
                //"composition_duration": 44
                //}
                {"action_timestamp", DateTime.UtcNow.ToUnixTime()},
                {"composer_session_events_log", new JObject
                    {
                       {"number_of_keystrokes", 60},
                       {"number_of_copy_pastes", 0 },
                       {"composition_duration", 44},
                    }
                },
            };
            if (attachment == null)
            {
                input.Add("attachments", attachment == null ? new JArray() : new JArray(attachment));
            }
            var variables = new JObject
            {
                {"image_low_height", 2048},
                {"image_medium_width", 540},
                {"automatic_photo_captioning_enabled", "false"},
                {"angora_attachment_profile_image_size", 110},
                {"poll_facepile_size", 110},
                {"default_image_scale", "3"},
                {"image_high_height", 2048},
                {"image_large_aspect_height", 565},
                {"image_large_aspect_width", 1080},
                {"reading_attachment_profile_image_width", 248},
                {"image_low_width", 360},
                {"media_type", "image/jpeg"},
                {"input", input},
                {"nt_context", new JObject
                    {
                        {"using_white_navbar", true},
                        {"pixel_ratio", 3},
                        {"styles_id", FacebookApiConstants.FACEBOOK_STYLES_ID},
                        {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
                    }
                },
                {"size_style", "contain-fit"},
                //"image_high_width": 1080,
                //"poll_voters_count": 5,
                //"action_location": "feed",
                //"reading_attachment_profile_image_height": 371,
                //"include_image_ranges": true,
                //"profile_image_size": 110,
                //"enable_comment_shares": true,
                //"profile_pic_media_type": "image/x-auto",
                //"angora_attachment_cover_image_size": 1320,
                {"image_high_width", 1080},
                {"poll_voters_count", 5},
                {"action_location", "feed"},
                {"reading_attachment_profile_image_height", 371},
                {"include_image_ranges", true},
                {"profile_image_size", 110},
                {"enable_comment_shares", true},
                {"profile_pic_media_type", "image/x-auto"},
                {"angora_attachment_cover_image_size", 1320},
                //"question_poll_count": 100,
                //"image_medium_height": 2048,
                //"enable_comment_reactions_icons": true,
                //"enable_ranked_replies": "true",
                //"fetch_fbc_header": true,
                //"enable_comment_replies_most_recent": "true",
                //"fetch_whatsapp_ad_context": true,
                //"enable_comment_reactions": true,
                //"bloks_version": "7d7c2eae7a06dc8d95a66c8ab896354b9d0c372610859c5ca5c8c2715f9e5bc7",
                //"max_comment_replies": 3
                
                {"question_poll_count", 100},
                {"image_medium_height", 2048},
                {"enable_comment_reactions_icons", true},
                {"enable_ranked_replies", "true"},
                {"fetch_fbc_header", true},
                {"enable_comment_replies_most_recent", "true"},
                {"fetch_whatsapp_ad_context", true},
                {"enable_comment_reactions", true},
                {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
                {"max_comment_replies", 3},
            };

            //[
            //  "nav_attribution_id={\"0\":{\"bookmark_id\":\"4748854339\",\"session\":\"2152c\",\"subsession\":4,\"timestamp\":\"1648170493.133\",\"tap_point\":\"cold_start\",\"most_recent_tap_point\":null,\"bookmark_type_name\":null,\"fallback\":false}}",
            //  "GraphServices",
            //  "visitation_id=4748854339:2152c:4:1648170493.133",
            //  "surface_hierarchy=NewsFeedFragment,native_newsfeed,null;FeedFiltersFragment,native_newsfeed,null;FbChromeFragment,null,cold_start;FbMainTabActivity,unknown,null",
            //  "session_id=UFS-a84ffbd9-33b2-4a39-a68a-9df12caf809e-fg-8"
            //]
            var graphService = new JArray(
                "nav_attribution_id={\"0\":{\"bookmark_id\":\"4748854339\",\"session\":\"%SESSION%\",\"subsession\":4,\"timestamp\":\"%TIME%\",\"tap_point\":\"cold_start\",\"most_recent_tap_point\":null,\"bookmark_type_name\":null,\"fallback\":false}}"
                .Replace("%SESSION%", ses).Replace("%TIME%", time),
                "GraphServices",
                $"visitation_id=4748854339:{ses}:4:{time}",
                "surface_hierarchy=NewsFeedFragment,native_newsfeed,null;FeedFiltersFragment,native_newsfeed,null;FbChromeFragment,null,cold_start;FbMainTabActivity,unknown,null",
                $"session_id=UFS-{Guid.NewGuid()}-fg-8"
                );

            var result = await _facebookApi.SendRequestAsync<FacebookPaginationResultResponse<FacebookStoryCreateData>,
                FacebookPaginationResultResponse<FacebookStoryCreateData>>
                ( hasPhoto ? "91093790615628325179489167800" : "91093790617846848559363590473",
                "post",
                "ComposerStoryCreateMutation",
                graphService.ToString(Formatting.None),
                variables.ToString(Formatting.None),
                null,
                null,
                null,
                null,
                true,
                true);

            if (disableComments && result.Succeeded && result.Value?.Data?.StoryCreate?.Story != null)
            {
                var story = result.Value.Data.StoryCreate.Story;

                await DisableCommentsAsync(story.PostId, story.Id);
            }

            return result;
        }
    }
}

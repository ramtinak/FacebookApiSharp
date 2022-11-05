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
using System.Threading.Tasks;

namespace FacebookApiSharp.API.Processors
{
    internal class AccountProcessor : IAccountProcessor
    {
        private readonly HttpHelper _httpHelper;
        private readonly UserSessionData User;
        private readonly IFacebookLogger _logger;
        private readonly AndroidDevice _deviceInfo;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly FacebookApi _facebookApi;
        public AccountProcessor(AndroidDevice deviceInfo,
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

        public async Task<IResult<bool>> LogoutSessionAsync(string sessionId)
        {
            //{
            //  "input": {
            //    "client_mutation_id": "1",
            //    "actor_id": "100034137818552",
            //    "story_id": "UzpfSTE0MTYxMDc0MDg2Mzk0NjNfMV8xMDAwMzQxMzc4MTg1NTJfYVRveE5qUTJOekV5TVRrMk5EVTJNakkwT3c9PV8xMDExXzBfUkRveE9udHpPakkxT2lKMGFXMWxiR2x1WlY5MmFYTnBZbWxzYVhSNVgzUnZhMlZ1SWp0cE9qRXdNREF6TkRFek56Z3hPRFUxTWp0OV8xNjQ2NzEyMTk2",
            //    "story_location": "ACTIVITY_LOG",
            //    "action": "DELETE",
            //    "category_key": "YOURRECORDS",
            //    "deletion_request_id": "D_R:0:2d2e5847-ec0f-495b-9e66-e948a32fd029",
            //    "post_id_str": null
            //  }
            //}

            var variables = new JObject
            {
                {"input", new JObject
                    {
                        {"client_mutation_id", "1"},
                        {"actor_id", User.LoggedInUser.UId.ToString()},
                        {"story_id", sessionId},
                        {"story_location", "ACTIVITY_LOG"},
                        {"action", "DELETE"},
                        {"category_key", "YOURRECORDS"},
                        {"deletion_request_id", $"D_R:0:{Guid.NewGuid()}"},
                        {"post_id_str", null},
                    }
                },
            };

            //access_token=EAAAAUaZA8jlABAJDPHz5n41ufkL6UQ1s1ClQmPJ6gqiDZC04uZChZAqQRK3XNyhTGnUbOp0PKfjHLBXKeIcKmUPNVPUriOqEoRdZCqmYLKCjrFR68SZBcjtHXRLRi2Rr43Q3Jcw4iD3KxsZAb7St40ZBRFkkwZBZCTzkb3ipjZBnp0zgieMp9VVtFTEDYbG0kAgAJ0ZD&

            //fb_api_caller_class=RelayModern&
            //fb_api_req_friendly_name=ActivityLogStoryCurationMutation&
            //variables={"input":{"client_mutation_id":"1","actor_id":"100034137818552","story_id":"UzpfSTE0MTYxMDc0MDg2Mzk0NjNfMV8xMDAwMzQxMzc4MTg1NTJfYVRveE5qUTJOekV5TVRrMk5EVTJNakkwT3c9PV8xMDExXzBfUkRveE9udHpPakkxT2lKMGFXMWxiR2x1WlY5MmFYTnBZbWxzYVhSNVgzUnZhMlZ1SWp0cE9qRXdNREF6TkRFek56Z3hPRFUxTWp0OV8xNjQ2NzEyMTk2","story_location":"ACTIVITY_LOG","action":"DELETE","category_key":"YOURRECORDS","deletion_request_id":"D_R:0:2d2e5847-ec0f-495b-9e66-e948a32fd029","post_id_str":null}}

            //&server_timestamps=true&
            //doc_id=1694179450603018&
            //fb_api_analytics_tags=["visitation_id=295535957817803:f93ef:1:1648513182.484|281710865595635:3d3e6:1:1648513006.169","session_id=UFS-8068fb18-2bfe-921a-1f93-d9af0b19d750-fg-2","nav_attribution_id={\"0\":{\"bookmark_id\":\"295535957817803\",\"session\":\"f93ef\",\"subsession\":1,\"timestamp\":\"1648513182.484\",\"tap_point\":\"tap_bookmark\",\"most_recent_tap_point\":\"tap_bookmark\",\"bookmark_type_name\":\"settings\",\"fallback\":false},\"1\":{\"bookmark_id\":\"281710865595635\",\"session\":\"3d3e6\",\"subsession\":1,\"timestamp\":\"1648513006.169\",\"tap_point\":\"tap_top_jewel_bar\",\"most_recent_tap_point\":\"tap_top_jewel_bar\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}"]

            //[

            //  "visitation_id=295535957817803:f93ef:1:1648513182.484|281710865595635:3d3e6:1:1648513006.169",
            //  "session_id=UFS-8068fb18-2bfe-921a-1f93-d9af0b19d750-fg-2",
            //  "nav_attribution_id={\"0\":{\"bookmark_id\":\"295535957817803\",\"session\":\"f93ef\",\"subsession\":1,\"timestamp\":\"1648513182.484\",\"tap_point\":\"tap_bookmark\",\"most_recent_tap_point\":\"tap_bookmark\",\"bookmark_type_name\":\"settings\",\"fallback\":false},\"1\":{\"bookmark_id\":\"281710865595635\",\"session\":\"3d3e6\",\"subsession\":1,\"timestamp\":\"1648513006.169\",\"tap_point\":\"tap_top_jewel_bar\",\"most_recent_tap_point\":\"tap_top_jewel_bar\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}"
            //]
            var ses = 5.GetGuid();
            var time = DateTimeHelper.ToUnixTimeAsDouble(DateTime.UtcNow).ToString();
            var graphService = new JArray(
     $"visitation_id=295535957817803:{ses}:1:{time}",
     $"session_id=UFS-{Guid.NewGuid()}-fg-8",
     "nav_attribution_id={\"0\":{\"bookmark_id\":\"295535957817803\",\"session\":\"%SESSION%\",\"subsession\":1,\"timestamp\":\"%TIME%\",\"tap_point\":\"tap_bookmark\",\"most_recent_tap_point\":null,\"bookmark_type_name\":null,\"fallback\":false}}"
     .Replace("%SESSION%", ses).Replace("%TIME%", time)
     );

            var result = await _facebookApi.SendRequestAsync<FacebookPaginationResultResponse<FacebookLogoutData>, FacebookPaginationResultResponse<FacebookLogoutData>>
                ("%DELETE%",
                "post",
                "ActivityLogStoryCurationMutation",
                graphService.ToString(Formatting.None),
                variables.ToString(Formatting.None),
                null,
                UriCreator.GetGraphQLUri().AddQueryParameter("locale", _facebookApi.AppLocale.Replace("-", "_")),
                new Dictionary<string, string>                 
                {
                    { "access_token", User.LoggedInUser.AccessToken } ,
                    { "doc_id", "1694179450603018" } ,
                },
                new Dictionary<string, string>
                {
                    { FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "RelayFBNetwork_ActivityLogStoryCurationMutation" },
                    { FacebookApiConstants.HEADER_FB_REQUEST_ANALYTICS_TAGS, "unknown" },
                    //{ "privacy_context", "react_ActivityLogRoute" },
                },
                false,
                true,
                "RelayModern",
                true,
                true);

            return result.Succeeded && (result.Value?.Data?.ActivityLogStoryCuration?.Story.Id == sessionId) ?
                Result.Success(true) : Result.Fail<bool>(result.Info.Message);
        }
        public async Task<IResult<FacebookLoginActivityStoriesResponse>> GetLoginSessionsAsync(PaginationParameters pagination)
        {
            var loginActivity = new FacebookLoginActivityStoriesResponse();
            try
            {
                if (pagination == null)
                    pagination = PaginationParameters.MaxPagesToLoad(1);

                var result = await GetLoginSessions(pagination);
                if (!result.Succeeded)
                    return Result.Fail(result.Info, loginActivity);

                pagination.PagesLoaded++;
                loginActivity = result.Value;

                pagination.EndCursor = loginActivity.PageInfo?.EndCursor;
                pagination.HasPreviousPage = loginActivity.PageInfo?.HasPreviousPage ?? false;
                while (pagination.HasPreviousPage
                   && !string.IsNullOrEmpty(pagination.EndCursor)
                   && pagination.PagesLoaded <= pagination.MaximumPagesToLoad)
                {

                    var nextPage = await GetLoginSessions(pagination);
                    if (!nextPage.Succeeded)
                        return Result.Fail(nextPage.Info, loginActivity);

                    loginActivity.Edges.AddRange(nextPage.Value.Edges);

                    pagination.EndCursor = nextPage.Value?.PageInfo?.EndCursor;
                    pagination.HasPreviousPage = nextPage.Value?.PageInfo?.HasPreviousPage ?? false;
                    pagination.PagesLoaded++;
                }

                return Result.Success(loginActivity);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, loginActivity, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail(exception, loginActivity);
            }
        }

        private async Task<IResult<FacebookLoginActivityStoriesResponse>> GetLoginSessions(PaginationParameters pagination)
        {

            //{
            //  "cursor": null,
            //  "count": null,
            //  "category_key": "YOURRECORDS",
            //  "end_date": null,
            //  "entry_point": null,
            //  "person_id": null,
            //  "privacy": null,
            //  "start_date": null,
            //  "timeline_visibility": null,
            //  "year": null,
            //  "month": null,
            //  "scale": 2.75,
            //  "manage_mode": false,
            //  "activity_history": false,
            //  "on_archive_page": false,
            //  "on_trash_page": false,
            //  "on_manage_activity_page": false,
            //  "has_archive_trash": true,
            //  "has_bulk_selection": true
            //}
            var variables = new JObject
            {      
                //  "cursor": null,
                //  "count": null,
                //  "category_key": "YOURRECORDS",
                //  "end_date": null,
                //  "entry_point": null,
                //  "person_id": null,
                //  "privacy": null,
                //  "start_date": null,
                {"cursor", string.IsNullOrEmpty(pagination.EndCursor) ? null : pagination.EndCursor},
                {"count", !string.IsNullOrEmpty(pagination.EndCursor) ? 30 : (int?)null},
                {"category_key", "YOURRECORDS"},
                {"end_date", null},
                {"entry_point", null},
                {"person_id", null},
                {"privacy", null},
                {"start_date", null},
            //  "timeline_visibility": null,
            //  "year": null,
            //  "month": null,
            //  "scale": 2.75,
            //  "manage_mode": false,
            //  "activity_history": false,
            //  "on_archive_page": false,
            //  "on_trash_page": false,
            //  "on_manage_activity_page": false,
            //  "has_archive_trash": true,
            //  "has_bulk_selection": true
                {"timeline_visibility", null},
                {"year", null},
                {"month", null},
                {"scale", 2.75d},
                {"manage_mode", false},
                {"activity_history", false},
                {"on_archive_page", false},
                {"on_trash_page", false},
                {"on_manage_activity_page", false},
                {"has_archive_trash", true},
                {"has_bulk_selection", true},
            };
            //access_token=&

            //fb_api_caller_class=RelayModern&
            //fb_api_req_friendly_name=ActivityLogSurfaceQuery&
            //variables={"cursor":null,"count":null,"category_key":"YOURRECORDS","end_date":null,"entry_point":null,"person_id":null,"privacy":null,"start_date":null,"timeline_visibility":null,"year":null,"month":null,"scale":2.75,"manage_mode":false,"activity_history":false,"on_archive_page":false,"on_trash_page":false,"on_manage_activity_page":false,"has_archive_trash":true,"has_bulk_selection":true}


            //&server_timestamps=true&doc_id=4754972177899300

        //FacebookLoginActivityDataResponse
            var result = await _facebookApi.SendRequestAsync<FacebookPaginationResultResponse<FacebookLoginActivityDataResponse>, FacebookPaginationResultResponse<FacebookLoginActivityDataResponse>>
                ("%DELETE%",
                "post",
                "ActivityLogSurfaceQuery",
                "%DELETE%",
                variables.ToString(Formatting.None),
                null, 
                UriCreator.GetGraphQLUri().AddQueryParameter("locale", _facebookApi.AppLocale.Replace("-", "_")),
                new Dictionary<string, string> 
                { 
                    { "access_token", User.LoggedInUser.AccessToken } ,
                    { "doc_id", "4754972177899300" } ,
                }, 
                new Dictionary<string, string> 
                { 
                    { FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "RelayFBNetwork_ActivityLogSurfaceQuery" },
                    { FacebookApiConstants.HEADER_FB_REQUEST_ANALYTICS_TAGS, "unknown" },
                    { "privacy_context", "react_ActivityLogRoute" },
                }, 
                false, 
                true,
                "RelayModern",
                true,
                true
                );
            return result.Succeeded ? 
                Result.Success(result.Value?.Data?.Viewer?.ActivityLogActor?.Stories) :
                Result.Fail<FacebookLoginActivityStoriesResponse>(result.Info.Message);
        }



        public async Task<IResult<bool>> GetBookmarksQueryAsync()
        {

            //X-FB-Privacy-Context: c0000000c09dc946
            //X-FB-Friendly-Name: BookmarksQuery

            var variables = new JObject
            {
                {"profile_image_small_size", 110},
                {"should_fetch_favorites_folder", false},
                {"nt_context", new JObject
                    {
                        {"using_white_navbar", true},
                        {"styles_id", FacebookApiConstants.FACEBOOK_STYLES_ID},
                        {"pixel_ratio", 3},
                        {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
                    }
                },
                {"scale", "3"},
                {"should_fetch_community_resources_folder", true},
                {"should_fetch_plaza_nt_tiles", true},
                {"should_fetch_plaza_nt_sections", true},
                {"query_source", "PLAZA"},
                {"should_fetch_additional_profiles_count", true},
                {"locale", _facebookApi.AppLocale.Replace("-","_")},
            };

            //client_doc_id=287503529816245100337685272847&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //purpose=refresh&
            //variables={"profile_image_small_size":110,"should_fetch_favorites_folder":false,"nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"5e0de59125eab28ac26cc293eeffba01","bloks_version":"7d7c2eae7a06dc8d95a66c8ab896354b9d0c372610859c5ca5c8c2715f9e5bc7"},"scale":"3","should_fetch_community_resources_folder":true,"should_fetch_plaza_nt_tiles":true,"should_fetch_plaza_nt_sections":true,"query_source":"PLAZA","should_fetch_additional_profiles_count":true,"locale":"en_US"}&
            //fb_api_req_friendly_name=BookmarksQuery&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["GraphServices"]&
            //server_timestamps=true

            // {"data":{"viewer":{"featuredBookmarkFolder":{"name":"Featured","id":"1877127739228850","description":null,"bookmarks":{"visible_count":0,"edges":[],"tracking":null,"nt_plaza_section":{"nt_view":{"logging_id":"{\"callsite\":\"{\\\"product\\\":\\\"navigation_plaza\\\",\\\"feature\\\":\\\"plaza_section_1877127739228850\\\",\\\"oncall\\\":\\\"pacman\\\"}\",\"push_phase\":\"C3\",\"version\":1}","data_diff_item_id":"e650370764db605e0b19cbd2076d3b9b","data_diff_content_id":"e650370764db605e0b19cbd2076d3b9b__:__8560f18ce2f37b362f81679064c92686","unique_id":"w1hr5i692u:1","native_template_bundles":[{"nt_bundle_tree":"{\"\\\"\":\"8\",\";\":\"column\",\"n\":100,\" \":[{\"\\\"\":\"8\",\"`\":16,\" \":[{\"\\\"\":\"8\",\";\":\"column\",\"n\":100,\" \":[{\"\\\"\":\"8\",\"b\":16,\"d\":16,\"n\":100,\" \":[{\"\\\"\":\"8\",\"n\":100,\" \":[{\"\\\"\":\"\\uc97c\",\".\":[{\"\\\"\":\"\\uc97d\",\"*\":\"__ntattrp__:1\"}],\"0\":\"LEVEL_4\",\"I\":[{\"\\\"\":\"\\uccb8\"}]}]}]}]}],\"!\":\"w1hpii:18\"}]}","nt_bundle_attributes":[{"__typename":"NTTextWithEntitiesAttribute","name":"1","strong_id__":null,"twe_value":{"text":"All shortcuts","ranges":[]},"twe_for_feed_message_value":null}],"nt_bundle_state":null,"nt_bundle_default_state":null,"nt_bundle_referenced_states":[],"nt_bundle_client_defined_states":null,"nt_follow_up_map":[]}],"bloks_root_component":null},"position":"BELOW_NATIVE"}}},"identityBookmarkFolder":{"name":"Profile","id":"370399423318783","description":null,"bookmarks":{"visible_count":1,"edges":[{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjM3MDM5OTQyMzMxODc4MzoxMDAwMzQxMzc4MTg1NTI6Ojo6dW5rbm93bg==","name":"Nasrollah Jokar","url":"fb:\/\/profile\/","icon_uri":null,"image":{"uri":"https:\/\/scontent-lcy1-1.xx.fbcdn.net\/v\/t1.6435-1\/51998503_103731534108107_2613954363776827392_n.jpg?stp=c0.52.480.480a_cp0_dst-jpg_e15_p480x480_q65&_nc_cat=105&ccb=1-5&_nc_sid=dbb9e7&_nc_ohc=dY4x2D9lDKgAX9YEL_T&_nc_ad=z-m&_nc_cid=0&_nc_ht=scontent-lcy1-1.xx&oh=00_AT_yMpdpeA4jwz8DcSAoTb1HuCrbQKur0Y9QVnGxJ2cCKg&oe=62663279","is_silhouette":false},"bookmarked_node":{"__typename":"User","id":"100034137818552","strong_id__":"100034137818552"},"section":"USER","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":100034137818552,\"bookmark_type\":\"type_self_timeline\",\"position\":0}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}}],"tracking":null,"nt_plaza_section":{"nt_view":{"logging_id":"{\"callsite\":\"{\\\"product\\\":\\\"navigation_plaza\\\",\\\"feature\\\":\\\"plaza_section_370399423318783\\\",\\\"oncall\\\":\\\"pacman\\\"}\",\"push_phase\":\"C3\",\"version\":1}","data_diff_item_id":"e650370764db605e0b19cbd2076d3b9b","data_diff_content_id":"e650370764db605e0b19cbd2076d3b9b__:__a6ec6009b0142ac702a2d0213ba8b423","unique_id":"w1hr5i692u:0","native_template_bundles":[{"nt_bundle_tree":"{\"\\\"\":\"8\",\";\":\"column\",\"f\":4,\"n\":100,\" \":[{\"\\\"\":\"8\",\"`\":8,\" \":[{\"\\\"\":\"8\",\";\":\"column\",\"n\":100,\" \":[{\"\\\"\":\"8\",\"`\":10,\"b\":16,\"d\":16,\"n\":100,\" \":[{\"\\\"\":\"8\",\"n\":100,\" \":[{\"\\\"\":\"\\uc97c\",\".\":[{\"\\\"\":\"\\uc97d\",\"*\":\"__ntattrp__:1\"}],\"0\":\"LEVEL_4\",\"I\":[{\"\\\"\":\"\\uccb8\"}]}]}]},{\"\\\"\":\"8\",\";\":\"column\",\"n\":100,\"[\":101,\" \":[{\"\\\"\":\"\\uc2c0\",\"K\":0,\"L\":0,\"\":[{\"\\\"\":\"\\uc6d0\",\"(\":\" \",\"*\":\"w1hpii:12\",\"&\":[],\"#\":\"w1hpii:14\",\"$\":[{\"\\\"\":\"\\uc432\",\" \":[{\"\\\"\":\"\\uc410\",\"$\":\"{}\",\" \":[{\"\\\"\":\"8\",\";\":\"column\",\"`\":8,\"m\":16,\"[\":101,\" \":[{\"\\\"\":\"8\",\"n\":100,\"G\":70},{\"\\\"\":\"8\",\"m\":4,\" \":[{\"\\\"\":\"\\uc36f\",\"2\":\"FFF0F2F5\",\"\\u00ee\":\"FF18191A\",\":\":12,\"=\":\"CENTER\",\"B\":2,\"C\":\". .\",\"K\":\"END\"}]}],\"(\":0}],\"&\":\"[-1]\"}],\"0\":[{\"\\\"\":\"8\",\":\":\"center\",\";\":\"column\",\"d\":12,\"[\":101,\" \":[{\"\\\"\":\"8\",\"`\":6,\"b\":6,\"d\":6,\"f\":6,\" \":[{\"\\\"\":\"9\",\"e\":60,\">\":60,\" \":[{\"\\\"\":\"\\uc7a0\",\"-\":8,\"\":[{\"\\\"\":\"\\uc64c\",\"$\":\"1\",\"(\":\"w1hpii:14\",\"#\":[\"position\"],\"!\":\"w1hpii:16\"}]}]}]},{\"\\\"\":\"8\",\";\":\"column\",\"Y\":70,\" \":[{\"\\\"\":\"9\",\"e\":60,\">\":10,\" \":[{\"\\\"\":\"\\uc7a0\",\"1\":0,\"-\":5}]},{\"\\\"\":\"8\",\"f\":2,\" \":[{\"\\\"\":\"9\",\"e\":48,\">\":10,\" \":[{\"\\\"\":\"\\uc7a0\",\"1\":1,\"-\":5}]}]}]}]}],\"\":[{\"\\\"\":\"\\uc64c\",\"$\":\"5\",\"(\":\"w1hpii:14\",\"#\":[\"position\"],\"!\":\"w1hpii:15\"}]}],\"!\":\"w1hpii:17\"}]}],\"!\":\"w1hpii:13\",\")\":[{\"\\\"\":\"+\",\" \":[{\"\\\"\":\"X\",\"$\":\"\\\/product_engagement\\\/load_shortcuts_v2?plaza_section_id=\\u00255B\\u002522w1hpii\\u00253A5\\u002522\\u00252Cnull\\u00255D&section_id=\\u00255B\\u002522w1hpii\\u00253A13\\u002522\\u00252Cnull\\u00255D&content\\u00255B0\\u00255D\\u00255Bcontent_provider_id\\u00255D=your_friends&content\\u00255B0\\u00255D\\u00255Bcontent_id\\u00255D=100007855531564&content\\u00255B0\\u00255D\\u00255Bcontent_type\\u00255D=shortcut_entity\",\"-\":\"CANCEL_PENDING\"}],\"$\":1}]}]}],\"!\":\"w1hpii:5\"}]}","nt_bundle_attributes":[{"__typename":"NTTextWithEntitiesAttribute","name":"1","strong_id__":null,"twe_value":{"text":"Your shortcuts","ranges":[]},"twe_for_feed_message_value":null}],"nt_bundle_state":"{\"w1hpii:12\":[{\"__key\":\"-1\",\"position\":-1},{\"__key\":\"0\",\"position\":0},{\"__key\":\"1\",\"position\":1},{\"__key\":\"2\",\"position\":2},{\"__key\":\"3\",\"position\":3},{\"__key\":\"4\",\"position\":4}]}","nt_bundle_default_state":null,"nt_bundle_referenced_states":[],"nt_bundle_client_defined_states":null,"nt_follow_up_map":[]}],"bloks_root_component":null},"position":"BELOW_NATIVE"}}},"productsBookmarkFolder":{"name":"All","id":"1351687314953730","bookmarks":{"visible_count":8,"edges":[{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MTAyNzg0NzY2NDkxOTEwOjo6OnVua25vd24=","name":"Find friends","url":"fb:\/\/friends\/home?source_ref=FIND_FRIENDS_BOOKMARK","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yh\/r\/lQNu8PkUHPc.png","image":null,"bookmarked_node":{"__typename":"Application","id":"102784766491910","strong_id__":"102784766491910"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":102784766491910,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":0}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MjM2MTgzMTYyMjo6Ojp1bmtub3du","name":"Groups","url":"fb:\/\/groups\/tab?source=bookmark","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yX\/r\/L2qlIhGe55v.png","image":null,"bookmarked_node":{"__typename":"Application","id":"2361831622","strong_id__":"2361831622"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":2361831622,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":1}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MjM5Mjk1MDEzNzo6Ojp1bmtub3du","name":"Videos on Watch","url":"fb:\/\/watch\/","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yl\/r\/ob1CgXwDORG.png","image":null,"bookmarked_node":{"__typename":"Application","id":"2392950137","strong_id__":"2392950137"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":2392950137,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":2}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6NTg2MjU0NDQ0NzU4Nzc2Ojo6OnVua25vd24=","name":"Saved","url":"fb:\/\/saved\/?section_name=ALL&referer=29","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/y_\/r\/EygD86pCwoD.png","image":null,"bookmarked_node":{"__typename":"Application","id":"586254444758776","strong_id__":"586254444758776"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":586254444758776,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":3}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MjUwMTAwODY1NzA4NTQ1Ojo6OnVua25vd24=","name":"Pages","url":"fb:\/\/nt_screen\/FB-SCREEN-FB?a=\u00257B\u002522analytics_module\u002522\u00253A\u002522new_launchpoint\u002522\u00252C\u002522title\u002522\u00253A\u002522Pages\u002522\u00252C\u002522hide-search-field\u002522\u00253Atrue\u00252C\u002522pull-to-refresh-enabled\u002522\u00253Atrue\u00257D&p=\u00252Fpages\u00252Fnt_launchpoint_redesign\u00252Fhomescreen\u00252F&q=\u00257B\u002522ref\u002522\u00253A\u002522bookmark\u002522\u00257D&b=\u00255B\u00255D","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/y-\/r\/TbOx5lWkw-N.png","image":null,"bookmarked_node":{"__typename":"Application","id":"250100865708545","strong_id__":"250100865708545"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":250100865708545,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":4}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MjM0NDA2MTAzMzo6Ojp1bmtub3du","name":"Events","url":"fb:\/\/events\/?extra_ref_module=bookmarks_menu","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yj\/r\/qB7KME4oQ7N.png","image":null,"bookmarked_node":{"__typename":"Application","id":"2344061033","strong_id__":"2344061033"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":2344061033,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":5}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6NTEzNzQ2OTkyMTY3Mzc0Ojo6OnVua25vd24=","name":"Gaming","url":"fb:\/\/games\/","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/ys\/r\/63SCnWduL0m.png","image":null,"bookmarked_node":{"__typename":"Application","id":"513746992167374","strong_id__":"513746992167374"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":513746992167374,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":6}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6NTgyNjAyOTQ1MDg3MTQ5Ojo6OnVua25vd24=","name":"Nearby friends","url":"fb:\/\/friendsnearby\/?source=bookmark","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yE\/r\/DDvhSnJybMm.png","image":null,"bookmarked_node":{"__typename":"Application","id":"582602945087149","strong_id__":"582602945087149"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":582602945087149,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":7}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MzAzMjU3NTA2NTQ0MzcwOjo6OnVua25vd24=","name":"Memories","url":"fb:\/\/memories_home\/?source=bookmark","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yp\/r\/24MgFlPfCs5.png","image":null,"bookmarked_node":{"__typename":"Application","id":"303257506544370","strong_id__":"303257506544370"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":303257506544370,\"bookmark_type\":\"type_facebook_app\",\"visible\":true,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":8}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MTE2MzAzMDcxNDEyNDQ4Mzo6Ojp1bmtub3du","name":"Recent & favorites","url":"fb:\/\/feed_filters\/","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yx\/r\/5EcoEbpdMLl.png","image":null,"bookmarked_node":{"__typename":"Application","id":"1163030714124483","strong_id__":"1163030714124483"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":1163030714124483,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":9}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MzAyNjc3NTM2Nzk4NDcwOjo6OnVua25vd24=","name":"Weather","url":"fb:\/\/daily_dialogue_weather_permalink\/?orig_src=bookmark&entrypoint=bookmark","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/ym\/r\/5II62CEPpjX.png","image":null,"bookmarked_node":{"__typename":"Application","id":"302677536798470","strong_id__":"302677536798470"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":302677536798470,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":10}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6NTI4MjE2MjI0MDMzOTEzOjo6OnVua25vd24=","name":"Device requests","url":"fb:\/\/device_requests\/","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yT\/r\/zP9A9sw2Z1Z.png","image":null,"bookmarked_node":{"__typename":"Application","id":"528216224033913","strong_id__":"528216224033913"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":528216224033913,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":11}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MjM1MTM0ODQwMzEyNzAzOjo6OnVua25vd24=","name":"Stories","url":"fbinternal:\/\/storysurface\/","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yA\/r\/C2jUoQEs_K3.png","image":null,"bookmarked_node":{"__typename":"Application","id":"235134840312703","strong_id__":"235134840312703"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":235134840312703,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":12}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MTQ2OTMxMjg5MDY4NzIxOjo6OnVua25vd24=","name":"Find Wi-Fi","url":"fb:\/\/wifi\/?source=bookmark","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yP\/r\/GHnaq-xpbIi.png","image":null,"bookmarked_node":{"__typename":"Application","id":"146931289068721","strong_id__":"146931289068721"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":146931289068721,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":13}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MjgwMDMzODQ1NzYwNjQ1Ojo6OnVua25vd24=","name":"Recent ad activity","url":"fb:\/\/ad_activity\/","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yH\/r\/u6E3qp8lSIK.png","image":null,"bookmarked_node":{"__typename":"Application","id":"280033845760645","strong_id__":"280033845760645"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":280033845760645,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":14}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6MTEzNzc5NDM1OTc4NTY2Ojo6OnVua25vd24=","name":"Avatars","url":"fb-avatar:\/\/fb_avatar_editor\/?surface=bookmarks&mechanism=bookmark_button&post_save_share_option=share_to_feed","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yV\/r\/E57YooF7B3z.png","image":null,"bookmarked_node":{"__typename":"Application","id":"113779435978566","strong_id__":"113779435978566"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":113779435978566,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":15}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6NTI2NzMyNzk0MDE2Mjc5Ojo6OnVua25vd24=","name":"Offers","url":"fb:\/\/nt_screen\/FB-SCREEN-FB?a=\u00257B\u002522analytics_module\u002522\u00253A\u002522offers_bookmark_nt_screen\u002522\u00252C\u002522title\u002522\u00253A\u002522Offers\u002522\u00252C\u002522hide-search-field\u002522\u00253Atrue\u00257D&p=offers\u00252Fnt\u00252Fbookmark\u00252Fscreen\u00252F&q=\u00257B\u002522referrer\u002522\u00253A\u002522offer_bookmark\u002522\u00257D&b=\u00255B\u00255D","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/y5\/r\/Hi_deV2MggB.png","image":null,"bookmarked_node":{"__typename":"Application","id":"526732794016279","strong_id__":"526732794016279"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":526732794016279,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":16}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEzNTE2ODczMTQ5NTM3MzA6NzcxMDc4MTMwMjEzMjEyOjo6OnVua25vd24=","name":"Fantasy Games","url":"fb:\/\/nt_screen\/FB-SCREEN-FB?a=\u00257B\u002522analytics_module\u002522\u00253A\u002522fantasy_games_am_bookmark\u002522\u00252C\u002522hide-search-field\u002522\u00253Atrue\u00257D&p=\u00252Ffantasy_games\u00252Fbookmark&q=\u00257B\u00257D&b=\u00255B\u00255D","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yy\/r\/rSjIV705xzZ.png","image":null,"bookmarked_node":{"__typename":"Application","id":"771078130213212","strong_id__":"771078130213212"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":771078130213212,\"bookmark_type\":\"type_facebook_app\",\"visible\":false,\"ranker\":\"BookmarkOrderingV2\",\"ranking_reason\":\"from_featured_via_SV_BOOKMARK_FEATURED_ORDER\",\"content_eligible\":true,\"bookmark_request_nonce\":\"69bdeae2-2422-4ea3-82e2-992383e32dbe\",\"position\":17}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}}],"tracking":null,"nt_plaza_section":null}},"productsFromFBBookmarkFolder":{"name":"Also from Meta","id":"1383723755291105","bookmark_image":{"uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yo\/r\/clNmVsvG7cF.png"},"folder_should_start_expanded":null,"bookmarks":{"visible_count":0,"edges":[],"tracking":null,"nt_plaza_section":null}},"settingsBookmarkFolder":{"name":"Settings & privacy","id":"310956912693156","description":null,"bookmarks":{"visible_count":0,"edges":[],"tracking":null,"nt_plaza_section":null}},"hubsBookmarkFolder":{"name":"HubsFolder","id":"1060513094758909","bookmark_image":{"uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yd\/r\/RnXz7rNjrF_.png"},"folder_should_start_expanded":null,"bookmarks":{"visible_count":4,"edges":[{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEwNjA1MTMwOTQ3NTg5MDk6NDc0MTcxMTgzMjU5MTc1Ojo6OnVua25vd24=","name":"COVID-19 Information Center","url":"fbinternal:\/\/discovery_hub_screen\/covid_19_info_center?ENABLE_STICKY_SUB_NAV_BAR=0&ENTRYPOINT=bookmark&SERVER_REQUEST_INPUT_DATA=\u00257B\u002522hub\u002522\u00253A\u002522covid_19_info_center\u002522\u00252C\u002522surface_config_id\u002522\u00253Anull\u00252C\u002522sub_hub\u002522\u00253Anull\u00252C\u002522session_id\u002522\u00253A\u0025222de851ed-a76e-401d-97dc-b9b501d3dadf\u002522\u00252C\u002522entry_point\u002522\u00253A\u002522bookmark\u002522\u00252C\u002522entrypoint_data\u002522\u00253Anull\u00252C\u002522hoisted_unit_ids\u002522\u00253A\u00255B1279678602387460\u00252C161945042046412\u00252C953575212046303\u00252C285328603018746\u00255D\u00252C\u002522hoisted_content_ids\u002522\u00253Anull\u00252C\u002522hoisted_fav\u002522\u00253Anull\u00252C\u002522hoisted_position\u002522\u00253A0\u00252C\u002522screen_id\u002522\u00253A\u00255B\u002522w1hpii\u00253A10\u002522\u00252Cnull\u00255D\u00252C\u002522disable_redirect\u002522\u00253Afalse\u00257D&TTRC_QPL_MARKER_ID=78384815&PAGINATION_QPL_MARKER_ID=78385360","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yJ\/r\/kfSgA-GTBf9.png","image":null,"bookmarked_node":{"__typename":"Application","id":"474171183259175","strong_id__":"474171183259175"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":474171183259175,\"bookmark_type\":\"type_facebook_app\",\"position\":0}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEwNjA1MTMwOTQ3NTg5MDk6MTkzMzU2NjUxMDAyMjIzOjo6OnVua25vd24=","name":"Fundraisers","url":"fb:\/\/nt_screen\/FB-SCREEN-FB?a=\u00257B\u002522analytics_module\u002522\u00253A\u002522charitable_giving_fundraiser_hub\u002522\u00252C\u002522title\u002522\u00253A\u002522Facebook+fundraisers\u002522\u00252C\u002522hide-search-field\u002522\u00253Atrue\u00252C\u002522custom-qpl-marker-id\u002522\u00253A47255760\u00252C\u002522pull-to-refresh-enabled\u002522\u00253Atrue\u00257D&p=fundraisers&q=\u00257B\u002522source\u002522\u00253A\u002522app_bookmark\u002522\u00257D&b=\u00255B\u00255D","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yM\/r\/hZ_wPvPD4VY.png","image":null,"bookmarked_node":{"__typename":"Application","id":"193356651002223","strong_id__":"193356651002223"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":193356651002223,\"bookmark_type\":\"type_facebook_app\",\"position\":1}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEwNjA1MTMwOTQ3NTg5MDk6NzY1NTE4NDczNDU5OTY5Ojo6OnVua25vd24=","name":"Crisis response","url":"fb:\/\/nt_screen\/FB-SCREEN-FB?a=\u00257B\u002522analytics_module\u002522\u00253A\u002522crisis_response\u002522\u00252C\u002522hide-search-field\u002522\u00253Atrue\u00252C\u002522navbar-background-color\u002522\u00253A\u002522FFFFFFFF\u002522\u00252C\u002522navbar-background-color-dark\u002522\u00253A\u002522FF242526\u002522\u00252C\u002522navbar-title-color\u002522\u00253A\u002522FF050505\u002522\u00252C\u002522navbar-title-color-dark\u002522\u00253A\u002522FFE4E6EB\u002522\u00252C\u002522hide-navbar-right\u002522\u00253Atrue\u00252C\u002522hide-navbar-shadow\u002522\u00253Atrue\u00257D&p=\u00252Fcrisis\u00252Fbookmark_v2\u00252F&q=\u00257B\u002522source\u002522\u00253A\u002522facebook_bookmark\u002522\u00257D&b=\u00255B\u00255D","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yl\/r\/gN0qQ419Snk.png","image":null,"bookmarked_node":{"__typename":"Application","id":"765518473459969","strong_id__":"765518473459969"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":765518473459969,\"bookmark_type\":\"type_facebook_app\",\"position\":2}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}},{"node":{"social_context_actors_for_facepiles":{"edges":[]},"unread_count":0,"unread_count_string":null,"highlight_nt_component":null,"is_dismissed":false,"id":"Ym9va21hcms6MTAwMDM0MTM3ODE4NTUyOjEwNjA1MTMwOTQ3NTg5MDk6MTQ1NTY0Mjk0MTEwMjQwOjo6OnVua25vd24=","name":"Emotional health","url":"fb:\/\/nt_screen\/FB-SCREEN-FB?a=\u00257B\u002522analytics_module\u002522\u00253A\u002522wellbeing_center\u002522\u00252C\u002522title\u002522\u00253A\u002522\u002522\u00252C\u002522hide-search-field\u002522\u00253Atrue\u00252C\u002522hide-navbar-right\u002522\u00253Atrue\u00252C\u002522hide-navbar\u002522\u00253Atrue\u00252C\u002522custom-qpl-marker-id\u002522\u00253A78396744\u00252C\u002522pull-to-refresh-enabled\u002522\u00253Afalse\u00252C\u002522ios-status-bar-style\u002522\u00253A\u002522DEFAULT\u002522\u00252C\u002522navigation-logging-extra\u002522\u00253A\u002522\u00257B\u00255C\u002522hub_session_id\u00255C\u002522\u00253A\u00255C\u002522d4fb8912-8ea0-4db0-abd7-85030ad0aab4\u00255C\u002522\u00257D\u002522\u00252C\u002522id\u002522\u00253A\u002522w1hpii\u00253A11\u002522\u00257D&p=\u00252Femotional_health\u00252F&q=\u00257B\u002522source\u002522\u00253A\u002522bookmark\u002522\u00252C\u002522entry_point\u002522\u00253A\u002522bookmark\u002522\u00252C\u002522current_session_id\u002522\u00253A\u002522d4fb8912-8ea0-4db0-abd7-85030ad0aab4\u002522\u00257D&b=\u00255B\u00255D","icon_uri":"https:\/\/static.xx.fbcdn.net\/rsrc.php\/v3\/yc\/r\/Qa07IQGXD4p.png","image":null,"bookmarked_node":{"__typename":"Application","id":"145564294110240","strong_id__":"145564294110240"},"section":"FACEBOOK_APP","context_sentence":null,"highlight_wash_color":"transparent","highlight_data":null,"tracking":"{\"bmid\":145564294110240,\"bookmark_type\":\"type_facebook_app\",\"position\":3}","plaza_tile_content_nt":null,"content_ids":[],"content_expiration_time":null,"reportable_content_id":null,"reportable_content_nfx_story_location":null,"server_navigation_route":null}}],"tracking":null,"nt_plaza_section":null}},"actor":{"__typename":"User","profile_switcher_eligible_profiles":{"count":0,"nodes":[]},"id":"100034137818552","strong_id__":"100034137818552"},"is_eligible_for_account_level_settings":true,"additional_profile_creation_eligibility":{"single_owner":{"can_create":false,"explanation":null},"experiments":{"creation_interest_survey":{"bloks_content":null}}}}},"extensions":{"server_metadata":{"request_start_time_ms":1648422241231,"time_at_flush_ms":1648422241613},"is_final":true}}

            return await _facebookApi.SendRequestAsync<bool, bool>
                ("287503529816245100337685272847",
                "post",
                "BookmarksQuery",
                "[\"GraphServices\"]", 
                variables.ToString(Formatting.None),
                null, null, 
                new Dictionary<string, string> { { "purpose", "fetch" } },
                null
                //, 
                //true
                );
        }

        public async Task<IResult<bool>> GetSettingsFrameworkAsync()
        {
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
                        {"nt_context", ntContext},
                        {"path", "/settings/framework/"},
                        {"params", "{\"entry_point\":\"bookmark\"}"},
                        {"extra_client_data", new JObject ()},
                    } 
                },
                {"scale", "3"},
                {"nt_context", ntContext},
            };

            //client_doc_id=221080835218269196007551755610&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //purpose=fetch&
            //variables={"params":{"nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"5e0de59125eab28ac26cc293eeffba01","bloks_version":"7d7c2eae7a06dc8d95a66c8ab896354b9d0c372610859c5ca5c8c2715f9e5bc7"},"path":"/settings/framework/","params":"{\"entry_point\":\"bookmark\"}","extra_client_data":{}},"scale":"3","nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"5e0de59125eab28ac26cc293eeffba01","bloks_version":"7d7c2eae7a06dc8d95a66c8ab896354b9d0c372610859c5ca5c8c2715f9e5bc7"}}&

            //fb_api_req_friendly_name=NativeTemplateScreenQuery&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["GraphServices"]&
            //server_timestamps=true


            return await _facebookApi.SendRequestAsync<bool, bool>
                ("221080835218269196007551755610",
                "post",
                "NativeTemplateScreenQuery",
                "[\"GraphServices\"]",
                variables.ToString(Formatting.None),
                null, 
                null,
                new Dictionary<string, string> { { "purpose", "fetch" } },
                null,false, true);
        }

        public async Task<IResult<bool>> GetNotificationsSettingsAsync()
        {
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
                        {"nt_context", ntContext},
                        {"path", "/notifications/user_settings/"},
                        {"params", "{\"surface\":\"account_settings\"}"},
                        {"extra_client_data", new JObject ()},
                    }
                },
                {"scale", "3"},
                {"nt_context", ntContext},
            };

            //client_doc_id=221080835218269196007551755610&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //purpose=fetch&
            //variables={"params":{"nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"5e0de59125eab28ac26cc293eeffba01","bloks_version":"7d7c2eae7a06dc8d95a66c8ab896354b9d0c372610859c5ca5c8c2715f9e5bc7"},"path":"/notifications/user_settings/","params":"{\"surface\":\"account_settings\"}","extra_client_data":{}},"scale":"3","nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"5e0de59125eab28ac26cc293eeffba01","bloks_version":"7d7c2eae7a06dc8d95a66c8ab896354b9d0c372610859c5ca5c8c2715f9e5bc7"}}&

            //fb_api_req_friendly_name=NativeTemplateScreenQuery&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["GraphServices"]&
            //server_timestamps=true


            return await _facebookApi.SendRequestAsync<bool, bool>
                ("221080835218269196007551755610",
                "post",
                "NativeTemplateScreenQuery",
                "[\"GraphServices\"]",
                variables.ToString(Formatting.None),
                null,
                null,
                new Dictionary<string, string> { { "purpose", "fetch" } },
                new Dictionary<string, string> { { FacebookApiConstants.HEADER_PRIORITY, "u=0" } });
        }

        public async Task<IResult<bool>> DisableNotificationsFor8HoursAsync()
        {
            await GetBookmarksQueryAsync();
            await GetSettingsFrameworkAsync();
            await GetNotificationsSettingsAsync();
            return await DisableNotificationsAsync();
        }

        private async Task<IResult<bool>> DisableNotificationsAsync()
        {
            //{
            //  "input": {
            //    "duration": 28800,
            //    "token": "fyLk5A27kAA:",
            //    "client_mutation_id": "-0b97-475e-9cb0-",
            //    "actor_id": ""
            //  }
            //}&
            var variables = new JObject
            {
                {"input", new JObject
                    {
                        {"duration", 28800},
                        //{"token", User.LoggedInUser.AccessToken},
                        {"client_mutation_id", _deviceInfo.DeviceGuid.ToString()},
                        {"actor_id", User.LoggedInUser.UId.ToString()},
                    }
                },
            };

            //client_doc_id=34554976101548113633120988623&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //variables={"input":{"duration":28800,"token":"fyLk5A27kAA:","client_mutation_id":"27485e36-0b97-475e-9cb0-","actor_id":""}}&
            //fb_api_req_friendly_name=PushNotificationsMuteMutation&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["nav_attribution_id={\"0\":{\"bookmark_id\":\"295535957817803\",\"session\":\"39df6\",\"subsession\":1,\"timestamp\":\"1648170116.402\",\"tap_point\":\"tap_bookmark\",\"most_recent_tap_point\":\"tap_bookmark\",\"bookmark_type_name\":\"settings\",\"fallback\":false},\"1\":{\"bookmark_id\":\"281710865595635\",\"session\":\"7e185\",\"subsession\":2,\"timestamp\":\"1648170111.366\",\"tap_point\":\"tap_top_jewel_bar\",\"most_recent_tap_point\":\"tap_top_jewel_bar\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}","visitation_id=295535957817803:39df6:1:1648170116.402|281710865595635:7e185:2:1648170111.366","GraphServices"]&

            //server_timestamps=true


            return await _facebookApi.SendRequestAsync<bool, bool>
                ("34554976101548113633120988623",
                "post",
                "PushNotificationsMuteMutation",
                "[\"GraphServices\"]",
                variables.ToString(Formatting.None),
                null,
                null,
                null,
                null,
                true);
        }


        private async Task<IResult<bool>> EnableNotificationsAsync()
        {
            //{
            //  "input": {
            //    "duration": 28800,
            //    "token": "fyLk5A27kAA:",
            //    "client_mutation_id": "-0b97-475e-9cb0-",
            //    "actor_id": ""
            //  }
            //}&
            var variables = new JObject
            {
                {"input", new JObject
                    {
                        {"duration", 28800},
                        {"token", User.LoggedInUser.AccessToken},
                        {"client_mutation_id", _deviceInfo.DeviceGuid.ToString()},
                        {"actor_id", User.LoggedInUser.UId.ToString()},
                    }
                },
            };

            //client_doc_id=34554976101548113633120988623&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //variables={"input":{"duration":28800,"token":"fyLk5A27kAA:","client_mutation_id":"27485e36-0b97-475e-9cb0-","actor_id":""}}&
            //fb_api_req_friendly_name=PushNotificationsMuteMutation&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["nav_attribution_id={\"0\":{\"bookmark_id\":\"295535957817803\",\"session\":\"39df6\",\"subsession\":1,\"timestamp\":\"1648170116.402\",\"tap_point\":\"tap_bookmark\",\"most_recent_tap_point\":\"tap_bookmark\",\"bookmark_type_name\":\"settings\",\"fallback\":false},\"1\":{\"bookmark_id\":\"281710865595635\",\"session\":\"7e185\",\"subsession\":2,\"timestamp\":\"1648170111.366\",\"tap_point\":\"tap_top_jewel_bar\",\"most_recent_tap_point\":\"tap_top_jewel_bar\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}","visitation_id=295535957817803:39df6:1:1648170116.402|281710865595635:7e185:2:1648170111.366","GraphServices"]&

            //server_timestamps=true


            return await _facebookApi.SendRequestAsync<bool, bool>
                ("34554976101548113633120988623",
                "post",
                "PushNotificationsMuteMutation",
                "[\"GraphServices\"]",
                variables.ToString(Formatting.None),
                null,
                null,
                null,
                null,
                true);
        }


    }
}

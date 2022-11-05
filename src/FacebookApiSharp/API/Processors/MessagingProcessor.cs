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
    class MessagingProcessor : IMessagingProcessor
    {
        private readonly HttpHelper _httpHelper;
        private readonly UserSessionData User;
        private readonly IFacebookLogger _logger;
        private readonly AndroidDevice _deviceInfo;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly FacebookApi _facebookApi;
        public MessagingProcessor(AndroidDevice deviceInfo,
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



        public async Task<IResult<FacebookMessage>> SendDirectMessageAsync(string userId, string message)
        {
            string responseContent = null;

            try
            {
                var instaUri = UriCreator.GetGraphQLUri();
                var token = ExtensionsHelper.GetThreadToken();

                //client_doc_id=192413211313323631505026260873&
                //method=post&
                //locale=en_US&
                //pretty=false&
                //format=json&

                //&fb_api_req_friendly_name=MessagingInBlueSendMessageMutation&
                //fb_api_caller_class=graphservice&
                //fb_api_analytics_tags=["nav_attribution_id={\"0\":{\"bookmark_id\":\"391724414624676\",\"session\":\"53b17\",\"subsession\":1,\"timestamp\":\"1646712153.760\",\"tap_point\":\"tap_search_bar\",\"most_recent_tap_point\":\"foreground\",\"bookmark_type_name\":null,\"fallback\":false},\"1\":{\"bookmark_id\":\"4748854339\",\"session\":\"6fca9\",\"subsession\":1,\"timestamp\":\"1646712143.488\",\"tap_point\":\"cold_start\",\"most_recent_tap_point\":\"cold_start\",\"bookmark_type_name\":null,\"fallback\":false,\"badging\":{\"badge_count\":0,\"badge_type\":\"num\"}}}","visitation_id=4748854339:6fca9:1:1646712143.488","GraphServices"]
                //&server_timestamps=true



                //"product_extras": { "product_type": "PROFILE" },
                //"offline_message_id": "6906811681774978189", // thread token
                //"message_content_info": {
                //  "text": "Hi",
                //  "message_type": "TEXT"
                //},
                var input = new JObject()
                {
                    {"product_extras",  new JObject
                        {
                            {"product_type", "PROFILE"}
                        }
                    },
                    {"offline_message_id", token},
                    {"message_content_info",  new JObject
                        {
                            {"text", message},
                            {"message_type", "TEXT"},
                        }
                    },
                    //"messaging_thread_id": "100076841659377",// user id taraf
                    {"messaging_thread_id", userId},
                    //"logging_info": {
                    //  "mib_instance_id": "8603276787900485746",
                    //  "mib_hierarchy_data": "{\"0\":{\"class\":\"MibMainFragment\",\"module\":null,\"tap_point\":null},\"1\":{\"class\":\"MibMainActivity\",\"module\":null,\"tap_point\":null},\"2\":{\"class\":\"ProfileFragment\",\"module\":\"timeline\",\"tap_point\":null},\"3\":{\"class\":\"ImmersiveActivity\",\"module\":\"unknown\",\"tap_point\":null},\"4\":{\"class\":\"SearchResultsFragment\",\"module\":\"graph_search_results_page_blended\",\"tap_point\":null},\"5\":{\"class\":\"GraphSearchFragment\",\"module\":\"unknown\",\"tap_point\":null},\"6\":{\"class\":\"ImmersiveActivity\",\"module\":\"unknown\",\"tap_point\":null},\"7\":{\"class\":\"NewsFeedFragment\",\"module\":\"native_newsfeed\",\"tap_point\":null},\"8\":{\"class\":\"FeedFiltersFragment\",\"module\":\"native_newsfeed\",\"tap_point\":null},\"9\":{\"class\":\"FbChromeFragment\",\"module\":null,\"tap_point\":\"cold_start\"},\"10\":{\"class\":\"FbMainTabActivity\",\"module\":\"unknown\",\"tap_point\":null}}",
                    //  "mib_entry_point": "fb_profile:message_button",
                    //  "messaging_source": "source:profile"
                    //},
                    {"logging_info",  new JObject
                        {
                            //{"mib_instance_id", "8603276787900485746"},
                            {"mib_hierarchy_data", "{\"0\":{\"class\":\"MibMainFragment\",\"module\":null,\"tap_point\":null},\"1\":{\"class\":\"MibMainActivity\",\"module\":null,\"tap_point\":null},\"2\":{\"class\":\"ProfileFragment\",\"module\":\"timeline\",\"tap_point\":null},\"3\":{\"class\":\"ImmersiveActivity\",\"module\":\"unknown\",\"tap_point\":null},\"4\":{\"class\":\"SearchResultsFragment\",\"module\":\"graph_search_results_page_blended\",\"tap_point\":null},\"5\":{\"class\":\"GraphSearchFragment\",\"module\":\"unknown\",\"tap_point\":null},\"6\":{\"class\":\"ImmersiveActivity\",\"module\":\"unknown\",\"tap_point\":null},\"7\":{\"class\":\"NewsFeedFragment\",\"module\":\"native_newsfeed\",\"tap_point\":null},\"8\":{\"class\":\"FeedFiltersFragment\",\"module\":\"native_newsfeed\",\"tap_point\":null},\"9\":{\"class\":\"FbChromeFragment\",\"module\":null,\"tap_point\":\"cold_start\"},\"10\":{\"class\":\"FbMainTabActivity\",\"module\":\"unknown\",\"tap_point\":null}}"},
                            {"mib_entry_point", "fb_profile:message_button"},
                            {"messaging_source", "source:profile"},
                        }
                    },
                    //"client_mutation_id": "fbd5c771-a462-48c9-a4f6-9410d166a821", // guid
                    //"actor_id": "100034137818552" // usere khodet
                    {"client_mutation_id", Guid.NewGuid().ToString()},
                    {"actor_id", User.LoggedInUser.UId.ToString()},
                };
                Debug.WriteLine(input.ToString(Formatting.Indented));
                var variables = new JObject()
                {
                    {"input", input},
                    {"profile_pic_size", 99},
                    {"large_preview_image_height", 750},
                    {"full_screen_image_width", 2048},
                    {"full_screen_image_height", 2048},
                    {"large_preview_image_width", 750},
                    {"nt_context", new JObject
                        {
                            {"using_white_navbar", true},
                            {"styles_id", FacebookApiConstants.FACEBOOK_STYLES_ID},
                            {"pixel_ratio", 3},
                            {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
                        }
                    },
                };
                var data = new Dictionary<string, string>
                {
                    {"client_doc_id", "192413211313323631505026260873"},
                    {"method", "post"},
                    {"locale", _facebookApi.AppLocale},
                    {"pretty", "false"},
                    {"format", "json"},
                    {"variables", variables.ToString(Formatting.None)},
                    {"fb_api_req_friendly_name", "MessagingInBlueSendMessageMutation"},
                    {"fb_api_caller_class", "graphservice"},
                    {"fb_api_analytics_tags", "[\"GraphServices\"]"},
                    {"server_timestamps", "true"},
                    {"access_token", FacebookApiConstants.FACEBOOK_ACCESS_TOKEN},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "MessagingInBlueSendMessageMutation", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_RMD, "cached=0;state=NO_MATCH", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_PRIORITY, "u=3, i");

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookMessage>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookMessage));

                responseContent = json;

                var obj = JsonConvert.DeserializeObject<FacebookMessageContainerResponse>(json);

                return Result.Success(ObjectsConverter.Instance.GetDirectMessageConverter(obj).Convert());
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookMessage), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookMessage>(exception, responseContent);
            }
        }

        public async Task<IResult<FacebookInboxFriends>> GetDirectInboxFriendsAsync()
        {
            //client_doc_id=157070121311310993102490765089&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&variables={"page_size":7,"tile_size":220}&
            //fb_api_req_friendly_name=FriendsFacepilesQuery&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["GraphServices"]&
            //server_timestamps=true
            return await _facebookApi.SendRequestAsync
                ("157070121311310993102490765089",
                "post", 
                "FriendsFacepilesQuery",
                "[\"GraphServices\"]",
                "{\"page_size\":7,\"tile_size\":220}",
                new FacebookInboxFriendsConverter());
        }

        public async Task<IResult<FacebookInboxTopFriends>> GetDirectInboxTopFriendsAsync()
        {
            //client_doc_id=750929114295349742161526918&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //variables={"tile_size":240}&
            //fb_api_req_friendly_name=MIBTopFriendsQuery&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["GraphServices"]&
            //server_timestamps=true
            return await _facebookApi.SendRequestAsync
                ("750929114295349742161526918",
                "post",
                "MIBTopFriendsQuery",
                "[\"GraphServices\"]",
                "{\"tile_size\":240}",
                new FacebookInboxTopFriendsConverter());
        }

        public async Task<IResult<FacebookInboxFriends>> GetDirectInboxRecentltyActiveFriendsAsync()
        {
            //client_doc_id=115333238017266656933682149605&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //variables={"count":20,"tile_size":165}&
            //fb_api_req_friendly_name=ActiveNowQueryWithRecentlyActive&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=["GraphServices"]&
            //server_timestamps=true
            return await _facebookApi.SendRequestAsync
                ("115333238017266656933682149605",
                "post",
                "ActiveNowQueryWithRecentlyActive",
                "[\"GraphServices\"]",
                "{\"count\":20,\"tile_size\":165}",
                new FacebookInboxFriendsConverter());
        }
    }
}

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
    internal class UserProcessor : IUserProcessor
    {
        private readonly HttpHelper _httpHelper;
        private readonly UserSessionData User;
        private readonly IFacebookLogger _logger;
        private readonly AndroidDevice _deviceInfo;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly FacebookApi _facebookApi;
        public UserProcessor(AndroidDevice deviceInfo,
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

        public async Task<IResult<List<FacebookActorResponse>>> SearchAsync(string query,
            PaginationParameters pagination)
        {
            var list = new List<FacebookActorResponse>();
            try
            {
                if (pagination == null)
                    pagination = PaginationParameters.MaxPagesToLoad(1);
                bool NotExists(string id) =>
                    list.Any(x => x.Id == id);

                void Append(List<FacebookActorResponse> appendList)
                {
                    foreach (var item in appendList)
                    {
                        if (!NotExists(item.Id))
                            list.Add(item);
                    }
                }
                var result = await SearchPagination(query, pagination);
                if (!result.Succeeded)
                    return Result.Fail(result.Info, list);
                pagination.PagesLoaded++;
                Append(result.Value);

                while (pagination.HasPreviousPage
                       && !string.IsNullOrEmpty(pagination.EndCursor)
                       && pagination.PagesLoaded <= pagination.MaximumPagesToLoad)
                {

                    var nextSearch = await SearchPagination(query, pagination);
                    if (!nextSearch.Succeeded)
                        return Result.Fail(nextSearch.Info, list);
                    Append(nextSearch.Value);

                    pagination.PagesLoaded++;
                }
                Debug.WriteLine("COUNT: " + list.Count);
                foreach (var actor in list)
                {
                    Debug.WriteLine($"{actor.StrongId}\t\t\t{actor.Id}\t\t\t{actor.Name}\t\t\t{actor.Typename}");
                }
                return Result.Success(list);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, list, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail(exception, list);
            }
        }

        public async Task<IResult<FacebookFriends>> GetFriendsAsync(PaginationParameters pagination)
        {
            var friendsList = new FacebookFriends();
            try
            {
                if (pagination == null)
                    pagination = PaginationParameters.MaxPagesToLoad(1);
                //bool NotExists(string id) =>
                //friendsList?.Users.Any(x => x.Id == id) ?? false;

                void Append(List<FacebookActorResponse> appendList)
                {
                    foreach (var item in appendList)
                    {
                        //if (!NotExists(item.Id))
                            friendsList.Users.Add(item);
                    }
                }

                var result = await GetFriends(pagination);
                if (!result.Succeeded || result.Value == null)
                    return Result.Fail(result.Info, friendsList);

                pagination.PagesLoaded++;
                friendsList = result.Value;

                pagination.EndCursor = friendsList.PageInfo?.EndCursor;
                pagination.HasPreviousPage = friendsList.PageInfo?.HasNextPage ?? false;
                while (pagination.HasPreviousPage
                   && !string.IsNullOrEmpty(pagination.EndCursor)
                   && pagination.PagesLoaded <= pagination.MaximumPagesToLoad)
                {

                    var nextPage = await GetFriends(pagination);
                    if (!nextPage.Succeeded)
                        return Result.Fail(nextPage.Info, friendsList);

                    Append(nextPage.Value.Users);

                    pagination.EndCursor = nextPage.Value?.PageInfo?.EndCursor;
                    pagination.HasPreviousPage = nextPage.Value?.PageInfo?.HasNextPage ?? false;
                    pagination.PagesLoaded++;
                }

                return Result.Success(friendsList);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, friendsList, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail(exception, friendsList);
            }
        }


        private async Task<IResult<FacebookFriends>> GetFriends(PaginationParameters pagination)
        {
            //{
            //  "profile_picture_normal_size": 253,
            //  "profile_picture_small_size": 165,
            //  "pivot_link_options": "friends",
            //  "nt_context": {
            //    "using_white_navbar": true,
            //    "pixel_ratio": 3,
            //    "styles_id": "5e0de59125eab28ac26cc293eeffba01",
            //    "bloks_version": "7d7c2eae7a06dc8d95a66c8ab896354b9d0c372610859c5ca5c8c2715f9e5bc7"
            //  },
            //  "scale": "3",
            //  "nt_render_id": "0",
            //  "supported_features": { "client_ccu_status": "DISABLED" },
            //  "dynamic_friending_tab_paginating_first": 30
            //}
            //{
            //  "profile_picture_normal_size": 253,
            //  "profile_picture_small_size": 165,
            //  "pivot_link_options": "friends",
            //  "nt_context": {
            //    "using_white_navbar": true,
            //    "pixel_ratio": 3,
            //    "styles_id": "24127905fff357472331453e654da67a",
            //    "bloks_version": "55eee11487f8abefe1258fb33b90535678f9445b1c17ee77645706a2592e95c5"
            //  },
            //  "scale": "3",
            //  "nt_render_id": "0",
            //  "supported_features": { "client_ccu_status": "ENABLED" },
            //  "dynamic_friending_tab_paginating_first": 30
            //}


            var variables = new JObject
            {
                {"profile_picture_normal_size", 253},
                {"profile_picture_small_size", 165},
                {"pivot_link_options", "friends"},
                {"nt_context", new JObject
                    {
                        {"using_white_navbar", true},
                        {"pixel_ratio", 3},
                        {"styles_id", FacebookApiConstants.FACEBOOK_STYLES_ID},
                        {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
                    }
                },
                {"scale", "3"},
                {"nt_render_id", "0"},
                {"supported_features", new JObject
                    {
                        {"client_ccu_status", "ENABLED"},
                    }
                },
                {"dynamic_friending_tab_paginating_first", 30},
            };

            //client_doc_id=274349594911706820795057134745&
            //method=post&
            //locale=en_US&
            //pretty=false&
            //format=json&
            //purpose=fetch&
            //variables={"profile_picture_normal_size":253,"profile_picture_small_size":165,"pivot_link_options":"friends","nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"5e0de59125eab28ac26cc293eeffba01","bloks_version":"7d7c2eae7a06dc8d95a66c8ab896354b9d0c372610859c5ca5c8c2715f9e5bc7"},"scale":"3","nt_render_id":"0","supported_features":{"client_ccu_status":"DISABLED"},"dynamic_friending_tab_paginating_first":30}&

            //fb_api_req_friendly_name=FriendingJewelContentQuery&
            //fb_api_caller_class=graphservice&
            //fb_api_analytics_tags=[\"At_Connection\",\"GraphServices\"]&
            //server_timestamps=true



            //x-fb-rmd: cached=0;state=NO_MATCH
            //Priority: u=0
            //Content-Type: application/x-www-form-urlencoded
            //x-fb-qpl-ec: 
            //X-FB-Privacy-Context: c0000000ebe595b4
            //X-FB-Friendly-Name: FriendingJewelContentQuery
            //x-fb-connection-token: 31ef6fc8ce6e598787be0369ed77e512

            var parameters = new Dictionary<string, string>
            {
                {FacebookApiConstants.HEADER_PRIORITY, "u=0"},
                {"x-fb-qpl-ec", ""}
            };
            if (!string.IsNullOrEmpty(pagination.EndCursor))
            {
                variables.Add("dynamic_friending_tab_paginating_after_cursor", pagination.EndCursor);
                parameters.Add("fb_api_client_context", "{\"load_next_page_counter\":1,\"client_connection_size\":31}");
            }
            //X-FB-QPL-ACTIVE-FLOWS-JSON: {"schema_version":"v1","inprogress_qpls":[{"marker_id":25952257,"annotations":{"current_endpoint":"FriendingJewelFragment:friend_requests"}}]}
            //X-FB-QPL-ACTIVE-FLOWS-JSON: {"schema_version":"v1","inprogress_qpls":[{"marker_id":25952257,"annotations":{"current_endpoint":"FriendingJewelFragment:friend_requests"}}]}


            //X-FB-Friendly-Name: FriendingJewelContentQuery
            //&fb_api_req_friendly_name=FriendingJewelContentQuery&


            //X-FB-Friendly-Name: FriendingJewelContentQuery_At_Connection_Pagination_Viewer_dynamic_friending_tab_paginating
            //&fb_api_req_friendly_name=FriendingJewelContentQuery_At_Connection_Pagination_Viewer_dynamic_friending_tab_paginating&fb_api_caller_class=ConnectionManager&
         


            return await _facebookApi.SendRequestAsync
                (!string.IsNullOrEmpty(pagination.EndCursor) ? "391589310113868657910806649396" : "27434959492399229206641439365",
                "post",
                string.IsNullOrEmpty(pagination.EndCursor) ? "FriendingJewelContentQuery" : "FriendingJewelContentQuery_At_Connection_Pagination_Viewer_dynamic_friending_tab_paginating",
                "[\"At_Connection\",\"GraphServices\"]",
                variables.ToString(Formatting.None),
                new FacebookFriendsConverter(),
                new Uri("https://graph.facebook.com/graphql"),
                new Dictionary<string, string> 
                { 
                    { "purpose", "fetch" } ,
                    { "X-FB-QPL-ACTIVE-FLOWS-JSON", "{\"schema_version\":\"v1\",\"inprogress_qpls\":[{\"marker_id\":25952257,\"annotations\":{\"current_endpoint\":\"FriendingJewelFragment:friend_requests\"}}]}" }
                },
                parameters,
                true,
                true, string.IsNullOrEmpty(pagination.EndCursor) ? "graphservice" : "ConnectionManager");
        }




        private async Task<IResult<List<FacebookActorResponse>>> SearchPagination(string query, PaginationParameters pagination)
        {
            var list = new List<FacebookActorResponse>();
            try
            {
                var result = await Search(query, pagination);
                if (!result.Succeeded || string.IsNullOrEmpty(result.Value))
                    return Result.Fail(result.Info, list);

                var splitter = result.Value.Split('\n');
                FacebookPaginationResultResponse paginationResultResponse = null;
                FacebookDataPageResultList facebookDataPageResults = new FacebookDataPageResultList();
                FacebookPaginationResultResponse<FacebookSearchData> facebookSearchData = null;
                foreach (var json in splitter)
                {
                    // {"data
                    // [{\"label
                    // {\"label\":"SearchResultsCombinedPageInfoStreaming"
                    if (json.StartsWith("{\"data"))
                    {
                        facebookSearchData = JsonConvert.DeserializeObject<FacebookPaginationResultResponse<FacebookSearchData>>(json);
                    }
                    else if (json.StartsWith("[{\"label"))
                    {
                        var facebookDataPageResultsX = JsonConvert.DeserializeObject<FacebookDataPageResultList>(json);
                        if (facebookDataPageResultsX?.Count > 0)
                            facebookDataPageResults.AddRange(facebookDataPageResultsX);


                    }
                    else if (json.StartsWith("{\"label") &&
                  json.Contains("\"serp_at_stream\""))
                    {
                        var facebookDataPageResultsX = JsonConvert.DeserializeObject<FacebookDataPageResult>(json);
                        if (facebookDataPageResultsX != null)
                            facebookDataPageResults.Add(facebookDataPageResultsX);
                    }
                    else if (json.StartsWith("{\"label") &&
                        json.Contains("\"SearchResultsCombinedPageInfoStreaming\""))
                    {
                        paginationResultResponse = JsonConvert.DeserializeObject<FacebookPaginationResultResponse>(json);
                    }
                }
                if (facebookSearchData?.Data?.SearchQuery?.CombinedResults?.Edges?.Count > 0)
                {
                    foreach (var item in facebookSearchData.Data.SearchQuery.CombinedResults.Edges)
                    {
                        if (item.NativeTemplateView?.NativeTemplateBundles?.Count > 0)
                        {
                            var ntTemplate = item.NativeTemplateView?
                                .NativeTemplateBundles?
                                .FirstOrDefault()?
                                .NtBundleAttributes?
                                .FirstOrDefault();

                            if (ntTemplate?.Typename == "NTRecentSearchEntityAttribute" &&
                               (ntTemplate?.RecentSearchEntityValue?.Typename?.Contains("User") ?? false))
                            {
                                list.Add(ntTemplate.RecentSearchEntityValue);
                            }
                        }
                    }

                }
                if (facebookDataPageResults?.Count > 0)
                {
                    foreach (var item in facebookDataPageResults)
                    {
                        if (item.Data?.NativeTemplateView?.NativeTemplateBundles?.Length > 0)
                            foreach (var y in item.Data.NativeTemplateView.NativeTemplateBundles)
                            {
                                foreach (var x in y.NtBundleAttributes)
                                {
                                    if (x.StoryValue?.Actors?.Length > 0)
                                    {
                                        var actor = x.StoryValue.Actors.FirstOrDefault();
                                        if (actor.Typename.ToLower().Contains("user"))
                                        {
                                            list.Add(actor);
                                            //Debug.WriteLine($"{actor.StrongId}\t\t\t{actor.Id}\t\t\t{actor.Name}");
                                        }
                                    }
                                }
                            }
                    }

                }

                pagination.EndCursor = paginationResultResponse?.Data?.PageInfo.EndCursor;
                pagination.HasPreviousPage = paginationResultResponse?.Data?.PageInfo.HasNextPage ?? false;

                return Result.Success(list);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, list, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail(exception, list);
            }
        }


        private async Task<IResult<string>> Search(string query, PaginationParameters pagination)
        {
            string responseContent = null;

            try
            {
                var instaUri = UriCreator.GetGraphQLUri();
                //client_doc_id=35067217398338956168214945464&
                //method=post&
                //locale=en_US&
                //pretty=false&
                //format=json&
                //purpose=fetch&
                //variables={"query":{"session_id":"0ce14e88-9ce1-4ac4-842e-252d12b403f1","query_text":"Instagram","friendly_name":null,"fetch_mode":"blended","ranking_model":"kw_s19n_default","request_id":"50188430","fetch_count":0}}&

                //fb_api_req_friendly_name=Fb4aSearchTypeaheadQuery&
                //fb_api_caller_class=graphservice&
                //fb_api_analytics_tags=["GraphServices"]&
                //server_timestamps=true
                //{
                //  "query": {
                //    "session_id": "0ce14e88-9ce1-4ac4-842e-252d12b403f1",
                //    "query_text": "Instagram",
                //    "friendly_name": null,
                //    "fetch_mode": "blended",
                //    "ranking_model": "kw_s19n_default",
                //    "request_id": "50188430",
                //    "fetch_count": 0
                //  }
                //}




                if (pagination == null)
                {
                    pagination = PaginationParameters.MaxPagesToLoad(1);
                }

                //"end_cursor": "AbpbOk4-DLBiWniKIDNNQMRIOl8PzLzTNOBxGx0bLqFTSEGga9RhD4mfhWO5wBoFXVUAoOjddQ0Xx5_7raaRdWG-tFN4FnfbGlqWJ7zKu0oDt8KaCQvYUUp0dGm2nxQtX7yWGjRSEGOqkC1OMCvLd0t-UzBBs8OeheyxlYn4L1Gb3y1tK3IUm0LmMu9gB8ZTaJXqtgLKreURWHAgzynQ9HhiN5N3dLPq-ATJLfYnU0jUQ1MuHyt1_3_jBF3sFzJ5xJzfaTgssim4l2zYlxz9gU8k_9RK4eCUhS9JYT3gLK6yvduWqsU25Lv0s1JfhlLuNwbl8t5UMjKTdVrBZbh9RXzWlsWJRjOgIPrc4xYFoTN_b0oCwdaODnwpTQdrPNoMwPSqG8PSg6u9xe2Y3BPVsg2dZHgV_mJNv21blqmZ8k1m0uMz0uzZFxRWDaZJODqKHr8lBIpG_A_NEHfF2gMH6IA6KdrDuq4EilaomKMi4ayA5Z6UapEgkO6ExxNuh4rFAINXIPhuKtsoxCQS-FHRB5ozDfvcXwPAKCTNyFH_7hKoJT3F-j504u4sHBE64_aPorYAA7kuw1TfXFhL6mkNirqBqQYkMK0dlQtzzcWrt468uLckihpNA3HfSwzlXWxXR2F3WirskUXaAXk9A5ZrLJqQu4VQe5XzaLbBaB1QuW80b6U0JD_lkKeg7EwrT65RlEiwFsh7Z5GYTB8eBCK2U2WdJvPKFz14IjOMoRDJD1Wlbrlp9WfUHLtVPk8X38wmvNIf6nnAtXUvshXt3SEm9C5zq7s7QSkY0SYUO8lf2pZTYKebO7f7gvCc3RU0oba4z3XgBPdIozr_NdS-3D3j-4o9--BtdbON4EsQhDUo4Tfa1_zdnSqxaM_jcC95fFhDCmTviJltcHV6JuPfErFUzRinZg3eYesZeCSAv5ovBwtA4fq9f0unTQqpkpAJ5vCFS6aDxVr9rUtx6p30zW3BQSlS_dL7kkj_2pwm7Zz1RnGamcbUFiLhEz3l6YqUwAI4ikXCs8s2gNNmuRcfCBi4TXLuCx9mCfVcrOlODikx8YayXBXtGlTwRC2wBnnnvJtgqyDjFu20eIuhkZdkDX5HCnVCWaz82AbWO5g30SH1VzxO74W_Rost89v9nQeQED9xKuhmIltIcb1Jn4T3YZd8sbmPiioMlTCTwK9SOAnFNMF0IMTZAOp1-U3A1GogH3o9h2y1-nMX1bOuGDLQQqQGP5vLJjEeC_f3Z-faPK8SQ5HjiZuVB-n-_P4AvDhl6MtccGKvISsvpOCdmWE4NWHN3qdsK_TMMo4YfQH5u91il9K3OB_qXM0k3PdVCYFnq4D22Md5aH3zLAmCY-YDz7CbTU3X6eYbWM0NXN_mzYSQdV84oVufwONH4xmdFYtQisJn6zwxBOMERawRiv9B6qPcWsFM17Sn8q4CXyktIsYHkv00JqvcXXW2KNk6cSYwDmklqy3th4FvA4Ie0xlNjY9FwHD7n441Z4YByrsC15itWekKuLJLICEfmMOGa1o3EdA_lO0CQ-tmxbbcoEj3d7PBKFAtmcepAxsg2dq7Nx5EC6x4GFZYBy2_kVWA3iPoNP_SnHjKrJfHnM9fklMf0FdUUJ9iVg6cOEw9GmLfgZB-vYdSpN9BZXiWD2Ocjye03lurYxVLWyfjOmVtGHw6kl-WdJYrzPn7KH6mGiZNigs3lQYJA5RJkjRL0I50XwBkrQVFxdG9sfbSFFmDWbpQn31Kw65gXrykRe5W5Lwb4enTmn3PGHpYpLJgUM1uBnaCwI3aEJ3Iowuo44r74NIHIU7e_NzCaGsqzUnVHvIamAflbDGthYJdGJiSd92xqUnn5hBKsHzS2c9PzphVIrxClUJhU0IuukqTXZuffdQeousgV7Cbn5d6YuX4GIeg8JuWdiJ_hc9o3LS-fAma9UdtMQefuUgjcwIpubIZZAfO0DTTHxCrgZ8f2YmU_M_TXVzO-G4EFG4hqezO7yQ_nfehw2m9K6Mz5aMTAIngxdDSFw_688OdRaijpaU581YmKuclFT9c4-YjISkK16PSBSMRrKC5ioS6gkcF8ZEQDdVbifecMz_4muJiCPSCegOXiO9NCpmsLgbf5RKFMAuEhQEYmUf-3Gea7WbzZUPXdOqXM0EOcE4CFTMIn9fltLSuaGY93Ea-pzXMViuPeGUz4XJohKFw0AX35f-tZr-sxrW7eG0Ij7PYnEvgFY3JFGtSIbGNdkXBtVtw4fkXM1lztXo_-KlnkWagKJx4BR4GyEh1XoPnNX36lcs6IUZCSo8VhQHWCy9c9k0lSSwSF7Fbe1yyetgoA61IFesA70St1EJKi1vKYd8GBlk6FZSUPvNbhUGmhZ4IKoCXCkr7IX8cO5TzGjydp1oJyYY-qXDdCVELL4rgnURjJ9G3kSgwQR8Op3IP_tuq5VR8vPzme-PuozVqwhBNBasvaYVVxd0R71HlkvbYC5vUSz5nOYLYi6ISeWhrWLFZmChi9vUEqJ4laClDQTuz0Otgxr9594oOGGPQq5EKNxhqN4LIYm-Hz3pXIMghEewKbemVKYfAF4ZT5vSMtW5G72zk3eSWQgOPUM61eOy50bID1JVGeaaAr77fnRpk7j-dYhIMWlDcciBz7KCVmIc2GkgT2mc8AcgI_IHQS63eBUjeaJE4tyCTUOz8pn5r-LNg5FVQjYycyral1DAMpegH5c55_vqit4kf6QabzI_g_zsvLAzYN5SicHDE-S7sIKO8aS5z-pF59rP0flgcTPR3Td4nGpZl1nVRPZ9P6DiNfV4456W3JaAk0nrEF0YbHNUY66F_JgCLEf5N2J9RbCUe-0tjbNeLU-7iTQHXCVctxUoAWaa-gTJTDHxInf4kpsLId-TdoaOeXs6_hHOt3qvOqmaokj3d-SOZX238u-owFMHioQbpdpcyGXEg8pjv0NzI1Ivjh5YlbB7QCDXZuJnSkO1HYsLlCWVTzazWdvkN-6nnEcihh9Cn9TA9IKbicURj9KCoyiAMhiQAhFNbpEnpHKN1x71nZUkNp1brcJFrDrhsSq7qDeQSua_eZjJCmuGd310OV3Dhdb4A9ZXd7vMoLovsj3UYvKm-t8r6o3XH8SYJDR4RhsTvgcb4VH29CsFlUVE_WSWALmcyUiGDJOjWPuoQAKmAoEDcsqErARYcmVEqvn15t04EMGFM7S6Gsu1Xdt6zrrAnfKf0V9ZixPU9YgJH1O0Lw6TXBhWAn-2sWjnxsVifYfk7An5n1gH2oIn2OkajWg3RCa6wpGsiyzZyKTrnkE0sRVWT4smbWR7hJCX9t5nbWCPy_koliOkCTdhi-5_LGUJZ-TV6hJd6bW96VP72ShSyqEPtP9eI1HA_4sPIfHu1H4lgH-ShEUCAKLH_0GDs-7KlFA_PkzINC21ASGhEZg2SUuHNnTKK9e_4OhrRyr27_KQymOFs125xNYRXKYlErYW6c-kCAirVbiKxLCyC-uvH2r1qDEo3xiHe6aIFJkvPnayNw4Q9d5bVSBk1poJfJ7EeZLt0NBaNm5x6-sa3hZMCe-wjtRjtxoOor8Qc1jHMM-TltGrgR_7xTzNcw6qdKkLxp1Su5PHxTFO0KS-EeVEPDZW5ThwS8vKIHX06BVMzYXtUf2ryYmEF6KLZeCaPtAXsO66tGHUTeQZNSP16tS5rbrEJHdjDGoq4NjMLGQigMsYDyD0bzai3lI8NtC6FGCHtjLwBhzdo5LZxW52Si-uPVxap4U2dZoqFJ9aZMRzeZL5BGwGeUO4_FbCt1H_IzGwzeagyBXrPT-sJ3NqJSUri5XHWrLl4dxzYeqvru14xNRvwPGLDknoy37nQ904N43B5CBcTOm5y6TdoAnexmScOFff3TbEjRoafdWMQEXucGK_ZFyjv4VwAwO7UgQPlQkYsfGx0qFSDy48mbbjGzSctN8NnE0vZH7S8LXrCyLU-2ME2M0igeA20BhigyKtWBIEkk95jm6PUVdi81pINqQITwWG5XIFhmErt8KKj9w4ArShrVLwqfj0C0ModVIOJeCmTmCHwa3t0YLaAwhMX8NfvOCIjaXsO8-Pk4pFs-YBVL-Ge1n6kpP7fNPWITwgeVPVz_QJZig-JFpRSfcLAWl2XSv1Twjy43rHWspumMXCMAbGDQZX5NMtc3kmwaqKNU9qniv6fJvAilKvPdY5qfxkysVHUWrfjU0efurFF8w",



                var variables = new JObject()
                {     //{"query", new JObject
                    //    {
                    //        {"session_id", pagination.SessionId},
                    //        {"query_text", query},
                    //        {"friendly_name", null},
                    //        {"fetch_mode", "blended"},
                    //        {"ranking_model", "kw_s19n_default"},
                    //        {"request_id", "50188430"},
                    //        {"fetch_count", "0"},
                    //    } 
                    //}
                    {"supported_experiences", new JArray("FAST_FILTERS", "FILTERS", "FILTERS_AS_SEE_MORE", "INSTANT_FILTERS", "MARKETPLACE_ON_GLOBAL", "MIXED_MEDIA", "NATIVE_TEMPLATES", "NT_ENABLED_FOR_TAB", "NT_SPLIT_VIEWS", "PHOTO_STREAM_VIEWER", "SEARCH_INTERCEPT", "SEARCH_SNIPPETS_ICONS_ENABLED", "USAGE_COLOR_SERP", "commerce_groups_search", "keyword_only") },
                    {"default_image_scale", 3},

                    {"nt_context", new JObject
                        {
                            {"styles_id", FacebookApiConstants.FACEBOOK_STYLES_ID},
                            {"pixel_ratio", 3},
                            {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
                            {"using_white_navbar", true},
                        }
                    },
                     { "bsid", pagination.SessionId},
                     {"entered_query_text", ""},// query text
                     {"disable_story_menu_actions", false},
                     {"query_source", "unknown"},
                     {"ui_theme_name", "APOLLO_FULL_BLEED"},
                     {"image_high_width", 1080},
                     {"image_large_aspect_width", 1080},
                     {"image_low_width", 360},
                     {"filters_enabled", false},
                     {"callsite", "android:user_search"},
                     {"image_large_aspect_height", 565},
                     {"image_medium_width", 540},
                     {"profile_image_size", 259},
                     {"enable_at_stream", true},
                     {"bqf", $"keywords_users({query})"},
                     {"scale", "3"},
                     //{"query_source", "graph_search_v2_typeahead_keyword_suggestion"},
                     {"product_item_image_size", 418},
                     {"request_index", 0},
                };
                if (!string.IsNullOrEmpty(pagination.EndCursor))
                {
                    variables.Add("end_cursor", pagination.EndCursor);
                }
                else
                {
                    variables.Add("first_unit_only", true);
                }
                //client_doc_id=39590791075919423816857174440&
                //method=post&
                //locale=en_US&
                //pretty=false&
                //format=json&
                //variables={"end_cursor":"AbpbOk4-DLBiWniKIDNNQMRIOl8PzLzTNOBxGx0bLqFTSEGga9RhD4mfhWO5wBoFXVUAoOjddQ0Xx5_7raaRdWG-tFN4FnfbGlqWJ7zKu0oDt8KaCQvYUUp0dGm2nxQtX7yWGjRSEGOqkC1OMCvLd0t-UzBBs8OeheyxlYn4L1Gb3y1tK3IUm0LmMu9gB8ZTaJXqtgLKreURWHAgzynQ9HhiN5N3dLPq-ATJLfYnU0jUQ1MuHyt1_3_jBF3sFzJ5xJzfaTgssim4l2zYlxz9gU8k_9RK4eCUhS9JYT3gLK6yvduWqsU25Lv0s1JfhlLuNwbl8t5UMjKTdVrBZbh9RXzWlsWJRjOgIPrc4xYFoTN_b0oCwdaODnwpTQdrPNoMwPSqG8PSg6u9xe2Y3BPVsg2dZHgV_mJNv21blqmZ8k1m0uMz0uzZFxRWDaZJODqKHr8lBIpG_A_NEHfF2gMH6IA6KdrDuq4EilaomKMi4ayA5Z6UapEgkO6ExxNuh4rFAINXIPhuKtsoxCQS-FHRB5ozDfvcXwPAKCTNyFH_7hKoJT3F-j504u4sHBE64_aPorYAA7kuw1TfXFhL6mkNirqBqQYkMK0dlQtzzcWrt468uLckihpNA3HfSwzlXWxXR2F3WirskUXaAXk9A5ZrLJqQu4VQe5XzaLbBaB1QuW80b6U0JD_lkKeg7EwrT65RlEiwFsh7Z5GYTB8eBCK2U2WdJvPKFz14IjOMoRDJD1Wlbrlp9WfUHLtVPk8X38wmvNIf6nnAtXUvshXt3SEm9C5zq7s7QSkY0SYUO8lf2pZTYKebO7f7gvCc3RU0oba4z3XgBPdIozr_NdS-3D3j-4o9--BtdbON4EsQhDUo4Tfa1_zdnSqxaM_jcC95fFhDCmTviJltcHV6JuPfErFUzRinZg3eYesZeCSAv5ovBwtA4fq9f0unTQqpkpAJ5vCFS6aDxVr9rUtx6p30zW3BQSlS_dL7kkj_2pwm7Zz1RnGamcbUFiLhEz3l6YqUwAI4ikXCs8s2gNNmuRcfCBi4TXLuCx9mCfVcrOlODikx8YayXBXtGlTwRC2wBnnnvJtgqyDjFu20eIuhkZdkDX5HCnVCWaz82AbWO5g30SH1VzxO74W_Rost89v9nQeQED9xKuhmIltIcb1Jn4T3YZd8sbmPiioMlTCTwK9SOAnFNMF0IMTZAOp1-U3A1GogH3o9h2y1-nMX1bOuGDLQQqQGP5vLJjEeC_f3Z-faPK8SQ5HjiZuVB-n-_P4AvDhl6MtccGKvISsvpOCdmWE4NWHN3qdsK_TMMo4YfQH5u91il9K3OB_qXM0k3PdVCYFnq4D22Md5aH3zLAmCY-YDz7CbTU3X6eYbWM0NXN_mzYSQdV84oVufwONH4xmdFYtQisJn6zwxBOMERawRiv9B6qPcWsFM17Sn8q4CXyktIsYHkv00JqvcXXW2KNk6cSYwDmklqy3th4FvA4Ie0xlNjY9FwHD7n441Z4YByrsC15itWekKuLJLICEfmMOGa1o3EdA_lO0CQ-tmxbbcoEj3d7PBKFAtmcepAxsg2dq7Nx5EC6x4GFZYBy2_kVWA3iPoNP_SnHjKrJfHnM9fklMf0FdUUJ9iVg6cOEw9GmLfgZB-vYdSpN9BZXiWD2Ocjye03lurYxVLWyfjOmVtGHw6kl-WdJYrzPn7KH6mGiZNigs3lQYJA5RJkjRL0I50XwBkrQVFxdG9sfbSFFmDWbpQn31Kw65gXrykRe5W5Lwb4enTmn3PGHpYpLJgUM1uBnaCwI3aEJ3Iowuo44r74NIHIU7e_NzCaGsqzUnVHvIamAflbDGthYJdGJiSd92xqUnn5hBKsHzS2c9PzphVIrxClUJhU0IuukqTXZuffdQeousgV7Cbn5d6YuX4GIeg8JuWdiJ_hc9o3LS-fAma9UdtMQefuUgjcwIpubIZZAfO0DTTHxCrgZ8f2YmU_M_TXVzO-G4EFG4hqezO7yQ_nfehw2m9K6Mz5aMTAIngxdDSFw_688OdRaijpaU581YmKuclFT9c4-YjISkK16PSBSMRrKC5ioS6gkcF8ZEQDdVbifecMz_4muJiCPSCegOXiO9NCpmsLgbf5RKFMAuEhQEYmUf-3Gea7WbzZUPXdOqXM0EOcE4CFTMIn9fltLSuaGY93Ea-pzXMViuPeGUz4XJohKFw0AX35f-tZr-sxrW7eG0Ij7PYnEvgFY3JFGtSIbGNdkXBtVtw4fkXM1lztXo_-KlnkWagKJx4BR4GyEh1XoPnNX36lcs6IUZCSo8VhQHWCy9c9k0lSSwSF7Fbe1yyetgoA61IFesA70St1EJKi1vKYd8GBlk6FZSUPvNbhUGmhZ4IKoCXCkr7IX8cO5TzGjydp1oJyYY-qXDdCVELL4rgnURjJ9G3kSgwQR8Op3IP_tuq5VR8vPzme-PuozVqwhBNBasvaYVVxd0R71HlkvbYC5vUSz5nOYLYi6ISeWhrWLFZmChi9vUEqJ4laClDQTuz0Otgxr9594oOGGPQq5EKNxhqN4LIYm-Hz3pXIMghEewKbemVKYfAF4ZT5vSMtW5G72zk3eSWQgOPUM61eOy50bID1JVGeaaAr77fnRpk7j-dYhIMWlDcciBz7KCVmIc2GkgT2mc8AcgI_IHQS63eBUjeaJE4tyCTUOz8pn5r-LNg5FVQjYycyral1DAMpegH5c55_vqit4kf6QabzI_g_zsvLAzYN5SicHDE-S7sIKO8aS5z-pF59rP0flgcTPR3Td4nGpZl1nVRPZ9P6DiNfV4456W3JaAk0nrEF0YbHNUY66F_JgCLEf5N2J9RbCUe-0tjbNeLU-7iTQHXCVctxUoAWaa-gTJTDHxInf4kpsLId-TdoaOeXs6_hHOt3qvOqmaokj3d-SOZX238u-owFMHioQbpdpcyGXEg8pjv0NzI1Ivjh5YlbB7QCDXZuJnSkO1HYsLlCWVTzazWdvkN-6nnEcihh9Cn9TA9IKbicURj9KCoyiAMhiQAhFNbpEnpHKN1x71nZUkNp1brcJFrDrhsSq7qDeQSua_eZjJCmuGd310OV3Dhdb4A9ZXd7vMoLovsj3UYvKm-t8r6o3XH8SYJDR4RhsTvgcb4VH29CsFlUVE_WSWALmcyUiGDJOjWPuoQAKmAoEDcsqErARYcmVEqvn15t04EMGFM7S6Gsu1Xdt6zrrAnfKf0V9ZixPU9YgJH1O0Lw6TXBhWAn-2sWjnxsVifYfk7An5n1gH2oIn2OkajWg3RCa6wpGsiyzZyKTrnkE0sRVWT4smbWR7hJCX9t5nbWCPy_koliOkCTdhi-5_LGUJZ-TV6hJd6bW96VP72ShSyqEPtP9eI1HA_4sPIfHu1H4lgH-ShEUCAKLH_0GDs-7KlFA_PkzINC21ASGhEZg2SUuHNnTKK9e_4OhrRyr27_KQymOFs125xNYRXKYlErYW6c-kCAirVbiKxLCyC-uvH2r1qDEo3xiHe6aIFJkvPnayNw4Q9d5bVSBk1poJfJ7EeZLt0NBaNm5x6-sa3hZMCe-wjtRjtxoOor8Qc1jHMM-TltGrgR_7xTzNcw6qdKkLxp1Su5PHxTFO0KS-EeVEPDZW5ThwS8vKIHX06BVMzYXtUf2ryYmEF6KLZeCaPtAXsO66tGHUTeQZNSP16tS5rbrEJHdjDGoq4NjMLGQigMsYDyD0bzai3lI8NtC6FGCHtjLwBhzdo5LZxW52Si-uPVxap4U2dZoqFJ9aZMRzeZL5BGwGeUO4_FbCt1H_IzGwzeagyBXrPT-sJ3NqJSUri5XHWrLl4dxzYeqvru14xNRvwPGLDknoy37nQ904N43B5CBcTOm5y6TdoAnexmScOFff3TbEjRoafdWMQEXucGK_ZFyjv4VwAwO7UgQPlQkYsfGx0qFSDy48mbbjGzSctN8NnE0vZH7S8LXrCyLU-2ME2M0igeA20BhigyKtWBIEkk95jm6PUVdi81pINqQITwWG5XIFhmErt8KKj9w4ArShrVLwqfj0C0ModVIOJeCmTmCHwa3t0YLaAwhMX8NfvOCIjaXsO8-Pk4pFs-YBVL-Ge1n6kpP7fNPWITwgeVPVz_QJZig-JFpRSfcLAWl2XSv1Twjy43rHWspumMXCMAbGDQZX5NMtc3kmwaqKNU9qniv6fJvAilKvPdY5qfxkysVHUWrfjU0efurFF8w","supported_experiences":["DENSE_RESULT_PAGE","FAST_FILTERS","FILTERS","FILTERS_AS_SEE_MORE","INSTANT_FILTERS","MARKETPLACE_ON_GLOBAL","MIXED_MEDIA","NATIVE_TEMPLATES","NT_SPLIT_VIEWS","PHOTO_STREAM_VIEWER","SEARCH_INTERCEPT","SEARCH_SNIPPETS_ICONS_ENABLED","SEE_MORE_ON_TABS","USAGE_COLOR_SERP","commerce_groups_search","keyword_only"],"default_image_scale":3,"nt_context":{"using_white_navbar":true,"pixel_ratio":3,"styles_id":"d82700195bed991a354f509243d7d19e","bloks_version":"f51c9d91e91f6e9c27f6f24b876c17457bb277986c33ecd69f5544a9cb39cc85"},"bsid":"6fbb7a56-1bd1-4ea2-99be-990d137b536e","entered_query_text":"","image_high_width":1080,"image_large_aspect_width":1080,"image_low_width":360,"filters_enabled":false,"image_large_aspect_height":565,"image_medium_width":540,"profile_image_size":259,"enable_at_stream":true,"scale":"3","bqf":"keywords_search(salam salam)","search_query_arguments":{"squashed_ent_ids":["100035736461518","100027215852320","100011810234508"]},"ui_theme_name":"APOLLO_FULL_BLEED","query_source":"graph_search_v2_typeahead_keyword_suggestion","independent_module_or_first_unit":true,"tsid":"f67928f3-21a9-4b39-a379-f91f177bda49","product_item_image_size":418,"request_index":0}&
                //fb_api_req_friendly_name=SearchResultsGraphQL-pagination_query&
                //fb_api_caller_class=graphservice&
                //fb_api_analytics_tags=["pagination_query","GraphServices"]&
                //server_timestamps=true
                var data = new Dictionary<string, string>
                {
                    //{"client_doc_id", "39590791075919423816857174440"/*"35067217398338956168214945464"*/},
                    {"client_doc_id", "395907910717822057189298004039"},
                    {"method", "post"},
                    {"locale", _facebookApi.AppLocale},
                    {"pretty", "false"},
                    {"format", "json"},
                    //{"purpose", "fetch"},
                    {"variables", variables.ToString(Formatting.None)},
                    {"fb_api_req_friendly_name", !string.IsNullOrEmpty(pagination.EndCursor) ? "SearchResultsGraphQL-pagination_query" : "SearchResultsGraphQL-main_query"},
                    {"fb_api_caller_class", "graphservice"},
                    {"fb_api_analytics_tags", "[\""+ (!string.IsNullOrEmpty(pagination.EndCursor) ?"pagination_query" :"main_query")+ "\",\"GraphServices\"]"},
                    {"server_timestamps", "true"},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data);
                if (!string.IsNullOrEmpty(pagination.EndCursor))
                    request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "SearchResultsGraphQL-pagination_query", true);
                else
                    request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "SearchResultsGraphQL-main_query", true);

                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_REQUEST_ANALYTICS_TAGS, "graphservice", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_PRIORITY, "u=3, i");
                request.Headers.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));
                //X-FB-Friendly-Name: Fb4aSearchTypeaheadQuery
                //X-FB-Request-Analytics-Tags: graphservice

                var response = await _httpRequestProcessor.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<string>(response, responseText);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(string));

                responseContent = responseText;
                if (responseText.Contains("\"errors\"") || responseText.Contains("\"error\""))
                    return Result.UnExpectedResponse<string>(response, responseText);

                return Result.Success(responseText);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(string), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<string>(exception, responseContent);
            }
        }

    }
}

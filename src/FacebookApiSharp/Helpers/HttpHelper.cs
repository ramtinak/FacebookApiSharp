/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.API;
using FacebookApiSharp.Classes;
using FacebookApiSharp.Classes.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace FacebookApiSharp.Helpers
{
    public class HttpHelper
    {
        public IHttpRequestProcessor _httpRequestProcessor;
        public IFacebookApi _facebookApi;
        internal HttpHelper(IHttpRequestProcessor httpRequestProcessor, IFacebookApi facebookApi)
        {
            _httpRequestProcessor = httpRequestProcessor;
            _facebookApi = facebookApi;
        }

        internal static readonly System.Globalization.CultureInfo EnglishCulture = new System.Globalization.CultureInfo("en-us");
        internal static System.Globalization.CultureInfo GetCurrentCulture() => System.Globalization.CultureInfo.CurrentCulture;

        public HttpRequestMessage GetDefaultRequest(HttpMethod method,
            Uri uri,
            AndroidDevice deviceInfo,
            bool userAgentGenTwo = false)
        {
            var currentCulture = GetCurrentCulture();
            System.Globalization.CultureInfo.CurrentCulture = EnglishCulture;
            var userAgent = deviceInfo.GenerateUserAgent(_facebookApi, userAgentGenTwo);
            var currentUser = _facebookApi.GetLoggedUser();
            var request = new HttpRequestMessage(method, uri);

            //X-FB-Connection-Quality: EXCELLENT
            //X-FB-SIM-HNI: 43235
            //X-FB-Net-HNI: 43211
            //X-FB-Connection-Type: unknown
            //User-Agent: [FBAN/FB4A;FBAV/355.0.0.21.108;FBBV/352948159;FBDM/{density=2.75,width=1080,height=2111};FBLC/en_US;FBRV/0;FBCR/IR-MCI;FBMF/Xiaomi;FBBD/Redmi;FBPN/com.facebook.katana;FBDV/M2007J22G;FBSV/11;FBBK/1;FBOP/1;FBCA/arm64-v8a:;]
            //Host: graph.facebook.com
            //Content-Type: application/x-www-form-urlencoded
            //X-Tigon-Is-Retry: False
            //x-fb-device-group: 7864
            //X-FB-Friendly-Name: suggestedLanguages
            //X-FB-Request-Analytics-Tags: unknown
            //Accept-Encoding: gzip, deflate
            //X-FB-HTTP-Engine: Liger
            //X-FB-Client-IP: True
            //X-FB-Server-Cluster: True

            request.Headers.Add(FacebookApiConstants.HEADER_FB_CONNECTION_QUALITY, "EXCELLENT");
            request.Headers.Add(FacebookApiConstants.HEADER_FB_SIM_HNI, "unknown");
            request.Headers.Add(FacebookApiConstants.HEADER_FB_NET_HNI, "unknown");
            request.Headers.Add(FacebookApiConstants.HEADER_FB_CONNECTION_TYPE, "WIFI");
            // Authorization: OAuth 
            if(IsLoggedIn() && !string.IsNullOrEmpty(currentUser.LoggedInUser.AccessToken))
            {
                request.Headers.Add(FacebookApiConstants.HEADER_AUTHORIZATION, $"OAuth {currentUser.LoggedInUser.AccessToken}");
            }
            request.Headers.TryAddWithoutValidation(FacebookApiConstants.HEADER_USER_AGENT, userAgent);
            request.Headers.Add(FacebookApiConstants.HEADER_FB_DEVICE_GROUP, "7864");
            request.Headers.Add(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "suggestedLanguages");
            request.Headers.Add(FacebookApiConstants.HEADER_FB_REQUEST_ANALYTICS_TAGS, "unknown");
            request.Headers.Add(FacebookApiConstants.HEADER_FB_HTTP_ENGINE, "Liger");
            request.Headers.Add(FacebookApiConstants.HEADER_FB_SERVER_CLUSTER, "True");
            request.Headers.Add(FacebookApiConstants.HEADER_X_IG_TIGON_RETRY, "False");


            request.Headers.Add(FacebookApiConstants.HEADER_ACCEPT_LANGUAGE, _facebookApi.AcceptLanguage);

            request.Headers.TryAddWithoutValidation(FacebookApiConstants.HEADER_ACCEPT_ENCODING, FacebookApiConstants.ACCEPT_ENCODING2);

            //request.Headers.Add(FacebookApiConstants.HOST, FacebookApiConstants.HOST_URI);

            System.Globalization.CultureInfo.CurrentCulture = currentCulture;

            bool IsLoggedIn()
            {
                return _facebookApi.IsUserAuthenticated && currentUser.LoggedInUser != null;
            }

            return request;
        }
        public HttpRequestMessage GetDefaultRequest(HttpMethod method,
            Uri uri, AndroidDevice deviceInfo, 
            Dictionary<string, string> data,
            bool userAgentGenTwo = false)
        {
            var request = GetDefaultRequest(HttpMethod.Post, uri, deviceInfo, userAgentGenTwo);
            request.Content = new FormUrlEncodedContent(data);
            return request;
        }
    }
}

/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookLoginSessionResponse
    {
        [JsonProperty("session_key")]
        public string SessionKey { get; set; }
        [JsonProperty("uid")]
        public long UId { get; set; }
        [JsonProperty("secret")]
        public string Secret { get; set; }
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("session_cookies")]
        public FacebookLoginSessionCookieResponse[] SessionCookies { get; set; }
        [JsonProperty("analytics_claim")]
        public string AnalyticsClaim { get; set; }
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }
        [JsonProperty("identifier")]
        public string Identifier { get; set; }
        [JsonProperty("user_storage_key")]
        public string UserStorageKey { get; set; }
    }

    public class FacebookLoginSessionCookieResponse
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("expires")]
        public string Expires { get; set; }
        [JsonProperty("expires_timestamp")]
        public long ExpiresTimestamp { get; set; }
        [JsonProperty("domain")]
        public string Domain { get; set; }
        [JsonProperty("path")]
        public string Path { get; set; }
        [JsonProperty("secure")]
        public bool Secure { get; set; }
        [JsonProperty("httponly")]
        public bool HttpOnly { get; set; }
    }
}

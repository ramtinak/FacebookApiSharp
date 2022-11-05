/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookLoginApprovalsKeyResponse
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("time_offset")]
        public string TimeOffset { get; set; }
    }
}

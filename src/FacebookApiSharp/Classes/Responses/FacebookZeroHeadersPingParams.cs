/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookZeroHeadersPingParams
    {
        [JsonProperty("clear")]
        public bool Clear { get; set; }
        [JsonProperty("next_cursor")]
        public string NextCursor { get; set; }
        [JsonProperty("cooldown_on_success")]
        public int CooldownOnSuccess { get; set; }
        [JsonProperty("cooldown_on_failure")]
        public int CooldownOnFailure { get; set; }
        [JsonProperty("transparency_content_type")]
        public int TransparencyContentType { get; set; }
        [JsonProperty("carrier_id")]
        public int CarrierId { get; set; }
        [JsonProperty("consent_required")]
        public bool ConsentRequired { get; set; }
        [JsonProperty("client_header_params")]
        public FacebookClientHeaderParams ClientHeaderParams { get; set; }
        [JsonProperty("headwind_immediate")]
        public bool HeadwindImmediate { get; set; }
        [JsonProperty("user_signal_required")]
        public bool UserSignalRequired { get; set; }
    }

    public class FacebookClientHeaderParams
    {
        [JsonProperty("is_jio_headers_enabled")]
        public bool IsJioHeadersEnabled { get; set; }
    }

}

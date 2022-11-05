/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookLoggingClientEvents

    {
        [JsonProperty("checksum")]
        public string Checksum { get; set; }
        [JsonProperty("config")]
        public string Config { get; set; }
        [JsonProperty("config_owner_id")]
        public string ConfigOwnerId { get; set; }
        [JsonProperty("app_data")]
        public string AppData { get; set; }
        [JsonProperty("qpl_version")]
        public string QplVersion { get; set; }
    }
}

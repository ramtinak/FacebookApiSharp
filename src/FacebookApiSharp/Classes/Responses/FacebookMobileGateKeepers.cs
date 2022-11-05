/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;
namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookMobileGateKeepers
    {
        [JsonProperty("result")]
        public string Result { get; set; }
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}

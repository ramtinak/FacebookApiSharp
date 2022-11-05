/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes
{
    public class FacebookNetworkInterfaceResponse
    {
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("cat_param")]
        public string CatParam { get; set; }
        [JsonProperty("rules")]
        public object Rules { get; set; }
        [JsonProperty("hints")]
        public object Hints { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("tgt_ip")]
        public string TgtIp { get; set; }
        [JsonProperty("net_iface")]
        public string NetIface { get; set; }
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}

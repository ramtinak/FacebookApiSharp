/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;
using System.Collections.Generic;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookLocaleLanguages
    {
        [JsonProperty("locales")]
        public List<FacebookLocale> Locales { get; set; } = new List<FacebookLocale>();
    }

    public class FacebookLocale
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }
        [JsonProperty("weight")]
        public float Weight { get; set; }
    }
}

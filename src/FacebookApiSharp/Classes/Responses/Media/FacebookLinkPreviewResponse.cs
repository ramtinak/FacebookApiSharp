/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookLinkPreviewDataResponse
    {
        [JsonProperty("link_preview")]
        public FacebookLinkPreviewResponse LinkPreview { get; set; }
    }

    public class FacebookLinkPreviewResponse
    {
        [JsonProperty("share_scrape_data")]
        public string ShareScrapeData { get; set; }
    }
}

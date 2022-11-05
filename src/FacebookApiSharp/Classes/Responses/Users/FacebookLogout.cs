/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookLogoutDataStory
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class FacebookLogoutDataStoryCuration
    {
        [JsonProperty("story")]
        public FacebookLogoutDataStory Story { get; set; }
    }

    public class FacebookLogoutData
    {
        [JsonProperty("activity_log_story_curation")]
        public FacebookLogoutDataStoryCuration ActivityLogStoryCuration { get; set; }
    }
}

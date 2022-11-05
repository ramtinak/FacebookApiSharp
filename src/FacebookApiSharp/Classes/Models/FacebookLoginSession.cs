/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System.Collections.Generic;
using System.Net;

namespace FacebookApiSharp.Classes.Models
{
    public class FacebookLoginSession
    {
        public string SessionKey { get; set; }
        public long UId { get; set; }
        public string Secret { get; set; }
        public string AccessToken { get; set; }
        public string AnalyticsClaim { get; set; }
        public bool Confirmed { get; set; }
        public string Identifier { get; set; }
        public string UserStorageKey { get; set; }
        public List<Cookie> RawCookies { get; internal set; } = new List<Cookie>();
    }
}

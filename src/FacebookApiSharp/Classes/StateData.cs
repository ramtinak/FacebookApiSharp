/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Net;
namespace FacebookApiSharp.Classes
{
    [Serializable]
    public class StateData
    {
        public AndroidDevice DeviceInfo { get; set; }
        public UserSessionData UserSession { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<Cookie> RawCookies { get; set; }

        #region Locale

        public string SimCountry { get; set; } = "unknown";
        public string NetworkCountry { get; set; } = "unknown";
        public string ClientCountryCode { get; set; } = "unknown";
        public string DeviceLocale { get; set; }
        public string AppLocale { get; set; }
        public string AcceptLanguage { get; set; }

        #endregion

    }
}
/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes.Models;
using FacebookApiSharp.Classes.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookApiSharp.Converters
{
    class FacebookLoginSessionConverter : IObjectConverter<FacebookLoginSession, FacebookLoginSessionResponse>
    {
        public FacebookLoginSessionResponse SourceObject { get; set; }

        public FacebookLoginSession Convert()
        {
            if (SourceObject == null) throw new ArgumentNullException($"Source object");
            var loginSession = new FacebookLoginSession
            {
                AccessToken = SourceObject.AccessToken,
                Secret = SourceObject.Secret,
                SessionKey = SourceObject.SessionKey,
                UserStorageKey = SourceObject.UserStorageKey,
                AnalyticsClaim = SourceObject.AnalyticsClaim,
                Confirmed = SourceObject.Confirmed,
                Identifier = SourceObject.Identifier,
                UId = SourceObject.UId,
            };
            return loginSession;
        }
    }
}

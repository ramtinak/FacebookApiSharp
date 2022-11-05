/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes.Models;
using FacebookApiSharp.Classes.Responses;
using System;

namespace FacebookApiSharp.Converters
{
    public class ObjectsConverter
    {
        private static readonly Lazy<ObjectsConverter> LazyInstance =
            new Lazy<ObjectsConverter>(() => new ObjectsConverter());
        public static ObjectsConverter Instance => LazyInstance.Value;


        public IObjectConverter<FacebookLoginSession, FacebookLoginSessionResponse> GetUserShortConverter(
            FacebookLoginSessionResponse response)
        {
            return new FacebookLoginSessionConverter { SourceObject = response };
        }

        public IObjectConverter<FacebookMessage, FacebookMessageContainerResponse> GetDirectMessageConverter(
            FacebookMessageContainerResponse response)
        {
            return new FacebookMessageConverter { SourceObject = response };
        }

        public IObjectConverter<FacebookInboxFriends, FacebookInboxFriendsResponse> GetDirectInboxFriendsConverter(
            FacebookInboxFriendsResponse response)
        {
            return new FacebookInboxFriendsConverter { SourceObject = response };
        }
    }
}
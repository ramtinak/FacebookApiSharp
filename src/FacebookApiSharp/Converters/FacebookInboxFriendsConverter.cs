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
    class FacebookInboxFriendsConverter : IObjectConverter<FacebookInboxFriends, FacebookInboxFriendsResponse>
    {
        public FacebookInboxFriendsResponse SourceObject { get; set; }

        public FacebookInboxFriends Convert()
        {
            if (SourceObject == null) throw new ArgumentNullException($"Source object");
            var inboxFriends = new FacebookInboxFriends
            {
                SelfUserId = SourceObject.Data?.Viewer?.AccountUser?.Id,
                Count = SourceObject.Data?.Viewer?.AccountUser?.Friends?.Count ?? 0
            };
            if (SourceObject.Data?.Viewer?.AccountUser?.Friends?.Nodes?.Length > 0)
            {
                foreach(var item in SourceObject.Data.Viewer.AccountUser.Friends.Nodes)
                {
                    inboxFriends.Users.Add(new FacebookInboxFriendsUser
                    {
                        Id = item.Id,
                        ShortName = item.ShortName,
                        Name = item.Name,
                        ProfilePicture = item.ProfilePicture?.Uri
                    });
                }
            }
            return inboxFriends;
        }
    }
}

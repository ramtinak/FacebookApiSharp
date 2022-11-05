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
    class FacebookInboxTopFriendsConverter : IObjectConverter<FacebookInboxTopFriends, FacebookInboxFriendsResponse>
    {
        public FacebookInboxFriendsResponse SourceObject { get; set; }

        public FacebookInboxTopFriends Convert()
        {
            if (SourceObject == null) throw new ArgumentNullException($"Source object");
            var inboxTopFriends = new FacebookInboxTopFriends
            {
                SelfUserId = SourceObject.Data?.Viewer?.AccountUser?.Id,
                Count = SourceObject.Data?.Viewer?.AccountUser?.Friends?.Count ?? 0
            };
            if (SourceObject.Data?.Viewer?.AccountUser?.TopFriends?.Nodes?.Length > 0)
            {
                foreach (var item in SourceObject.Data.Viewer.AccountUser.TopFriends.Nodes)
                {
                    inboxTopFriends.Users.Add(new FacebookInboxTopFriendsUser
                    {
                        Id = item.Id,
                        ShortName = item.ShortName,
                        Name = item.Name,
                        ProfilePicture = item.ProfilePicture?.Uri,
                        LastActiveMessagesStatusTime = item.LastActiveMessagesStatus?.Time,
                        IsCurrentlyActive = item.LastActiveMessagesStatus?.IsCurrentlyActive
                    });
                }
            }
            if (SourceObject.Data?.Viewer?.AccountUser?.TopFriendsWithMessenger?.Nodes?.Length > 0)
            {
                foreach (var item in SourceObject.Data.Viewer.AccountUser.TopFriendsWithMessenger.Nodes)
                {
                    inboxTopFriends.Users.Add(new FacebookInboxTopFriendsUser
                    {
                        Id = item.Id,
                        ShortName = item.ShortName,
                        Name = item.Name,
                        ProfilePicture = item.ProfilePicture?.Uri,
                        LastActiveMessagesStatusTime = item.LastActiveMessagesStatus?.Time,
                        IsCurrentlyActive = item.LastActiveMessagesStatus?.IsCurrentlyActive
                    });
                }
            }
            return inboxTopFriends;
        }
    }
}

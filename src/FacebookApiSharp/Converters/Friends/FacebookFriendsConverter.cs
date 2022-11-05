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
    class FacebookFriendsConverter : IObjectConverter<FacebookFriends, FacebookPaginationResultResponse<FacebookFriendsDataResponse>>
    {
        public FacebookPaginationResultResponse<FacebookFriendsDataResponse> SourceObject { get; set; }

        public FacebookFriends Convert()
        {
            if (SourceObject == null) throw new ArgumentNullException($"Source object");
            var friends = new FacebookFriends
            {
                PageInfo = SourceObject.Data?.Viewer?.DynamicFriendingTab?.PageInfo,
                FriendRequestCount = SourceObject.Data?.Viewer?.DynamicFriendingTab?.FriendRequestCount ?? 0
            };
            if (SourceObject.Data?.Viewer?.DynamicFriendingTab?.Edges?.Length > 0)
            {
                foreach (var item in SourceObject.Data.Viewer.DynamicFriendingTab.Edges)
                {
                    if (item.Node?.User != null)
                    {
                        friends.Users.Add(item.Node.User);
                    }
                }
            }
            return friends;
        }
    }
}

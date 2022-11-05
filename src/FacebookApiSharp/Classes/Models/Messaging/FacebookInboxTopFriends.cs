/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookApiSharp.Classes.Models
{
    public class FacebookInboxTopFriends
    {
        public string SelfUserId { get; set; }
        public int Count { get; set; }
        public List<FacebookInboxTopFriendsUser> Users { get; set; } = new List<FacebookInboxTopFriendsUser>();
    }
    public class FacebookInboxTopFriendsUser : FacebookInboxFriendsUser
    {
        public string IsCurrentlyActive { get; set; }
        public long? LastActiveMessagesStatusTime { get; set; }
    }
}

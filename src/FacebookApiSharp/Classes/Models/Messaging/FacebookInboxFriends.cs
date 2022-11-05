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
    public class FacebookInboxFriends
    {
        public string SelfUserId { get; set; }
        public int Count { get; set; }
        public List<FacebookInboxFriendsUser> Users { get; set; } = new List<FacebookInboxFriendsUser>();
    }
    public class FacebookInboxFriendsUser
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string ProfilePicture { get; set; }
        public string Id { get; set; }
    }
}

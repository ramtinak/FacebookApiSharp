/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookApiSharp.Classes.Models
{
    public class FacebookFriends
    {
        public int FriendRequestCount { get; set; }
        public FacebookPageInfoResponse PageInfo { get; set; }
        public List<FacebookActorResponse> Users { get; set; } = new List<FacebookActorResponse>();
    }
}

/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */
using FacebookApiSharp.Classes;
using FacebookApiSharp.Classes.DeviceInfo;
using FacebookApiSharp.Classes.Models;
using FacebookApiSharp.Classes.Responses;
using FacebookApiSharp.Classes.SessionHandlers;
using FacebookApiSharp.Converters;
using FacebookApiSharp.Enums;
using FacebookApiSharp.Helpers;
using FacebookApiSharp.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FacebookApiSharp.API.Processors
{
    public interface IMessagingProcessor
    {
        Task<IResult<FacebookMessage>> SendDirectMessageAsync(string userId, string message);
        Task<IResult<FacebookInboxFriends>> GetDirectInboxFriendsAsync();
        Task<IResult<FacebookInboxTopFriends>> GetDirectInboxTopFriendsAsync();
        Task<IResult<FacebookInboxFriends>> GetDirectInboxRecentltyActiveFriendsAsync();

    }
}

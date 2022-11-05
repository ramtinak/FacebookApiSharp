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
    public class FacebookMessage
    {
        public string Typename { get; set; }
        public string MessageId { get; set; }
        public string OfflineThreadingId { get; set; }
        public string TimestampPrecise { get; set; }
        public string Snippet { get; set; }
        public string UnsentTimestampPrecise { get; set; }
        public FacebookActorResponse MessageSender { get; set; }
        public string Text { get; set; }
        public string Id { get; set; }
        public string[] TagsList { get; set; }
        public string StrongId { get; set; }
        public string ClientMutationId { get; set; }
    }
}

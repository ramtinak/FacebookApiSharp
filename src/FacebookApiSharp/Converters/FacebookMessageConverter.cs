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
    class FacebookMessageConverter : IObjectConverter<FacebookMessage, FacebookMessageContainerResponse>
    {
        public FacebookMessageContainerResponse SourceObject { get; set; }

        public FacebookMessage Convert()
        {
            if (SourceObject == null) throw new ArgumentNullException($"Source object");
            var ss = SourceObject.Data.MessagingInBlueSendMessage.Message;
            var message = new FacebookMessage
            {
                ClientMutationId = SourceObject.Data.MessagingInBlueSendMessage.ClientMutationId,
                Id = ss.Id,
                Snippet = ss.Snippet,
                StrongId = ss.StrongId,
                MessageSender = ss.MessageSender.MessagingActor,
                MessageId = ss.MessageId,
                OfflineThreadingId = ss.OfflineThreadingId,
                TagsList = ss.TagsList,
                Text = ss.Message.Text,
                TimestampPrecise = ss.TimestampPrecise,
                Typename = ss.Typename,
                UnsentTimestampPrecise = ss.UnsentTimestampPrecise
            };
            return message;
        }
    }
}

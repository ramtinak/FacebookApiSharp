/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookMessageContainerResponse
    {
        [JsonProperty("data")]
        public FacebookMessageDataResponse Data { get; set; }
        [JsonProperty("extensions")]
        public FacebookPageResultExtensionsResponse Extensions { get; set; }
    }
    public class FacebookMessageDataResponse
    {
        [JsonProperty("messaging_in_blue_send_message")]
        public FacebookMessagingInBlueSendMessageResponse MessagingInBlueSendMessage { get; set; }
    }

    public class FacebookMessagingInBlueSendMessageResponse
    {
        [JsonProperty("client_mutation_id")]
        public string ClientMutationId { get; set; }
        [JsonProperty("message")]
        public FacebookMessageResponse Message { get; set; }
    }

    public class FacebookMessageResponse
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }
        [JsonProperty("message_id")]
        public string MessageId { get; set; }
        [JsonProperty("offline_threading_id")]
        public string OfflineThreadingId { get; set; }
        [JsonProperty("timestamp_precise")]
        public string TimestampPrecise { get; set; }
        [JsonProperty("snippet")]
        public string Snippet { get; set; }
        [JsonProperty("unsent_timestamp_precise")]
        public string UnsentTimestampPrecise { get; set; }
        [JsonProperty("message_sender")]
        public FacebookMessageSenderResponse MessageSender { get; set; }
        [JsonProperty("message")]
        public FacebookInnerMessageResponse Message { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("tags_list")]
        public string[] TagsList { get; set; }
        [JsonProperty("strong_id__")]
        public string StrongId { get; set; }
    }

    public class FacebookMessageSenderResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("messaging_actor")]
        public FacebookActorResponse MessagingActor { get; set; }
    }


    public class FacebookInnerMessageResponse
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }


}

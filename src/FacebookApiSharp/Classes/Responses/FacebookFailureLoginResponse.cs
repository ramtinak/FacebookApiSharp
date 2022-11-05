/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;

namespace FacebookApiSharp.Classes.Responses
{
    public class FacebookFailureLoginResponse
    {
        [JsonProperty("error")]
        public FacebookFailureLoginErrorResponse Error { get; set; }
    }

    public class FacebookFailureLoginErrorResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("error_data")]
        public FacebookFailureLoginErrorDataResponse ErrorData { get; set; }
        [JsonProperty("error_subcode")]
        public int ErrorSubcode { get; set; }
        [JsonProperty("is_transient")]
        public bool IsTransient { get; set; }
        [JsonProperty("error_user_title")]
        public string ErrorUserTitle { get; set; }
        [JsonProperty("error_user_msg")]
        public string ErrorUserMsg { get; set; }
        [JsonProperty("fbtrace_id")]
        public string FbtraceId { get; set; }
    }

    public class FacebookFailureLoginErrorDataResponse
    {
        [JsonProperty("pwd_enc_key_pkg")]
        public FacebookPwdEncKeyPkgResponse PwdEncKeyPkg { get; set; }
        [JsonProperty("error_subcode")]
        public int ErrorSubcode { get; set; }
        [JsonProperty("cpl_info")]
        public FacebookFailureLoginErrorDataCplInfoResponse CplInfo { get; set; }

    }

    public class FacebookPwdEncKeyPkgResponse
    {
        [JsonProperty("key_id")]
        public int KeyId { get; set; }
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
        [JsonProperty("seconds_to_live")]
        public int SecondsToLive { get; set; }
    }
    public class FacebookFailureLoginErrorDataCplInfoResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("profile_pic_uri")]
        public string ProfilePicUri { get; set; }
        [JsonProperty("cpl_eligible")]
        public bool CplEligible { get; set; }
        [JsonProperty("cpl_after_openid")]
        //public Contactpoints contactpoints { get; set; }
        public bool CplAfterOpenid { get; set; }
        [JsonProperty("cpl_group")]
        public int CplGroup { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("password_reset_nonce_length")]
        public int PasswordResetNonceLength { get; set; }
        [JsonProperty("cpl_sms_retriever_auto_submit_test_group")]
        public string CplSmsRetrieverAutoSubmitTestGroup { get; set; }
        [JsonProperty("nonce_send_status")]
        public int NonceSendStatus { get; set; }
        [JsonProperty("show_dbl_cpl_interstitial")]
        public bool ShowDblCplInterstitial { get; set; }
    }

    //public class Contactpoints
    //{
    //    public _0 _0 { get; set; }
    //}

    //public class _0
    //{
    //    public string id { get; set; }
    //    public string display { get; set; }
    //    public string type { get; set; }
    //}

}

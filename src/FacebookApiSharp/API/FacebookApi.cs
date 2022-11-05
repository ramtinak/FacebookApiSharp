/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.API.Processors;
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

namespace FacebookApiSharp.API
{
    internal class FacebookApi : IFacebookApi
    {
        public UserSessionData User { get; private set; }
        private IFacebookLogger _logger;
        private AndroidDevice _deviceInfo;
        private IHttpRequestProcessor _httpRequestProcessor;
        private HttpHelper _httpHelper;

        private bool _isUserAuthenticated;

        #region Properties
        /// <summary>
        ///     Indicates whether user authenticated or not
        /// </summary>
        public bool IsUserAuthenticated
        {
            get { return _isUserAuthenticated; }
            internal set { _isUserAuthenticated = value; }
        }
        public ISessionHandler SessionHandler { get; set; }
        public string AppLocale { get; set; }
        public string DeviceLocale { get; set; }
        public string AcceptLanguage { get; set; }
        public string SimCountry { get; set; } = "unknown";
        public string NetworkCountry { get; set; } = "unknown";
        public string ClientCountryCode { get; set; } = "unknown";
        public IMessagingProcessor MessagingProcessor { get; private set; }

        public IAccountProcessor AccountProcessor { get; private set; }
        public IUserProcessor UserProcessor { get; private set; }
        public IMediaProcessor MediaProcessor { get; private set; }
        public UserSessionData GetLoggedUser() => User;

        public AndroidDevice GetCurrentDevice() => _deviceInfo;
        #endregion Properties
        public FacebookApi(UserSessionData user,
            IFacebookLogger logger,
            AndroidDevice deviceInfo,
            IHttpRequestProcessor httpRequestProcessor)
        {
            User = user;
            _logger = logger;
            _deviceInfo = deviceInfo;
            _httpRequestProcessor = httpRequestProcessor;
            _httpHelper = new HttpHelper(httpRequestProcessor, this); 
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }



        public async Task<IResult<FacebookLocaleLanguages>> GetAppLocaleSuggestionsAsync()
        {
            string responseContent = null;

            try
            {
                var instaUri = UriCreator.GetAppLocaleSuggestionsUri();
                //format=json&
                //device_locale=en_US&
                //sim_country=ir&
                //network_country=ir&
                //device_id=c2846f25-54b3-434a-be1c-72e9a71573a3&
                //locale=en_US&
                //client_country_code=IR&
                //fb_api_req_friendly_name=suggestedLanguages&
                //fb_api_caller_class=LanguageSwitcher&
                //api_key=882a8490361da98702bf97a021ddc14d&
                //sig=b9b035a28341257e2874e199ffd4fba9&
                //access_token=350685531728|62f8ce9f74b12f84c123cc23437a4a32
                var data = new Dictionary<string, string>
                {
                    {"format", "json"},
                    {"device_locale", DeviceLocale.Replace("-","_")},
                    //{"sim_country", SimCountry},
                    {"network_country", NetworkCountry},
                    {"device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"locale", AppLocale.Replace("-","_")},
                    {"client_country_code", ClientCountryCode},
                    {"fb_api_req_friendly_name","suggestedLanguages"},
                    {"fb_api_caller_class","LanguageSwitcher"},
                    {"api_key", FacebookApiConstants.FACEBOOK_API_KEY},
                    {"sig", CryptoHelper.CalculateMd5()},
                    {"access_token", FacebookApiConstants.FACEBOOK_ACCESS_TOKEN},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, _deviceInfo, data);
                var response = await _httpRequestProcessor.SendAsync(request);

                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookLocaleLanguages>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookLocaleLanguages));

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookLocaleLanguages>(response, json);

                var obj = JsonConvert.DeserializeObject<FacebookLocaleLanguages>(json);

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookLocaleLanguages), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookLocaleLanguages>(exception, responseContent);
            }
        }


        public async Task<IResult<FacebookZeroHeadersPingParams>> GetZeroHeadersPingParamsAsync()
        {
            string responseContent = null;
            try
            {
                var instaUri = UriCreator.GetZeroHeadersPingParamsUri();
                //logged_out_id=c2846f25-54b3-434a-be1c-72e9a71573a3&
                //c=GwAA&
                //interface=wifi&
                //reason=unknown&
                //headwind_version=3&
                //headwind_cursor={}&
                //locale=en_US&
                //client_country_code=IR&
                //method=GET&
                //fb_api_req_friendly_name=headersConfigurationParamsV2&
                //fb_api_caller_class=HeadersV2ConfigFetchRequestHandler&
                //access_token=350685531728|62f8ce9f74b12f84c123cc23437a4a32
                var data = new Dictionary<string, string>
                {
                    {"logged_out_id", _deviceInfo.DeviceGuid.ToString()},
                    {"c", "GwAA"},
                    {"interface", "wifi"},
                    {"reason", "unknown"},
                    {"headwind_version", "3"},
                    {"headwind_cursor", "{}"},
                    {"locale", AppLocale.Replace("-","_")},
                    {"client_country_code", ClientCountryCode},
                    {"method","GET"},
                    {"fb_api_req_friendly_name","headersConfigurationParamsV2"},
                    {"fb_api_caller_class","HeadersV2ConfigFetchRequestHandler"},
                    {"access_token", FacebookApiConstants.FACEBOOK_ACCESS_TOKEN},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data, true);
                request.Headers.AddHeader("Authorization", "OAuth null");
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "headersConfigurationParamsV2", true);
                
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookZeroHeadersPingParams>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookZeroHeadersPingParams));

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookZeroHeadersPingParams>(response, json);
                var obj = JsonConvert.DeserializeObject<FacebookZeroHeadersPingParams>(json);

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookZeroHeadersPingParams), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookZeroHeadersPingParams>(exception, responseContent);
            }
        }

        public async Task<IResult<FacebookMobileGateKeepers>> GetMobileGatekeepersAsync()
        {
            string responseContent = null;

            try
            {
                var instaUri = UriCreator.GetMobileGatekeepersUri();
                //format=json&
                //query_hash=D798CEFD68368EBB2245135BA1842BD69CFA300D&
                //hash_id=c2846f25-54b3-434a-be1c-72e9a71573a3&
                //access_token=350685531728|62f8ce9f74b12f84c123cc23437a4a32&
                //locale=en_US&
                //client_country_code=IR&
                //fb_api_req_friendly_name=fetchSessionlessGKInfo&
                //fb_api_caller_class=FetchMobileGatekeepersMethod&
                //api_key=882a8490361da98702bf97a021ddc14d&
                //sig=b4d7ecc6ae0ae7da5a602c2c9fe4082b
                var data = new Dictionary<string, string>
                {
                    {"format", "json"},
                    //{"query_hash", Guid.NewGuid().ToString().Replace("-","").ToUpper()},
                    {"query_hash", "D798CEFD68368EBB2245135BA1842BD69CFA300D"},
                    {"hash_id", _deviceInfo.DeviceGuid.ToString()},
                    {"access_token", FacebookApiConstants.FACEBOOK_ACCESS_TOKEN},
                    {"locale", AppLocale.Replace("-","_")},
                    {"client_country_code", ClientCountryCode},
                    {"fb_api_req_friendly_name","fetchSessionlessGKInfo"},
                    {"fb_api_caller_class","FetchMobileGatekeepersMethod"},
                    {"api_key", FacebookApiConstants.FACEBOOK_API_KEY},
                    {"sig", CryptoHelper.CalculateMd5()},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data, true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "fetchSessionlessGKInfo", true);

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookMobileGateKeepers>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookMobileGateKeepers));

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookMobileGateKeepers>(response, json);

                var obj = JsonConvert.DeserializeObject<FacebookMobileGateKeepers>(json);

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookMobileGateKeepers), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookMobileGateKeepers>(exception, responseContent);
            }
        }

        internal async Task<IResult<FacebookLoggingClientEvents>> GetLoggingClientEventsAsync()
        {
            string responseContent = null;

            try
            {
                var instaUri = UriCreator.GetLoggingClientEventsUri();

                //X-FB-Friendly-Name: sendAnalyticsLog
                //X-FB-Request-Analytics-Tags: unknown
                //Accept-Encoding: gzip, deflate
                //X-FB-HTTP-Engine: Liger
                //X-FB-Client-IP: True
                //X-FB-Server-Cluster: True
                //Connection: close
                //Content-Length: 2219

                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="cmsg"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit

                //binary
                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="compressed"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit

                //0
                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="cmethod"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit

                //gzip
                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="locale"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit

                //en_US
                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="client_country_code"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit

                //IR
                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="fb_api_req_friendly_name"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit

                //sendAnalyticsLog
                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="fb_api_caller_class"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit

                //FbHttpUploader
                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="access_token"
                //Content-Type: text/plain; charset=UTF-8
                //Content-Transfer-Encoding: 8bit

                //350685531728|62f8ce9f74b12f84c123cc23437a4a32
                //--CZ2U_XomI2ps3nOrgnmBKLNxH3R7nuhwG1
                //Content-Disposition: form-data; name="cmsg"; filename="message"
                //Content-Type: application/octet-stream
                //Content-Transfer-Encoding: binary

                var data = new Dictionary<string, string>
                {
                    {"format", "json"},
                    //{"query_hash", Guid.NewGuid().ToString().Replace("-","").ToUpper()},
                    {"query_hash", "D798CEFD68368EBB2245135BA1842BD69CFA300D"},
                    {"hash_id", _deviceInfo.DeviceGuid.ToString()},
                    {"access_token", FacebookApiConstants.FACEBOOK_ACCESS_TOKEN},
                    {"locale", AppLocale.Replace("-","_")},
                    {"client_country_code", ClientCountryCode},
                    {"fb_api_req_friendly_name","fetchSessionlessGKInfo"},
                    {"fb_api_caller_class","FetchMobileGatekeepersMethod"},
                    {"api_key", FacebookApiConstants.FACEBOOK_API_KEY},
                    {"sig", CryptoHelper.CalculateMd5()},
                };
                
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "fetchSessionlessGKInfo", true);

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookLoggingClientEvents>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookLoggingClientEvents));

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookLoggingClientEvents>(response, json);
                var obj = JsonConvert.DeserializeObject<FacebookLoggingClientEvents>(json);

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookLoggingClientEvents), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookLoggingClientEvents>(exception, responseContent);
            }
        }


        public async Task<IResult<bool>> SendLoginFlowsAsync()
        {
            try
            {
                await GetAppLocaleSuggestionsAsync();
                await GetZeroHeadersPingParamsAsync();
                await GetMobileGatekeepersAsync();
                await GetPasswordEncrytionKeyAsync();

                return Result.Success(true);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }
        public async Task<IResult<FacebookLoginResult>> LoginAsync(bool secondTime = false)
        {
            string responseContent = null;

            try
            {
                var instaUri = UriCreator.GetAuthLoginUri();
                
                if (string.IsNullOrEmpty(User.PublicKey) || string.IsNullOrEmpty(User.PublicKeyId))
                    await SendLoginFlowsAsync();

                var password = this.GetEncryptedPassword(User.Password);
                var data = new Dictionary<string, string>
                {
                    {"adid", _deviceInfo.AdId.ToString()},
                    {"format", "json"},
                    {"device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"email", User.User},
                    {"password", password},
                    {"generate_analytics_claim","1"},
                    {"community_id",""},
                    {"cpl","true"},
                    {"try_num","1"},
                    {"cds_experiment_group","-1"},

                    {"family_device_id", _deviceInfo.FamilyDeviceGuid.ToString()},
                    {"secure_family_device_id", _deviceInfo.PhoneGuid.ToString()},
                    {"credentials_type","password"},
                    {"fb4a_shared_phone_cpl_experiment","fb4a_shared_phone_nonce_cpl_at_risk_v3"},
                    {"fb4a_shared_phone_cpl_group","enable_v3_at_risk"},
                    {"enroll_misauth","false"},
                    {"generate_session_cookies","1"},
                    {"error_detail_type","button_with_disabled"},
                    {"source","login"},
                    {"machine_id", User.MachineId},
                    {"jazoest", ExtensionsHelper.GenerateJazoest(_deviceInfo.DeviceGuid.ToString())},


                    

                //meta_inf_fbmeta=&
                //advertiser_id=-db16-41c4-b581-&
                //encrypted_msisdn=&
                //currently_logged_in_userid=0&
                //locale=en_US&
                //client_country_code=IR&
                //fb_api_req_friendly_name=authenticate&
                //fb_api_caller_class=Fb4aAuthHandler&
                //api_key=882a8490361da98702bf97a021ddc14d&
                //sig=&
                //access_token=350685531728|62f8ce9f74b12f84c123cc23437a4a32
                
                    {"meta_inf_fbmeta",""},
                    {"advertiser_id", _deviceInfo.AdId.ToString()},
                    {"encrypted_msisdn",""},
                    {"currently_logged_in_userid","0"},
                    {"locale", AppLocale},
                    {"client_country_code", ClientCountryCode},

                    {"fb_api_req_friendly_name","authenticate"},
                    {"fb_api_caller_class","Fb4aAuthHandler"},
                    {"api_key", FacebookApiConstants.FACEBOOK_API_KEY},
                    {"sig", CryptoHelper.CalculateMd5()},
                    {"access_token", FacebookApiConstants.FACEBOOK_ACCESS_TOKEN},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data, true);
                request.Headers.AddHeader("Authorization", "OAuth null");
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "authenticate", true);

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var failureResponse = JsonConvert.DeserializeObject<FacebookFailureLoginResponse>(json);
                    var error = failureResponse.Error;
                    if (error != null)
                    {
                        if (error.Code == 401) // Wrong Credentials
                        {
                            //"code": 401,
                            //"message": "Invalid username or password",
                            //"error_user_title": "Wrong Credentials",
                            //"error_user_msg": "Invalid username or password",
                            return Result.Fail(error.Message, FacebookLoginResult.WrongUserOrPassword);
                        }
                        else if (error.Code == 418)
                        {
                            //"message": "An unexpected error occurred. Please try logging in again.",
                            //"code": 418,
                            //"error_user_title": "Login Error",
                            //"error_subcode": 2779001,
                            //"error_user_title": "Login Error",
                            //"error_user_msg": "An unexpected error occurred. Please try logging in again.",
                            if (error.ErrorData?.PwdEncKeyPkg != null && !secondTime)
                            {
                                User.PublicKey = error.ErrorData.PwdEncKeyPkg.PublicKey.Base64Encode();
                                User.PublicKeyId = error.ErrorData.PwdEncKeyPkg.KeyId.ToString();
                                return await LoginAsync(true);
                            }

                            return Result.Fail(error.Message, FacebookLoginResult.RenewPwdEncKeyPkg);
                        }
                    }

                    return Result.UnExpectedResponse<FacebookLoginResult>(response, json);

                }

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookLoginResult));

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookLoginResult>(response, json);
                var obj = JsonConvert.DeserializeObject<FacebookLoginSessionResponse>(json);

                InvalidateSuccessLogin(obj);
                return Result.Success(FacebookLoginResult.Success);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookLoginResult), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookLoginResult>(exception, responseContent);
            }
        }

        public async Task<IResult<bool>> SendAfterLoginFlowsAsync()
        {
            try
            {
                await GetNetworkInterfaceAsync();
                await GetPersistentComponentsAsync();
                await GetLoginDataBatchAsync();
                await GetLoginApprovalsKeysAsync();

                return Result.Success(true);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }
        public async Task<IResult<object>> GetFeedsAsync()
        {
            try
            {
                var instaUri = UriCreator.GetGraphQLUri();
                //format=json&
                //device_locale=en_US&
                //sim_country=ir&
                //network_country=ir&
                //device_id=c2846f25-54b3-434a-be1c-72e9a71573a3&
                //locale=en_US&
                //client_country_code=IR&
                //fb_api_req_friendly_name=suggestedLanguages&
                //fb_api_caller_class=LanguageSwitcher&
                //api_key=882a8490361da98702bf97a021ddc14d&
                //sig=b9b035a28341257e2874e199ffd4fba9&
                //access_token=350685531728|62f8ce9f74b12f84c123cc23437a4a32
                var data = new Dictionary<string, string>
                {
                    {"format", "json"},
                    {"device_locale", DeviceLocale.Replace("-","_")},
                    {"sim_country", SimCountry},
                    {"network_country", NetworkCountry},
                    {"device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"locale", AppLocale.Replace("-","_")},
                    {"client_country_code", ClientCountryCode},
                    {"fb_api_req_friendly_name","suggestedLanguages"},
                    {"fb_api_caller_class","LanguageSwitcher"},
                    {"api_key", FacebookApiConstants.FACEBOOK_API_KEY},
                    {"sig", CryptoHelper.CalculateMd5()},
                    {"access_token", FacebookApiConstants.FACEBOOK_ACCESS_TOKEN},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, _deviceInfo, data);

                //X-FB-Privacy-Context: c0000000fce04b3e
                //X-FB-Friendly-Name: fresh_feed_new_data_fetch
                //X-FB-Request-Analytics-Tags: graphservice
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_PRIVACY_CONTEXT, "c0000000" + 8.GetGuid(), true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "fresh_feed_new_data_fetch", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_REQUEST_ANALYTICS_TAGS, "graphservice", true);
                
                
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookLocaleLanguages>(response, json);

                var obj = JsonConvert.DeserializeObject<object>(json);

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(object), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<object>(exception);
            }
        }


        public async Task<IResult<FacebookNetworkInterfaceResponse>> GetNetworkInterfaceAsync()
        {
            string responseContent = null;

            try
            {
                var instaUri = UriCreator.GetNetworkInterfaceUri();
     
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post, instaUri, _deviceInfo);

                //X-FB-Privacy-Context: c0000000fce04b3e
                //X-FB-Friendly-Name: fresh_feed_new_data_fetch
                //X-FB-Request-Analytics-Tags: graphservice
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_PRIVACY_CONTEXT, "c0000000" + 8.GetGuid(), true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "rmd-mapfetcher", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_REQUEST_ANALYTICS_TAGS, "rmd", true);


                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookNetworkInterfaceResponse>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookNetworkInterfaceResponse));

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookNetworkInterfaceResponse>(response, json);

                var obj = JsonConvert.DeserializeObject<FacebookNetworkInterfaceResponse>(json);

                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookNetworkInterfaceResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookNetworkInterfaceResponse>(exception, responseContent);
            }
        }


        public async Task<IResult<bool>> GetPersistentComponentsAsync()
        {
            try
            {
                var instaUri = UriCreator.GetPersistentComponentsUri(AppLocale, ClientCountryCode);
                //{
                //  "nux_ids": [ "7301", "2504", "4280", "8403", "8720", "4545", "8148", "1630", "6849", "3931", "1907", "8057", "8169" ],
                //  "device_id": "c2846f25-54b3-434a-be1c-",
                //  "nt_context": {
                //    "styles_id": "d82700195bed991a354f509243d7d19e",
                //    "pixel_ratio": 3.0,
                //    "bloks_version": "f51c9d91e91f6e9c27f6f24b876c17457bb277986c33ecd69f5544a9cb39cc85",
                //    "using_white_navbar": true
                //  },
                //  "avatar_nux_image_width": 1080,
                //}
                var variablesObject = new JObject
                {
                    {"nux_ids", new JArray("7301", "2504", "4280", "8403", "8720", "4545", "8148", "1630", "6849", "3931", "1907", "8057", "8169") },
                    {"device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"nt_context", new JObject
                        {
                            {"styles_id", FacebookApiConstants.FACEBOOK_STYLES_ID},
                            {"pixel_ratio", 3.0},
                            {"bloks_version", FacebookApiConstants.FACEBOOK_BLOKS_VERSION},
                            {"using_white_navbar", true},
                        }
                    },
                    {"avatar_nux_image_width", 1080},
                    {"is_from_internal_tool", false},
                    {"family_device_id", _deviceInfo.FamilyDeviceGuid.ToString()},
                    {"should_query_for_animation", true},
                    {"locale", AppLocale.Replace("-","_")},
                    {"scale", "3"},
                };
                var variables = new StringBuilder();
                variables.Append("variables=");
                variables.Append(variablesObject.ToString(Formatting.None));
                //&method=post&
                //client_doc_id=9596065706713671445955878567&
                //query_name=FetchInterstitials&
                //strip_defaults=true&
                //strip_nulls=true&locale=en_US&client_country_code=IR&
                //fb_api_req_friendly_name=FetchInterstitials
                variables.Append("&method=post");
                variables.Append("&client_doc_id=9596065706713671445955878567");
                variables.Append("&query_name=FetchInterstitials");
                variables.Append("&strip_defaults=true");
                variables.Append("&strip_nulls=true");
                variables.Append("&locale=" + AppLocale);
                variables.Append("&client_country_code="+ClientCountryCode);
                variables.Append("&fb_api_req_friendly_name=FetchInterstitials");
                //[
                //  {
                //    "method": "POST",
                //    "body": "variables=iendly_name=FetchInterstitials",
                //    "name": "fetch_interstititals_graphql",
                //    "omit_response_on_success": false,
                //    "relative_url": "graphql"
                //  }
                //]

                var batch = new JObject
                {
                    {"method", "POST"},
                    {"body", variables.ToString()},
                    {"name", "fetch_interstititals_graphql"},
                    {"omit_response_on_success", false},
                    {"relative_url", "graphql"},

                };


                var data = new Dictionary<string, string>
                {
                    {"batch", new JArray(batch).ToString()},
                    {"fb_api_req_friendly_name","fetchPersistentComponents"},
                    {"fb_api_caller_class","Fb4aAuthHandler"},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data, true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "fetchPersistentComponents", true);

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<bool>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(bool));


                return Result.Success(true);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }




        public async Task<IResult<bool>> GetLoginDataBatchAsync()
        {
            try
            {
                var instaUri = UriCreator.GetPersistentComponentsUri(AppLocale, ClientCountryCode);
                var batch = new JArray(
                    new JObject
                    {
    //"method": "POST",
    //"body": "method=post&client_doc_id=362709109117887025531783922711&query_name=FetchAudienceInfoForLogin&strip_defaults=true&strip_nulls=true&locale=en_US&client_country_code=IR&fb_api_req_friendly_name=FetchAudienceInfoForLogin",
    //"name": "fetchViewerLoginAudienceInfo",
    //"omit_response_on_success": false,
    //"relative_url": "graphql"
                        {"method", "POST"},
                        {"body", $"method=post&client_doc_id=362709109117887025531783922711&query_name=FetchAudienceInfoForLogin&strip_defaults=true&strip_nulls=true&locale={AppLocale}&client_country_code={ClientCountryCode}&fb_api_req_friendly_name=FetchAudienceInfoForLogin"},
                        {"name", "fetchViewerLoginAudienceInfo"},
                        {"omit_response_on_success", false},
                        {"relative_url", "graphql"},
                    },
                    new JObject
                    {
    //"method": "POST",
    //"body": "method=post&client_doc_id=19779199095859283665209704276&query_name=ComposerPrivacyOptionsQuery&strip_defaults=true&strip_nulls=true&locale=en_US&client_country_code=IR&fb_api_req_friendly_name=ComposerPrivacyOptionsQuery",
    //"name": "fetchComposerPrivacyOptions",
    //"omit_response_on_success": false,
    //"relative_url": "graphql"
                        {"method", "POST"},
                        {"body", $"method=post&client_doc_id=19779199095859283665209704276&query_name=ComposerPrivacyOptionsQuery&strip_defaults=true&strip_nulls=true&locale={AppLocale}&client_country_code={ClientCountryCode}&fb_api_req_friendly_name=ComposerPrivacyOptionsQuery"},
                        {"name", "fetchComposerPrivacyOptions"},
                        {"omit_response_on_success", false},
                        {"relative_url", "graphql"},
                    },
                    new JObject
                    {
    //"method": "POST",
    //"body": "carrier_mcc=&carrier_mnc=11&sim_mcc=432&sim_mnc=35&format=json&interface=wifi&dialtone_enabled=false&needs_backup_rules=true&token_hash=&request_reason=login&locale=en_US&client_country_code=IR&fb_api_req_friendly_name=fetchZeroToken",
    //"name": "fetchZeroToken",
    //"omit_response_on_success": false,
    //"relative_url": "mobile_zero_campaign"
                        {"method", "POST"},
                        {"body", $"carrier_mcc=&carrier_mnc=&sim_mcc=&sim_mnc=&format=json&interface=wifi&dialtone_enabled=false&needs_backup_rules=true&token_hash=&request_reason=login&locale={AppLocale}&client_country_code={ClientCountryCode}&fb_api_req_friendly_name=fetchZeroToken"},
                        {"name", "fetchZeroToken"},
                        {"omit_response_on_success", false},
                        {"relative_url", "mobile_zero_campaign"},
                    }, 
                    new JObject
                    {
    //"method": "POST",
    //"body": "carrier_mcc=432&carrier_mnc=11&sim_mcc=432&sim_mnc=35&format=json&interface=wifi&dialtone_enabled=true&needs_backup_rules=true&token_hash=&request_reason=login&locale=en_US&client_country_code=IR&fb_api_req_friendly_name=fetchZeroToken",
    //"name": "fetchZeroTokenForDialtone",
    //"omit_response_on_success": false,
    //"relative_url": "mobile_zero_campaign"
                        {"method", "POST"},
                        {"body", $"carrier_mcc=432&carrier_mnc=11&sim_mcc=432&sim_mnc=35&format=json&interface=wifi&dialtone_enabled=true&needs_backup_rules=true&token_hash=&request_reason=login&locale={AppLocale}&client_country_code={ClientCountryCode}&fb_api_req_friendly_name=fetchZeroToken"},
                        {"name", "fetchZeroTokenForDialtone"},
                        {"omit_response_on_success", false},
                        {"relative_url", "mobile_zero_campaign"},
                    }
                    );
                var data = new Dictionary<string, string>
                {
                    {"batch", new JArray(batch).ToString()},
                    {"fb_api_req_friendly_name","fetchLoginData-batch"},
                    {"fb_api_caller_class","Fb4aAuthHandler"},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data, true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "fetchLoginData-batch", true);

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<bool>(response, json);
                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(bool));

                return Result.Success(true);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }


        public async Task<IResult<FacebookLoginApprovalsKeyResponse>> GetLoginApprovalsKeysAsync()
        {
            string responseContent = null;

            //UserAuthValidator.Validate(_userAuthValidate);
            try
            {
                var instaUri = UriCreator.GetLoginApprovalsKeysUri();
                //machine_id=8uccYvEu&
                //format=json&
                //locale=en_US&
                //client_country_code=IR&
                //fb_api_req_friendly_name=graphUserLoginApprovalsKeysPost&
                //fb_api_caller_class=CodeGeneratorOperationHandler
                var data = new Dictionary<string, string>
                {
                    {"machine_id", User.MachineId},
                    {"format", "json"},
                    {"locale", AppLocale},
                    {"client_country_code", ClientCountryCode},
                    {"fb_api_req_friendly_name", "graphUserLoginApprovalsKeysPost"},
                    {"fb_api_caller_class", "CodeGeneratorOperationHandler"},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "graphUserLoginApprovalsKeysPost", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_RMD, "cached=0;state=NO_MATCH", true);

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookLoginApprovalsKeyResponse>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookLoginApprovalsKeyResponse));

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookLoginApprovalsKeyResponse>(response, json);
                return Result.Success(JsonConvert.DeserializeObject<FacebookLoginApprovalsKeyResponse>(json));
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookLoginApprovalsKeyResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookLoginApprovalsKeyResponse>(exception, responseContent);
            }
        }


        public async Task<IResult<FacebookPwdEncKeyPkgResponse>> GetPasswordEncrytionKeyAsync()
        {
            string responseContent = null;
            try
            {
                var instaUri = UriCreator.GetPasswordEncrytionKeyUri();
                //device_id=-54b3-434a-be1c-&
                //version=2&
                //flow=CONTROLLER_INITIALIZATION&
                //locale=en_US&
                //client_country_code=IR&
                //method=GET&
                //fb_api_req_friendly_name=pwdKeyFetch&
                //fb_api_caller_class=Fb4aAuthHandler&
                //access_token=350685531728|62f8ce9f74b12f84c123cc23437a4a32
                var data = new Dictionary<string, string>
                {
                    {"device_id", _deviceInfo.DeviceGuid.ToString()},
                    {"version", "2"},
                    {"flow", "CONTROLLER_INITIALIZATION"},
                    {"locale", AppLocale},
                    {"client_country_code", ClientCountryCode},
                    {"method", "GET"},
                    {"fb_api_req_friendly_name", "pwdKeyFetch"},
                    {"fb_api_caller_class", "Fb4aAuthHandler"},
                    {"access_token", FacebookApiConstants.FACEBOOK_ACCESS_TOKEN},
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, "pwdKeyFetch", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_RMD, "cached=0;state=NO_MATCH", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_PRIORITY, "u=3, i");
                //Priority: u=3, i
                //X-ZERO-EH: 2,,AQmONaetXcMEBmu37Smc38pFC2P6PN6MlKY7q8dsJAIJklZuGH7ifzw7m8spNVAnSPg
                //x-fb-session-id: nid=EBuB4v7W7LTa;tid=53;nc=1;fc=0;bc=0;cid=ce2e4860c607a84912133181388452b6
                //X-FB-Friendly-Name: pwdKeyFetch
                //X-FB-Request-Analytics-Tags: unknown
                //x-fb-connection-token: ce2e4860c607a84912133181388452b6

                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<FacebookPwdEncKeyPkgResponse>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(FacebookPwdEncKeyPkgResponse));

                responseContent = json;
                if (json.Contains("\"errors\"") || json.Contains("\"error\""))
                    return Result.UnExpectedResponse<FacebookPwdEncKeyPkgResponse>(response, json);
                var obj = JsonConvert.DeserializeObject<FacebookPwdEncKeyPkgResponse>(json);
                if(!string.IsNullOrEmpty(obj.PublicKey))
                {
                    User.PublicKey = obj.PublicKey?.Base64Encode();
                    User.PublicKeyId = obj.KeyId.ToString();
                }
                return Result.Success(obj);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(FacebookPwdEncKeyPkgResponse), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<FacebookPwdEncKeyPkgResponse>(exception, responseContent);
            }
        }

        internal void InvalidateSuccessLogin(FacebookLoginSessionResponse response)
        {
            if (response == null) return;

            var convertedLoggedIn = ObjectsConverter.Instance.GetUserShortConverter(response).Convert();
            User.LoggedInUser = convertedLoggedIn;
            if (response.SessionCookies?.Length > 0 && User.LoggedInUser != null)
            {
                try
                {
                    foreach (var cookie in response.SessionCookies)
                    {
                        var newCookie = new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain)
                        {
                            Secure = cookie.Secure,
                            Expires = DateTime.Parse(cookie.Expires),
                            HttpOnly = cookie.HttpOnly,
                        };
                        User.LoggedInUser.RawCookies.Add(newCookie);
                        //_httpRequestProcessor
                        //    .HttpHandler
                        //    .CookieContainer
                        //    .Add(new Uri(FacebookApiConstants.FACEBOOK_WITHOUT_URL), newCookie);
                    }
                }
                catch { }

                var machineIdCookie = response
                    .SessionCookies
                    .FirstOrDefault(x => x.Name?.ToLower().Equals("datr") ?? false);

                User.MachineId = machineIdCookie?.Value;
            }
            IsUserAuthenticated = true;
            InvalidateProcessors();
            SessionHandler?.Save();
        }

        public async Task<IResult<T>> SendRequestAsync<T, TT>(string clientDocId,
            string method,
            string friendlyName,
            string analyticsTags,
            string variables,
            IObjectConverter<T, TT> converter = null,
            Uri customUri = null,
            Dictionary<string, string> otherParams = null,
            Dictionary<string, string> headers = null,
            bool appendPrivacyContextHeader = false,
            bool appendConnectionToken = false,
            string callerClass = "graphservice",
            bool userAgentGenTwo = false,
            bool removeOAuth = false)
        {
            string responseContent = null;
            try
            {
                var instaUri = customUri ?? UriCreator.GetGraphQLUri();

                //client_doc_id=157070121311310993102490765089&
                //method=post&
                //locale=en_US&
                //pretty=false&
                //format=json&variables={"page_size":7,"tile_size":220}&
                //fb_api_req_friendly_name=FriendsFacepilesQuery&
                //fb_api_caller_class=graphservice&
                //fb_api_analytics_tags=["GraphServices"]&
                //server_timestamps=true
                var data = new Dictionary<string, string>
                {
                    {"client_doc_id", clientDocId},
                    {"method", method},
                    {"locale", AppLocale},
                    {"pretty", "false"},
                    {"format", "json"},
                    {"variables", variables},
                    {"fb_api_req_friendly_name", friendlyName},
                    {"fb_api_caller_class", callerClass},
                    {"fb_api_analytics_tags", analyticsTags},
                    {"server_timestamps", "true"},
                };
                if (otherParams?.Count > 0)
                {
                    //foreach (var item in otherParams)
                    //    data.Add(item.Key, item.Value);
                    foreach (var item in data)
                        otherParams.Add(item.Key, item.Value);
                    data = otherParams;
                }
                foreach(var item in data.ToDictionary(n=> n.Key, v =>v.Value))
                {
                    if (item.Value == "%DELETE%")
                    {
                        data.Remove(item.Key);
                    }
                }
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    instaUri, _deviceInfo, data, userAgentGenTwo);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_FRIENDLY_NAME, friendlyName, true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_REQUEST_ANALYTICS_TAGS, callerClass, true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_PRIORITY, "u=3, i");
                request.Headers.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));
                if (headers?.Count > 0)
                {
                    try
                    {
                        foreach (var item in headers)
                            request.Headers.AddHeader(item.Key, item.Value, true);
                    }
                    catch { }
                }
                if (removeOAuth)
                    request.Headers.Remove(FacebookApiConstants.HEADER_AUTHORIZATION);
                if (appendPrivacyContextHeader)
                {
                    request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_PRIVACY_CONTEXT, "c0000000" + 8.GetGuid(), true);
                }
                if (appendConnectionToken)
                {
                    request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_CONNECTION_TOKEN, ExtensionsHelper.GetNewGuid(), true);
                }
                //x-fb-session-id: nid=qax0nfzFxoO1;tid=1;nc=0;fc=0;bc=0;cid=30efd71da9f4bf4c77e0021fba565618
    //            request.Headers.AddHeader("x-fb-session-id", $"nid=qax0nfzFxoO1;" +
    //$"tid=1;nc=0;fc=0;bc=0;cid=" + ExtensionsHelper.GetNewGuid(), true);
                var response = await _httpRequestProcessor.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<T>(response, responseText);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(T));

                responseContent = responseText;
                if (responseText.Contains("\"errors\"") || responseText.Contains("\"error\""))
                    return Result.UnExpectedResponse<T>(response, responseText);

                if (string.IsNullOrEmpty(responseText) || responseText?.Length == 0)
                    return Result.Fail<T>("It seems your account is blocked by facebook", ResponseType.AccountBlocked, default);

                var splitter = responseText.Split('\n');
                if (splitter.Length > 1)
                    responseText = splitter[0];
                if (converter != null)
                {
                    var obj = JsonConvert.DeserializeObject<TT>(responseText);
                    converter.SourceObject = obj;
                    return Result.Success(converter.Convert());
                }
                else
                {
                    var ttype = typeof(T);
                    if (ttype != typeof(bool))
                        return Result.Success(JsonConvert.DeserializeObject<T>(responseText));
                    else
                        return Result.Success(default(T));
                }
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(T), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<T>(exception, responseContent);
            }
        }


        public async Task<IResult<bool>> OneAsync()
        {
            try
            {
                var boundary = Guid.NewGuid().ToString().Replace("-", "");
                //{
                //  "database": "1",
                //  "epoch_id": 3903408941195007624,
                //  "format": "flatbuffer",
                //  "sync_params": "{\"full_height\":2111,\"full_width\":1080,\"gif_mime_type\":null,\"locale\":\"en_US\",\"preview_height\":1055,\"preview_width\":540,\"scale\":2.75,\"sticker_height\":633,\"sticker_width\":324}",
                //  "version": "7157075301031669"
                //}
                //{
                //  "database": "8",
                //  "epoch_id": 4898880737337363469,
                //  "format": "flatbuffer",
                //  "last_applied_cursor": "FsbngqMMFgAcLBgAGAAAAAA",
                //  "version": "7157075301031669"
                //}

                var jOb = new JObject
                {
                    {"database", "8"},
                    {"epoch_id", 4898880737337363469D},
                    {"format", "flatbuffer"},
                    //{"sync_params", "{\"full_height\":2111,\"full_width\":1080,\"gif_mime_type\":null,\"locale\":\"en_US\",\"preview_height\":1055,\"preview_width\":540,\"scale\":2.75,\"sticker_height\":633,\"sticker_width\":324}"},
                    {"version", "7157075301031669"},
                };


                var requestContent = new MultipartFormDataContent(boundary)
                {
                    //{new StringContent(jOb.ToString(Formatting.None)), "request_payload"},
                    //{new StringContent("2"), "request_type"},
                };
                var a = new StringContent(jOb.ToString(Formatting.None));
                //var a = new StringContent(/*jOb.ToString(Formatting.None)*/"{\"database\":\"1\",\"epoch_id\":3903408941195007624,\"format\":\"flatbuffer\",\"sync_params\":\"{\"full_height\":2111,\"full_width\":1080,\"gif_mime_type\":null,\"locale\":\"en_US\",\"preview_height\":1055,\"preview_width\":540,\"scale\":2.75,\"sticker_height\":633,\"sticker_width\":324}\",\"version\":\"7157075301031669\"}");
                a.Headers.Remove("Content-Type");
                var b = new StringContent("2");
                b.Headers.Remove("Content-Type");
                requestContent.Add(a, "\"request_payload\"");
                requestContent.Add(b, "\"request_type\"");
                var request = /*GetDefaultRequest(HttpMethod.Post)*/ _httpHelper.GetDefaultRequest(HttpMethod.Post,
                    new Uri("https://web.facebook.com/messaging/lightspeed/request"), _deviceInfo);
                request.Headers.AddHeader("family_device_id", _deviceInfo.PhoneGuid.ToString(), true);
                request.Headers.AddHeader("device_id", _deviceInfo.DeviceGuid.ToString(), true);
                request.Headers.AddHeader("request_token", Guid.NewGuid().ToString(), true);
                request.Headers.AddHeader("Priority", "u=3, i", true);
                //request.Headers.AddHeader("user_id", "100007855531564"/*User.LoggedInUser.UId.ToString()*/, true);
                request.Headers.AddHeader("X-FB-HTTP-Engine", "Liger", true);
                request.Headers.AddHeader("X-FB-Client-IP", "True", true);
                request.Headers.AddHeader("X-FB-Server-Cluster", "True", true);
                request.Headers.AddHeader("X-FB-Friendly-Name", "msysDataTask0", true);
                request.Headers.AddHeader("X-FB-Request-Analytics-Tags", "unknown", true);
                //X-FB-SIM-HNI: 43235
                //X-FB-Net-HNI: 43211
                request.Headers.AddHeader("X-FB-SIM-HNI", "43235", true);
                request.Headers.AddHeader("X-FB-Net-HNI", "43211", true);
                request.Headers.AddHeader(FacebookApiConstants.HEADER_FB_RMD, "cached=0;state=NO_MATCH", true);
                //request.Headers.AddHeader("X-ZERO-EH", "2,,"+ User.LoggedInUser.AnalyticsClaim.Replace("hmac.", ""), true);
                //requestContent.Headers.AddHeader("x-fb-connection-token", Guid.NewGuid().ToString().Replace("-",""), true);
                request.Headers.Remove("X-FB-Connection-Quality");
                request.Headers.Remove("X-FB-Connection-Type");
                request.Headers.AddHeader("X-MSGR-Region", "FRC", true);

                //x-fb-session-id: nid=dNB0r62O8F9m;tid=73;nc=1;fc=1;bc=0;cid=7e06ada14958b602edf892b7546f6d1a
                request.Headers.AddHeader("x-fb-session-id", $"nid={User.LoggedInUser.SessionKey.Split('.')[1]};" +
                    $"tid=73;nc=1;fc=1;bc=0;cid="+
                    Guid.NewGuid().ToString().Replace("-", ""), true);

                request.Content = requestContent;
                var response = await _httpRequestProcessor.SendAsync(request);
                var reader = (await response.Content.ReadAsByteArrayAsync());
                File.WriteAllBytes("abc.txt", reader);
                File.WriteAllText("abc base64.txt", CryptoHelper.Base64Encode (reader));

                var json = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != HttpStatusCode.OK)
                    return Result.UnExpectedResponse<bool>(response, json);

                var validateCheckpoint = ExtensionsHelper.CheckpointCheck(response.Headers);
                if (validateCheckpoint.Item1)
                    return Result.Fail(validateCheckpoint.Item2,
                        ResponseType.CheckpointAccount, default(bool));

                return Result.Success("true", default(bool));
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, default(bool), ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
                _logger?.LogException(exception);
                return Result.Fail<bool>(exception);
            }
        }
        internal void InvalidateProcessors()
        {
            MessagingProcessor = new MessagingProcessor(_deviceInfo,
                User, _httpRequestProcessor, 
                _logger, 
                this, _httpHelper);

            AccountProcessor = new AccountProcessor(_deviceInfo,
                User, _httpRequestProcessor,
                _logger, 
                this, _httpHelper);

            MediaProcessor = new MediaProcessor(_deviceInfo,
                User, _httpRequestProcessor,
                _logger, 
                this, _httpHelper);

            UserProcessor = new UserProcessor(_deviceInfo,
                User, _httpRequestProcessor,
                _logger, 
                this, _httpHelper);
        }
        #region State data

        public Stream GetStateDataAsStream() =>
            SerializationHelper.SerializeToStream(GetStateDataAsObject());

        public string GetStateDataAsString() =>
            SerializationHelper.SerializeToString(GetStateDataAsObject());

        public StateData GetStateDataAsObject()
        {
            var state = new StateData
            {
                DeviceInfo = _deviceInfo,
                IsAuthenticated = IsUserAuthenticated,
                UserSession = User,
                AcceptLanguage = AcceptLanguage,
                AppLocale = AppLocale,
                DeviceLocale = DeviceLocale,
                SimCountry = SimCountry,
                NetworkCountry = NetworkCountry,
                ClientCountryCode = ClientCountryCode,
            };

            return state;
        }

        public void LoadStateDataFromStream(Stream stream) =>
            LoadStateDataFromObject(SerializationHelper.DeserializeFromStream<StateData>(stream));

        public void LoadStateDataFromString(string json) =>
            LoadStateDataFromObject(SerializationHelper.DeserializeFromString<StateData>(json));

        public void LoadStateDataFromObject(StateData data)
        {
            if (data == null) throw new ArgumentNullException("data can't be null");

            _deviceInfo = data.DeviceInfo;
            User = data.UserSession;
            AppLocale = data.AppLocale;
            DeviceLocale = data.DeviceLocale;
            AcceptLanguage = data.AcceptLanguage;
            SimCountry = data.SimCountry;
            NetworkCountry = data.NetworkCountry;
            ClientCountryCode = data.ClientCountryCode;
            
            if (data.RawCookies?.Count > 0)
            {
                if (User.LoggedInUser.RawCookies == null)
                    User.LoggedInUser.RawCookies = new List<Cookie>();

                foreach (var cookie in data.RawCookies)
                {
                    User.LoggedInUser.RawCookies.Add(cookie);
                }
            }

            _httpHelper = new HttpHelper(_httpRequestProcessor, this);

            IsUserAuthenticated = data.IsAuthenticated;
            InvalidateProcessors();
        }

        #endregion State data
    }
}

/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes;
using FacebookApiSharp.Classes.DeviceInfo;
using FacebookApiSharp.Classes.SessionHandlers;
using FacebookApiSharp.Logger;
using System;
using System.Net;
using System.Net.Http;

namespace FacebookApiSharp.API.Builder
{
    public class FacebookApiBuilder : IFacebookApiBuilder
    {
        private IRequestDelay _delay = RequestDelay.Empty();
        private AndroidDevice _device;
        private HttpClient _httpClient;
        private HttpClientHandler _httpHandler = new HttpClientHandler()
        {
            UseProxy = false,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        };
        private IHttpRequestProcessor _httpRequestProcessor;
        private IFacebookLogger _logger;
        private ISessionHandler _sessionHandler;
        private UserSessionData _user;
        private string _language = "en-US";
        private FacebookApiBuilder() { }

        public IFacebookApi Build()
        {
            if (_user == null)
                _user = UserSessionData.Empty;

            if (_httpHandler == null) _httpHandler = new HttpClientHandler
            {
                UseProxy = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            if (_httpClient == null)
                _httpClient = new HttpClient(_httpHandler) { BaseAddress = new Uri(FacebookApiConstants.FACEBOOK_URL) };

            if (_device == null)
                _device = AndroidDeviceGenerator.GetRandomAndroidDevice();

            if (_httpRequestProcessor == null)
                _httpRequestProcessor =
                    new HttpRequestProcessor(_delay,
                    _httpClient,
                    _httpHandler, _logger);

            var facebookApi = new FacebookApi(_user, _logger, _device, _httpRequestProcessor);
            if (!string.IsNullOrEmpty(_language))
            {
                // no need to add StartupCountry
                facebookApi.AppLocale = facebookApi.DeviceLocale = _language.Replace("-", "_");
                facebookApi.AcceptLanguage = _language;
            }
            if (_sessionHandler != null)
            {
                _sessionHandler.FacebookApi = facebookApi;
                facebookApi.SessionHandler = _sessionHandler;
            }
            return facebookApi;
        }

        public IFacebookApiBuilder UseLogger(IFacebookLogger logger)
        {
            _logger = logger;
            return this;
        }

        public IFacebookApiBuilder UseHttpClient(HttpClient httpClient)
        {
            if (httpClient != null)
                httpClient.BaseAddress = new Uri(FacebookApiConstants.FACEBOOK_URL);

            _httpClient = httpClient;
            return this;
        }

        public IFacebookApiBuilder UseHttpClientHandler(HttpClientHandler handler)
        {
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            _httpHandler = handler;
            return this;
        }

        public IFacebookApiBuilder SetUser(UserSessionData user)
        {
            _user = user;
            return this;
        }

        public IFacebookApiBuilder SetRequestDelay(IRequestDelay delay)
        {
            if (delay == null)
                delay = RequestDelay.Empty();
            _delay = delay;
            return this;
        }

        public IFacebookApiBuilder SetDevice(AndroidDevice androidDevice)
        {
            _device = androidDevice;
            return this;
        }

        public IFacebookApiBuilder SetSessionHandler(ISessionHandler sessionHandler)
        {
            _sessionHandler = sessionHandler;
            return this;
        }

        public static IFacebookApiBuilder CreateBuilder()
        {
            return new FacebookApiBuilder();
        }
    }
}

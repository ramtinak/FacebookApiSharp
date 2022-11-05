/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.API;
using FacebookApiSharp.Helpers;
using FacebookApiSharp.Logger;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacebookApiSharp.Classes
{
    internal class HttpRequestProcessor : IHttpRequestProcessor
    {
        private IRequestDelay _delay;
        private IFacebookLogger _logger;
        public IRequestDelay Delay { get { return _delay; } set { _delay = value; } }
        public HttpRequestProcessor(IRequestDelay delay, HttpClient httpClient, HttpClientHandler httpHandler,
           IFacebookLogger logger)
        {
            _delay = delay;
            Client = httpClient;
            HttpHandler = httpHandler;
            _logger = logger;
        }

        public HttpClientHandler HttpHandler { get; set; }
        public HttpClient Client { get; set; }
        public void SetHttpClientHandler(HttpClientHandler handler)
        {
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            HttpHandler = handler;
            Client = new HttpClient(handler) { BaseAddress = new Uri(FacebookApiConstants.FACEBOOK_URL) };
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, bool keepAlive = false)
        {
            await AppendOtherHeaders(requestMessage, keepAlive);
            LogHttpRequest(requestMessage);
            if (_delay.Exist)
                await Task.Delay(_delay.Value);
            var response = await Client.SendAsync(requestMessage);
            LogHttpResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> GetAsync(Uri requestUri, bool keepAlive = false)
        {
            await AppendOtherHeaders(null, keepAlive);
            _logger?.LogRequest(requestUri);
            if (_delay.Exist)
                await Task.Delay(_delay.Value);
            var response = await Client.GetAsync(requestUri);
            LogHttpResponse(response);
            return response;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage,
            HttpCompletionOption completionOption, bool keepAlive = false)
        {
            await AppendOtherHeaders(requestMessage, keepAlive);
            LogHttpRequest(requestMessage);
            if (_delay.Exist)
                await Task.Delay(_delay.Value);
            var response = await Client.SendAsync(requestMessage, completionOption);
            LogHttpResponse(response);
            return response;
        }

        public async Task<string> SendAndGetJsonAsync(HttpRequestMessage requestMessage,
            HttpCompletionOption completionOption, bool keepAlive = false)
        {
            await AppendOtherHeaders(requestMessage, keepAlive);
            LogHttpRequest(requestMessage);
            if (_delay.Exist)
                await Task.Delay(_delay.Value);
            var response = await Client.SendAsync(requestMessage, completionOption);
            LogHttpResponse(response);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GeJsonAsync(Uri requestUri, bool keepAlive = false)
        {
            await AppendOtherHeaders(null, keepAlive);
            _logger?.LogRequest(requestUri);
            if (_delay.Exist)
                await Task.Delay(_delay.Value);
            var response = await Client.GetAsync(requestUri);
            LogHttpResponse(response);
            return await response.Content.ReadAsStringAsync();
        }

        private void LogHttpRequest(HttpRequestMessage request)
        {
            _logger?.LogRequest(request);
        }

        private void LogHttpResponse(HttpResponseMessage request)
        {
            _logger?.LogResponse(request);
        }

        async Task AppendOtherHeaders(HttpRequestMessage request, bool keepAlive = false)
        {
            var currentCulture = HttpHelper.GetCurrentCulture();
            System.Globalization.CultureInfo.CurrentCulture = HttpHelper.EnglishCulture;
            Client.DefaultRequestHeaders.ConnectionClose = keepAlive;
            if (request != null)
            {
                request.Headers.ConnectionClose = keepAlive;
                if (request.Content != null)
                {
                    if (request.Content.Headers.ContentType != null)
                        request.Content.Headers.ContentType.CharSet = "UTF-8";
                    if(!request.RequestUri.ToString().Contains("me/photos"))
                    request.Content.Headers.ContentLength = request.Content.ReadAsStringAsync().GetAwaiter().GetResult()?.Length;
                }
            }
            System.Globalization.CultureInfo.CurrentCulture = currentCulture;
            await Task.Delay(1).ConfigureAwait(false); // lets force compiler to wait for AppendOtherHeaders
        }

        static bool WasntIndexOf(string str1, string str2) => str1.IndexOf(str2, StringComparison.OrdinalIgnoreCase) == -1;

        #region IDisposable Implementations

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Client?.CancelPendingRequests();
                Client?.Dispose();
                HttpHandler?.Dispose();
            }

            _delay = null;
            _logger = null;
            HttpHandler = null;
            Client = null;
        }

        #endregion
    }
}
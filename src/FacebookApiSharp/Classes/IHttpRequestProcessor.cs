/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */


using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacebookApiSharp.Classes
{
    public interface IHttpRequestProcessor : IDisposable
    {
        HttpClientHandler HttpHandler { get; set; }
        HttpClient Client { get; }
        void SetHttpClientHandler(HttpClientHandler handler);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, bool keepAlive = false);
        Task<HttpResponseMessage> GetAsync(Uri requestUri, bool keepAlive = false);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption, bool keepAlive = false);
        Task<string> SendAndGetJsonAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption, bool keepAlive = false);
        Task<string> GeJsonAsync(Uri requestUri, bool keepAlive = false);
        IRequestDelay Delay { get; set; }
    }
}
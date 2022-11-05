/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FacebookApiSharp.Logger
{
    public class DebugLogger : IFacebookLogger
    {
        public string UserName { get; set; }
        public string LetsLog { get; set; }
        private readonly LogLevel _logLevel;

        public DebugLogger(LogLevel loglevel)
        {
            _logLevel = loglevel;
        }
        public DebugLogger(LogLevel loglevel, string userName = "noName", string logStuff = "true")
        {
            LetsLog = logStuff;
            UserName = userName;
            _logLevel = loglevel;
        }

        public async Task LogRequest(HttpRequestMessage request)
        {
            if (_logLevel < LogLevel.Request) return;
            WriteSeprator();
            Write($"Request: {request.Method} {request.RequestUri}");
            WriteHeaders(request.Headers);
#if NET
            WriteProperties(request.Options);
#else
            WriteProperties(request.Properties);
#endif
            if (request.Method == HttpMethod.Post)
                await WriteRequestContent(request.Content);
        }

        public void LogRequest(Uri uri)
        {
            if (_logLevel < LogLevel.Request) return;
            Write($"Request: {uri}");
        }

        public async Task LogResponse(HttpResponseMessage response)
        {
            if (_logLevel < LogLevel.Response) return;
            Write($"Response: {response.RequestMessage.Method} {response.RequestMessage.RequestUri} [{response.StatusCode}]");
            WriteHeaders(response.Headers);
            await WriteContent(response.Content, Formatting.None, 0);
        }

        public void LogException(Exception ex)
        {
            if (_logLevel < LogLevel.Exceptions) return;
#if NETSTANDARD || NET || NETCOREAPP3_1
            Console.WriteLine($"Exception: {ex}");
            Console.WriteLine($"Stacktrace: {ex.StackTrace}");
#else
            System.Diagnostics.Debug.WriteLine($"Exception: {ex}");
            System.Diagnostics.Debug.WriteLine($"Stacktrace: {ex.StackTrace}");
#endif
        }

        public void LogInfo(string info)
        {
            if (_logLevel < LogLevel.Info) return;
            Write($"Info:{Environment.NewLine}{info}");
        }

        private void WriteHeaders(HttpHeaders headers)
        {
            if (headers == null) return;
            if (!headers.Any()) return;
            Write("Headers:");
            foreach (var item in headers)
                Write($"{item.Key}:{JsonConvert.SerializeObject(item.Value)}");
        }

        private void WriteProperties(IDictionary<string, object> properties)
        {
            if (properties == null) return;
            if (properties.Count == 0) return;
            Write($"Properties:\n{JsonConvert.SerializeObject(properties, Formatting.Indented)}");
        }

        private async Task WriteContent(HttpContent content, Formatting formatting, int maxLength = 0)
        {
            Write("Content:");
            if (content.Headers.ContentType != null)
            content.Headers.ContentType.CharSet = "utf-8";
            var raw = await content.ReadAsStringAsync();
            if (formatting == Formatting.Indented) raw = FormatJson(raw);
            raw = raw.Contains("<!DOCTYPE html>") ? "got html content!" : raw;
            if ((raw.Length > maxLength) & (maxLength != 0))
                raw = raw.Substring(0, maxLength);
            Write(raw);
        }
        private async Task WriteRequestContent(HttpContent content, int maxLength = 0)
        {
            Write("Content:");
            if(content == null)
            {
                Write("Doesn't have any content");
                return;
            }
            var raw = await content.ReadAsStringAsync();
            if ((raw.Length > maxLength) & (maxLength != 0))
                raw = raw.Substring(0, maxLength);
            Write(WebUtility.UrlDecode(raw));
        }

        private void WriteSeprator()
        {
            var sep = new StringBuilder();
            for (var i = 0; i < 100; i++) sep.Append("-");
            Write(sep.ToString());
        }

        private string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        private void Write(string message)
        {
            if (LetsLog == "true")
            {
                Directory.CreateDirectory("debug");
                File.AppendAllText($@"debug\{UserName}.txt", $"{DateTime.Now.ToString()}:\t{message}" + Environment.NewLine);
            }
#if NETSTANDARD || NET || NETCOREAPP3_1
            Console.WriteLine($"{DateTime.Now}:\t{message}");
#else
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}:\t{message}");
#endif
        }
    }
}

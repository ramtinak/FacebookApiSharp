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
    public class FileDebugLogger : IFacebookLogger
    {
        private readonly string FilePath;
        private readonly LogLevel _logLevel;
        private readonly object _lock = new object();

        public FileDebugLogger(LogLevel loglevel, string fileName = null)
        {
            _logLevel = loglevel;

            if (string.IsNullOrEmpty(fileName))
                fileName = $"logs {DateTime.Now:MM-dd-yy H-mm-ss}.txt";

            FilePath = fileName;
        }
        public async Task LogRequest(HttpRequestMessage request)
        {
            if (_logLevel < LogLevel.Request) return;
            var sb = new StringBuilder();
            var headers = WriteHeaders(request.Headers);
            string properties;
            sb.AppendLine(WriteSeprator());
            sb.AppendLine(GetText($"Request: {request.Method} {request.RequestUri}"));
            if (!string.IsNullOrEmpty(headers))
                sb.Append(headers);
#if NET
            properties = WriteProperties(request.Options);
#else
            properties = WriteProperties(request.Properties);
#endif
            if (!string.IsNullOrEmpty(properties))
                sb.AppendLine(properties);
            if (request.Method == HttpMethod.Post)
                sb.AppendLine(await WriteRequestContent(request.Content));
            Write(sb.ToString().TrimStart());
        }

        public void LogRequest(Uri uri)
        {
            if (_logLevel < LogLevel.Request) return;
            Write(GetText($"Request: {uri}"));
        }

        public async Task LogResponse(HttpResponseMessage response)
        {
            if (_logLevel < LogLevel.Response) return;
            var sb = new StringBuilder();
            var headers = WriteHeaders(response.Headers);
            sb.AppendLine(GetText($"Response: {response.RequestMessage.Method} {response.RequestMessage.RequestUri} [{response.StatusCode}]"));
            if (!string.IsNullOrEmpty(headers))
                sb.AppendLine(headers);
            sb.Append(await WriteContent(response.Content, Formatting.None, 0));
            Write(sb.ToString());
        }

        public void LogException(Exception ex)
        {
            if (_logLevel < LogLevel.Exceptions) return;

            string text = $"Exception: {ex}" +
                Environment.NewLine +
                $"Stacktrace: {ex.StackTrace}";

            Write(GetText(text));
        }

        public void LogInfo(string info)
        {
            if (_logLevel < LogLevel.Info) return;
            Write(GetText($"Info:{Environment.NewLine}{info}"));
        }

        private string WriteHeaders(HttpHeaders headers)
        {
            if (headers == null) return null;
            if (!headers.Any()) return null;
            var sb = new StringBuilder();
            sb.AppendLine(GetText("Headers:"));
            foreach (var item in headers)
                sb.AppendLine(GetText($"{item.Key}:{JsonConvert.SerializeObject(item.Value)}"));

            return sb.ToString();
        }

        private string WriteProperties(IDictionary<string, object> properties)
        {
            if (properties == null) return null;
            if (properties.Count == 0) return null;
            return GetText($"Properties:\n{JsonConvert.SerializeObject(properties, Formatting.Indented)}");
        }

        private async Task<string> WriteContent(HttpContent content, Formatting formatting, int maxLength = 0)
        {
            var sb = new StringBuilder();
            sb.Append(GetText("Content:"));
            if (content == null)
            {
                sb.Append(GetText("Doesn't have any content"));
                return sb.ToString();
            }
            var raw = await content.ReadAsStringAsync();
            if (formatting == Formatting.Indented) raw = FormatJson(raw);
            raw = raw.Contains("<!DOCTYPE html>") ? "got html content!" : raw;
            if ((raw.Length > maxLength) & (maxLength != 0))
                raw = raw.Substring(0, maxLength);
            sb.AppendLine(GetText(raw));
            return sb.ToString();
        }

        private async Task<string> WriteRequestContent(HttpContent content, int maxLength = 0)
        {
            var sb = new StringBuilder();
            sb.Append(GetText("Content:"));
            if (content == null)
            {
                sb.Append(GetText("Doesn't have any content"));
                return sb.ToString();
            }
            var raw = await content.ReadAsStringAsync();
            if ((raw.Length > maxLength) & (maxLength != 0))
                raw = raw.Substring(0, maxLength);
            sb.AppendLine(GetText(WebUtility.UrlDecode(raw)));
            return sb.ToString();
        }

        private string WriteSeprator()
        {
            var sep = new StringBuilder();
            for (var i = 0; i < 100; i++) sep.Append("-");
            return GetText(sep.ToString());
        }

        private string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        private void Write(string message)
        {
            lock (_lock)
            {
                File.AppendAllText(FilePath, message);
            }
        }

        private string GetText(string message)
        {
            return $"{DateTime.Now}:\t{message}";
        }
    }
}

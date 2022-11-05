/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.API;
using System;
using System.Net.Http.Headers;

namespace FacebookApiSharp.Helpers
{
    public static class HttpExtensions
    {
        public static Uri AddQueryParameter(this Uri uri, string name, string value, bool dontCheck = false)
        {
            if (!dontCheck)
                if (value == null || value == "" || value == "[]") return uri;

            if (value == null)
                value = "";
            var httpValueCollection = HttpUtility.ParseQueryString(uri);

            httpValueCollection.Remove(name);
            httpValueCollection.Add(name, value);

            var ub = new UriBuilder(uri);
            var q = "";
            foreach (var item in httpValueCollection)
            {
                if (q == "") q += $"{item.Key}={item.Value}";
                else q += $"&{item.Key}={item.Value}";
            }
            ub.Query = q;
            return ub.Uri;
        }
        internal static void AddHeader(this HttpRequestHeaders headers,
            string name,
            string value,
            bool removeHeader = false)
        {
            var currentCulture = HttpHelper.GetCurrentCulture();
            System.Globalization.CultureInfo.CurrentCulture = HttpHelper.EnglishCulture;

            if (removeHeader)
                headers.Remove(name);

            headers.Add(name, value);

            System.Globalization.CultureInfo.CurrentCulture = currentCulture;
        }
        internal static void AddHeader(this HttpContentHeaders headers, string name,
            string value,
            bool removeHeader = false)
        {
            var currentCulture = HttpHelper.GetCurrentCulture();
            System.Globalization.CultureInfo.CurrentCulture = HttpHelper.EnglishCulture;

            if (removeHeader)
                headers.Remove(name);

            headers.Add(name, value);

            System.Globalization.CultureInfo.CurrentCulture = currentCulture;
        }
    }
}
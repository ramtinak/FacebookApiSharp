/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FacebookApiSharp.Logger
{
    public interface IFacebookLogger
    {
        Task LogRequest(HttpRequestMessage request);
        void LogRequest(Uri uri);
        Task LogResponse(HttpResponseMessage response);
        void LogException(Exception exception);
        void LogInfo(string info);
    }
}

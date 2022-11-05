/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes;
using FacebookApiSharp.Classes.DeviceInfo;
using FacebookApiSharp.Classes.SessionHandlers;
using FacebookApiSharp.Logger;
using System.Net.Http;

namespace FacebookApiSharp.API.Builder
{
    public interface IFacebookApiBuilder
    {
        IFacebookApi Build();

        IFacebookApiBuilder UseLogger(IFacebookLogger logger);

        IFacebookApiBuilder UseHttpClient(HttpClient httpClient);

        IFacebookApiBuilder UseHttpClientHandler(HttpClientHandler handler);

        IFacebookApiBuilder SetUser(UserSessionData user);

        IFacebookApiBuilder SetRequestDelay(IRequestDelay delay);

        IFacebookApiBuilder SetDevice(AndroidDevice androidDevice);

        IFacebookApiBuilder SetSessionHandler(ISessionHandler sessionHandler);

    }
}
/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes;
using FacebookApiSharp.Classes.DeviceInfo;
using FacebookApiSharp.Classes.Models;
using FacebookApiSharp.Classes.Responses;
using FacebookApiSharp.Classes.SessionHandlers;
using FacebookApiSharp.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using FacebookApiSharp.API.Processors;

namespace FacebookApiSharp.API
{
    public interface IFacebookApi
    {
        Task<IResult<bool>> SendLoginFlowsAsync();
        Task<IResult<FacebookLoginResult>> LoginAsync(bool secondTime = false);
        Task<IResult<bool>> SendAfterLoginFlowsAsync();
        Task<IResult<FacebookNetworkInterfaceResponse>> GetNetworkInterfaceAsync();
        Task<IResult<bool>> GetPersistentComponentsAsync();
        Task<IResult<bool>> GetLoginDataBatchAsync();
        Task<IResult<FacebookLoginApprovalsKeyResponse>> GetLoginApprovalsKeysAsync();
        Task<IResult<FacebookPwdEncKeyPkgResponse>> GetPasswordEncrytionKeyAsync();

        ISessionHandler SessionHandler { get; }
        IMessagingProcessor MessagingProcessor { get; }
        IAccountProcessor AccountProcessor { get; }
        IUserProcessor UserProcessor { get; }
        IMediaProcessor MediaProcessor { get; }
        string SimCountry { get; set; }
        string NetworkCountry { get; set; }
        string ClientCountryCode { get; set; }
        string AppLocale { get; set; }
        string DeviceLocale { get; set; }
        string AcceptLanguage { get; set; }
        bool IsUserAuthenticated { get; }
        UserSessionData GetLoggedUser();
        AndroidDevice GetCurrentDevice();
    }
}

/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.API;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookApiSharp.Helpers
{
    public class UriCreator
    {
        private static readonly Uri FacebookUri = new Uri(FacebookApiConstants.FACEBOOK_URL);
        private static readonly Uri BFacebookUri = new Uri(FacebookApiConstants.B_FACEBOOK_URL);


        public static Uri GetAppLocaleSuggestionsUri()
        {
            if (!Uri.TryCreate(FacebookUri,
                "/app/locale_suggestions", 
                out var instaUri))
                throw new Exception("Cant create URI GetAppLocaleSuggestionsUri");
            return instaUri;
        }
        public static Uri GetZeroHeadersPingParamsUri()
        {
            if (!Uri.TryCreate(BFacebookUri,
                "/zero_headers_ping_params_v2?fields=use_for_fos,use_for_login,clear,remove_keys,next_cursor,cooldown_on_success,cooldown_on_failure,uri,transparency_content,transparency_content_type,carrier_name,carrier_id,consent_required,transparency_design,client_header_params,headwind_program,headwind_storage,headwind_immediate,user_signal_required",
                out var instaUri))
                throw new Exception("Cant create URI GetZeroHeadersPingParamsUri");
            return instaUri;
        }
        public static Uri GetMobileGatekeepersUri()
        {
            if (!Uri.TryCreate(BFacebookUri,
                "/mobile_gatekeepers",
                out var instaUri))
                throw new Exception("Cant create URI GetMobileGateKeepersUri");
            return instaUri;
        }
        public static Uri GetLoggingClientEventsUri()
        {
            if (!Uri.TryCreate(FacebookUri,
                "/logging_client_events",
                out var instaUri))
                throw new Exception("Cant create URI GetLoggingClientEventsUri");
            return instaUri;
        }

        public static Uri GetAuthLoginUri()
        {
            if (!Uri.TryCreate(BFacebookUri,
                "/auth/login",
                out var instaUri))
                throw new Exception("Cant create URI GetAuthLoginUri");
            return instaUri;
        }

        public static Uri GetGraphQLUri()
        {
            if (!Uri.TryCreate(FacebookUri,
                "/graphql",
                out var instaUri))
                throw new Exception("Cant create URI GetGraphQLUri");
            return instaUri;
        }

        public static Uri GetNetworkInterfaceUri()
        {
            if (!Uri.TryCreate(FacebookUri,
                "/v3.2/cdn_rmd?net_iface=Wifi&reason=APP_START",
                out var instaUri))
                throw new Exception("Cant create URI GetNetworkInterfaceUri");
            return instaUri;
        }

        public static Uri GetPersistentComponentsUri(string locale, string clientCountryCode)
        {
            if (!Uri.TryCreate(BFacebookUri,
                $"/?include_headers=false&decode_body_json=false&streamable_json_response=true&locale={locale}&client_country_code={clientCountryCode}",
                out var instaUri))
                throw new Exception("Cant create URI GetPersistentComponentsUri");
            return instaUri;
        }

        public static Uri GetLoginApprovalsKeysUri()
        {
            if (!Uri.TryCreate(FacebookUri,
                "/100034137818552/loginapprovalskeys", // yeah its two slashes!
                out var instaUri))
                throw new Exception("Cant create URI GetLoginApprovalsKeysUri");
            return instaUri;
        }

        public static Uri GetPasswordEncrytionKeyUri()
        {
            if (!Uri.TryCreate(FacebookUri,
                "/pwd_key_fetch", // yeah its two slashes!
                out var instaUri))
                throw new Exception("Cant create URI GetPasswordEncrytionKeyUri");
            return instaUri;
        }
    }
}

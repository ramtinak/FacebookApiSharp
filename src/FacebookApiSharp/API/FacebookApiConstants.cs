/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json.Linq;
using System;

namespace FacebookApiSharp.API
{
    public static class FacebookApiConstants
    {
        public const string FACEBOOK_API_VERSION = "361.0.0.39.115";
        public const string FACEBOOK_APP_VERSION = "361571102";
        public const string FACEBOOK_ACCESS_TOKEN = "350685531728|62f8ce9f74b12f84c123cc23437a4a32";
        public const string FACEBOOK_API_KEY = "882a8490361da98702bf97a021ddc14d";
        public const string FACEBOOK_BLOKS_VERSION = "fcb188fb4c97a862b85d875824832dd3e42ead2a17102661b4bed0825f260514";
        public const string FACEBOOK_STYLES_ID = "800cb86569d1fc1615bd1576caae8172";


        //X-FB-Connection-Quality: EXCELLENT
        //X-FB-SIM-HNI: 43235
        //X-FB-Net-HNI: 43211
        //X-FB-Connection-Type: unknown
        //User-Agent: [FBAN/FB4A;FBAV/355.0.0.21.108;FBBV/352948159;FBDM/{density=2.75,width=1080,height=2111};FBLC/en_US;FBRV/0;FBCR/IR-MCI;FBMF/Xiaomi;FBBD/Redmi;FBPN/com.facebook.katana;FBDV/M2007J22G;FBSV/11;FBBK/1;FBOP/1;FBCA/arm64-v8a:;]
        //Host: graph.facebook.com
        //Content-Type: application/x-www-form-urlencoded
        //X-Tigon-Is-Retry: False
        //x-fb-device-group: 7864
        //X-FB-Friendly-Name: suggestedLanguages
        //X-FB-Request-Analytics-Tags: unknown
        //Accept-Encoding: gzip, deflate
        //X-FB-HTTP-Engine: Liger
        //X-FB-Client-IP: True
        //X-FB-Server-Cluster: True
        public const string HEADER_FB_CONNECTION_QUALITY = "X-FB-Connection-Quality"; // EXCELLENT
        public const string HEADER_FB_SIM_HNI = "X-FB-SIM-HNI";
        public const string HEADER_FB_NET_HNI = "X-FB-Net-HNI";
        public const string HEADER_FB_ZERO_STATE = "X-ZERO-STATE";
        public const string HEADER_FB_CONNECTION_TYPE = "X-FB-Connection-Type";
        public const string HEADER_FB_DEVICE_GROUP = "x-fb-device-group";
        public const string HEADER_FB_CONNECTION_TOKEN = "x-fb-connection-token";
        public const string HEADER_FB_RMD = "x-fb-rmd";
        public const string HEADER_FB_FRIENDLY_NAME = "X-FB-Friendly-Name";
        public const string HEADER_FB_REQUEST_ANALYTICS_TAGS = "X-FB-Request-Analytics-Tags";
        public const string HEADER_FB_PRIVACY_CONTEXT = "X-FB-Privacy-Context";
        public const string HEADER_FB_HTTP_ENGINE = "X-FB-HTTP-Engine";
        public const string HEADER_FB_CLIENT_IP = "X-FB-Client-IP";
        public const string HEADER_FB_SERVER_CLUSTER = "X-FB-Server-Cluster";
        public const string HEADER_X_IG_TIGON_RETRY = "X-Tigon-Is-Retry";
        public const string HEADER_USER_AGENT = "User-Agent";
        public const string HEADER_ACCEPT_ENCODING = "Accept-Encoding";
        public const string HEADER_ACCEPT_LANGUAGE = "Accept-Language";
        public const string HEADER_AUTHORIZATION = "Authorization";
        public const string HEADER_PRIORITY = "Priority";



        public const string USER_AGENT_DEFAULT = "[FBAN/FB4A;FBAV/355.0.0.21.108;FBBV/352948159;FBDM/{density=2.75,width=1080,height=2111};FBLC/en_US;FBRV/0;FBCR/IR-MCI;FBMF/Xiaomi;FBBD/Redmi;FBPN/com.facebook.katana;FBDV/M2007J22G;FBSV/11;FBBK/1;FBOP/1;FBCA/arm64-v8a:;]";

        public const string HOST = "Host";
        public const string HOST_URI = "graph.facebook.com";
        public const string ACCEPT_ENCODING2 = "gzip, deflate";

        public const string ACCEPT_ENCODING = "gzip, deflate, sdch";
        public const string FACEBOOK_URL = "https://graph.facebook.com";
        public const string FACEBOOK_WITHOUT_URL = "https://facebook.com";
        public const string B_FACEBOOK_URL = "https://b-graph.facebook.com";
        public static readonly Uri BaseFacebookUri = new Uri(FACEBOOK_URL);



        public static readonly JArray SupportedCapabalities = new JArray
        {
            new JObject
            {
                {"name","multiplane"},
                {"value","multiplane_disabled"}
            },
            new JObject
            {
                {"name","world_tracker"},
                {"value","world_tracker_enabled"}
            },
            new JObject
            {
                {"name","xray"},
                {"value","xray_disabled"}
            },
            new JObject
            {
                {"name","half_float_render_pass"},
                {"value","half_float_render_pass_enabled"}
            },
            new JObject
            {
                {"name","multiple_render_targets"},
                {"value","multiple_render_targets_enabled"}
            },
            new JObject
            {
                {"name","vertex_texture_fetch"},
                {"value","vertex_texture_fetch_enabled"}
            },
            new JObject
            {
                {"name","body_tracking"},
                {"value","body_tracking_disabled"}
            },
            new JObject
            {
                {"name","gyroscope"},
                {"value","gyroscope_enabled"}
            },
            new JObject
            {
                {"name","geoanchor"},
                {"value","geoanchor_disabled"}
            },
            new JObject
            {
                {"name","scene_depth"},
                {"value","scene_depth_disabled"}
            },
            new JObject
            {
                {"name","segmentation"},
                {"value","segmentation_enabled"}
            },
            new JObject
            {
                {"name","hand_tracking"},
                {"value","hand_tracking_enabled"}
            },
            new JObject
            {
                {"name","real_scale_estimation"},
                {"value","real_scale_estimation_disabled"}
            },
            new JObject
            {
                {"name","hair_segmentation"},
                {"value","hair_segmentation_disabled"}
            },
            new JObject
            {
                {"name","depth_shader_read"},
                {"value","depth_shader_read_enabled"}
            },
            new JObject
            {
                {"name","etc2_compression"},
                {"value","compression"}
            },
            new JObject
            {
                {"name","face_tracker_version"},
                {"value","0"}
            },
        };
    }
}

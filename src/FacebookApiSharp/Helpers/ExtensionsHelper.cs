/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.API;
using FacebookApiSharp.Classes.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using System.Net.Http.Headers;

namespace FacebookApiSharp.Helpers
{
    public static class ExtensionsHelper
    {
        readonly static Random Rnd = new Random();
        public static string GetNewGuid() =>
            Guid.NewGuid().ToString().Replace("-", "");
        public static string GetGuid(this int length) =>
            GetNewGuid().Substring(0, length);
        public static string GenerateUserAgent(this AndroidDevice deviceInfo, 
            IFacebookApi facebookApi,
            bool userAgentGenTwo = false,
            string language = null)
        {
            var lang = language ?? facebookApi.AppLocale ?? "en_US";
            var device = deviceInfo;
            var dpi = int.Parse(device.Dpi.Replace("dpi", ""));
            var res = device.Resolution.Split('x');
            var width = res[0];
            var height = res[1];
            //[FBAN/FB4A;FBAV/355.0.0.21.108;FBBV/352948159;FBDM/{density=2.75,width=1080,height=2111};FBLC/en_US;FBRV/0;FBCR/IR-MCI;FBMF/Xiaomi;FBBD/Redmi;FBPN/com.facebook.katana;FBDV/M2007J22G;FBSV/11;FBBK/1;FBOP/1;FBCA/arm64-v8a:;]

            var fields = new Dictionary<string, string>
            {
                {"FBAN", "FB4A"},
                {"FBAV", FacebookApiConstants.FACEBOOK_API_VERSION},
                {"FBBV", FacebookApiConstants.FACEBOOK_APP_VERSION},
                {"FBDM",
                    $"{{density={Math.Round(dpi/ 160f, 1):F1},width={width},height={height}}}"
                },
                {"FBLC", lang.Replace("-","_")}, // en_US
                {"FBRV", "0"},
                {"FBCR", ""},   // We don't have cellular // IR-MCI
                {"FBMF", device.HardwareManufacturer},
                {"FBBD", device.AndroidBoardName},
                {"FBPN", "com.facebook.katana"},
                {"FBDV", device.HardwareModel},
                {"FBSV", device.AndroidVer.VersionNumber},
                {"FBBK", "1"},
                {"FBOP", "1"},
                {"FBCA", AndroidDevice.CPU_ABI}
            };
            var mergeList = new List<string>();
            foreach (var field in fields)
            {
                mergeList.Add($"{field.Key}/{field.Value}");
            }

            var userAgent = "";
            foreach (var field in mergeList)
            {
                userAgent += field + ';';
            }
            //Dalvik/2.1.0 (Linux; U; Android 11; M2007J22G Build/RP1A.200720.011)
            var generated = '[' + userAgent + ']';
            if (userAgentGenTwo)
            {
                var ua = $"Dalvik/2.1.0 (Linux; U; {deviceInfo.AndroidVer.Codename}; " +
                    $"{device.HardwareModel} Build/{device.HardwareModel}.{device.Resolution.Replace("x","")}.0) " +
                    generated;

                return ua;
            }    
            return generated;
        }
        public static string GenerateSnNonce(string emailOrPhoneNumber)
        {
            byte[] b = new byte[24];
            Rnd.NextBytes(b);
            var str = $"{emailOrPhoneNumber}|{DateTimeHelper.ToUnixTime(DateTime.UtcNow)}|{Encoding.UTF8.GetString(b)}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
        public static string GenerateJazoest(string phoneid)
        {
            int ix = 0;
            var chars = phoneid.ToCharArray();
            foreach (var ch in chars)
                ix += (int)ch;
            return "2" + ix;
        }

        static private readonly SecureRandom secureRandom = new SecureRandom();

        public static string GetEncryptedPassword(this IFacebookApi api,
            string password, long? providedTime = null)
        {
            var pubKey = api.GetLoggedUser().PublicKey;
            var pubKeyId = api.GetLoggedUser().PublicKeyId;
            byte[] randKey = new byte[32];
            byte[] iv = new byte[12];
            secureRandom.NextBytes(randKey, 0, randKey.Length);
            secureRandom.NextBytes(iv, 0, iv.Length);
            long time = providedTime ?? DateTime.UtcNow.ToUnixTime();
            byte[] associatedData = Encoding.UTF8.GetBytes(time.ToString());
            var pubKEY = pubKey.Base64Decode();
            byte[] encryptedKey;
            using (var rdr = PemKeyUtils.GetRSAProviderFromPemString(pubKEY.Trim()))
                encryptedKey = rdr.Encrypt(randKey, false);

            byte[] plaintext = Encoding.UTF8.GetBytes(password);

            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(randKey), 128, iv, associatedData);
            cipher.Init(true, parameters);

            var ciphertext = new byte[cipher.GetOutputSize(plaintext.Length)];
            var len = cipher.ProcessBytes(plaintext, 0, plaintext.Length, ciphertext, 0);
            cipher.DoFinal(ciphertext, len);

            var con = new byte[plaintext.Length];
            for (int i = 0; i < plaintext.Length; i++)
                con[i] = ciphertext[i];
            ciphertext = con;
            var tag = cipher.GetMac();

            byte[] buffersSize = BitConverter.GetBytes(Convert.ToInt16(encryptedKey.Length));
            byte[] encKeyIdBytes = BitConverter.GetBytes(Convert.ToUInt16(pubKeyId));
            if (BitConverter.IsLittleEndian)
                Array.Reverse(encKeyIdBytes);
            encKeyIdBytes[0] = 1;
            var payload = encKeyIdBytes
                .Concat(iv)
                .Concat(buffersSize)
                .Concat(encryptedKey)
                .Concat(tag)
                .Concat(ciphertext)
                .ToArray()
                .Base64Encode();

            return $"#PWD_FB4A:2:{time}:{payload}";
        }
        public static string GetThreadToken()
        {
            //6906811681774978189
            var str = "";
            str += Rnd.Next(1, 9);
            str += Rnd.Next(1, 9);
            str += Rnd.Next(1, 9);
            str += Rnd.Next(2, 9);
            str += Rnd.Next(1000, 9999);
            str += Rnd.Next(11111, 99999);

            str += Rnd.Next(2222, 6789);

            return $"92{str}";
        }
        public static (bool, string) CheckpointCheck(HttpHeaders headers)
        {
            if ((headers?.Contains("x-fb-integrity-required") ?? false) &&
                (headers?.Contains("nt") ?? false))
            {
                //x-fb-integrity-enrollment:["UVZSUGFXNUNlVXcxYTFKbWRDMVpXbGN0TFZoa2JVcHJOelpwVUdGTmFXRnVNVGRVU3pSUk1uUk1aMnh1VGxKNk5EUmxYelF0TkRoYVpEbDJaVlZhWjAxa05YTktMVk5pZWw5bWQxUnpXR2hUUzNwemNYcGZSakYwT0d0NlMwZzVUV3RKZDFCU1ozb3lSekpoYmpGc1JVSldNbU0wTUZseVowTkNWRFp3YW14SmJXRkhkVlJRUmsxRWNVUTBiRUZuWVhRNGFqQXlObkkxVDJKMFMySkdZelJ1WmkxVlJHOTNUVmcyVERjM1dGOTZRVkoxUm14YWJUUm9SVnBmUW1aTVFsRndXWFZFT0ZocWNtWldMVFpFWjIxeGQwWnJVWEJzUWpsb04xRm5PSFZmZG1SdGNUWjFSa3RhTlhabVpWQktha2xpVFVneVlXZGlOVVpuWVdKWWFYbFFXVVJOVjNJNVNXNWFkbDh3U1V4Q1RHYzJjRXhFTVhGUGFEVlJPRUZwZVdNeFYxSmpkak5OU21KalVERkJjWEJOZFVGeVh6RmZVbWhaYkZoVlRYZEtOVFJHVVROUVNFc3laMlpEYW5CTlRUQlJXa3RvZDFFMlYyVjVjbmN5TjFOdVQwZHJUMTluVUdKRExVUmZVVVJLTm01SGRFeFFkMGRtV2s5VU9HZEhiVWxmWDA5VmFuZDViVzFxWVRKVWR6VnVRM2s1VVRZeGRWbE9OemRtVFVkUFNsWlBjRVJJVlVOUWRFcGxaR1puTm01aWRGQnNaM2xuYXpKRVJHbHFiM0pvTTBvMmNtWjA="]
                //x-fb-integrity-session-id:["754ed23d-9600-4687-8120-0001b617887a"]
                //x-fb-integrity-required:["checkpoint"]
                //x-fb-integrity-render-mode:["nt"]
                return (true, "Your account got locked by Facebook.\nYou can unlock it via Facebook application and verify that you aren't a bot");
            }
            else
                return (false, "OK");
        }
    }
}

/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;
using System.Security.Cryptography;
using System.Text;

namespace FacebookApiSharp.Helpers
{
    public static class CryptoHelper
    {

        public static string Base64Encode(this string plainText)
        {
            return Base64Encode(Encoding.UTF8.GetBytes(plainText));
        }
        public static string Base64Encode(this byte[] plainTextBytes)
        {
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string CalculateMd5() => CalculateMd5(System.IO.Path.GetRandomFileName());

        public static string CalculateMd5(this string message)
        {
            var encoding = Encoding.UTF8;

            using (var md5 = MD5.Create())
            {
                var hashed = md5.ComputeHash(encoding.GetBytes(message));
                var hash = BitConverter.ToString(hashed).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
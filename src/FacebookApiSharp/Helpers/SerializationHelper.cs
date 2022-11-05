/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace FacebookApiSharp.Helpers
{
    internal class SerializationHelper
    {
        public static Stream SerializeToStream(object o)
        {
            var json = SerializeToString(o);
            var bytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream(bytes);
            return stream;
        }

        public static T DeserializeFromStream<T>(Stream stream)
        {
            var json = new StreamReader(stream).ReadToEnd();
            return DeserializeFromString<T>(json);
        }
        public static string SerializeToString(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static T DeserializeFromString<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
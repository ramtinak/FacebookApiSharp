/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.API;
using System.IO;

namespace FacebookApiSharp.Classes.SessionHandlers
{
    public class FileSessionHandler : ISessionHandler
    {
        public IFacebookApi FacebookApi { get; set; }
        /// <summary>
        ///     Path to file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        ///     Load and Set StateData to InstaApi
        /// </summary>
        public void Load()
        {
            if (File.Exists(FilePath))
            {
                var text = File.ReadAllText(FilePath);
                (FacebookApi as FacebookApi).LoadStateDataFromString(text);
            }
        }

        /// <summary>
        ///     Save current StateData from InstaApi
        /// </summary>
        public void Save()
        {
            File.WriteAllText(FilePath, (FacebookApi as FacebookApi).GetStateDataAsString());
        }
    }
}

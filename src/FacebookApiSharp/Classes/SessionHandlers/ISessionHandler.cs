/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.API;

namespace FacebookApiSharp.Classes.SessionHandlers
{
    public interface ISessionHandler
    {
        IFacebookApi FacebookApi { get; set; }
        /// <summary>
        ///     Path to file
        /// </summary>
        string FilePath { get; set; }

        /// <summary>
        ///     Load and Set StateData to InstaApi
        /// </summary>
        void Load();

        /// <summary>
        ///     Save current StateData from InstaApi
        /// </summary>
        void Save();
    }
}

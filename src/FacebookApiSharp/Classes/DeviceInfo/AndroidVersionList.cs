/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System.Collections.Generic;

namespace FacebookApiSharp.Classes.DeviceInfo
{
    public class AndroidVersionList
    {
        public static AndroidVersionList GetVersionList() => new AndroidVersionList();

        public List<AndroidVersion> AndroidVersions()
        {
            return new List<AndroidVersion>
            {
                new AndroidVersion
                {
                    Codename = "Pie",
                    VersionNumber = "9.0.0",
                    APILevel = "28"
                },
                new AndroidVersion
                {
                    Codename = "Android 10",
                    VersionNumber = "10.0.0",
                    APILevel = "29"
                },
                new AndroidVersion
                {
                    Codename = "Android 11",
                    VersionNumber = "11.0.0",
                    APILevel = "30"
                }
            };
        }
    }
}

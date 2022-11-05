/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;
using System.Linq;

namespace FacebookApiSharp.Classes.DeviceInfo
{
    [Serializable]
    public class AndroidVersion
    {
        public string Codename { get; set; }
        public string VersionNumber { get; set; }
        public string APILevel { get; set; }

        public static AndroidVersion FromString(string versionString)
        {
            var version = new Version(versionString);
            foreach (var androidVersion in AndroidVersionList.GetVersionList().AndroidVersions())
                if (version.CompareTo(new Version(androidVersion.VersionNumber)) == 0 ||
                    version.CompareTo(new Version(androidVersion.VersionNumber)) > 0 &&
                    androidVersion != AndroidVersionList.GetVersionList().AndroidVersions().Last() &&
                    version.CompareTo(new Version(AndroidVersionList.GetVersionList().AndroidVersions()[AndroidVersionList.GetVersionList().AndroidVersions().IndexOf(androidVersion) + 1]
                        .VersionNumber)) < 0)
                    return androidVersion;
            return null;
        }

        static Random Rnd = new Random();
        private static AndroidVersion LastAndriodVersion = AndroidVersionList.GetVersionList().AndroidVersions()[AndroidVersionList.GetVersionList().AndroidVersions().Count - 2];
        public static AndroidVersion GetRandomAndriodVersion()
        {
        TryLabel:
            var randomDeviceIndex = Rnd.Next(0, AndroidVersionList.GetVersionList().AndroidVersions().Count);
            var androidVersion = AndroidVersionList.GetVersionList().AndroidVersions().ElementAt(randomDeviceIndex);
            if (LastAndriodVersion != null)
                if (androidVersion.APILevel == LastAndriodVersion.APILevel)
                    goto TryLabel;
            LastAndriodVersion = androidVersion;
            return androidVersion;
        }
        public static AndroidVersion GetAndroidVersion(string apiLevel)
        {
            if (string.IsNullOrEmpty(apiLevel)) return null;

            return AndroidVersionList.GetVersionList().AndroidVersions().FirstOrDefault(api => api.APILevel == apiLevel);
        }
        public static AndroidVersion GetAndroid9()
        {
            var androidVer = AndroidVersionList.GetVersionList().AndroidVersions().FirstOrDefault(api => api.VersionNumber == "9.0.0");
            androidVer.VersionNumber = "9";
            return androidVer;
        }
    }
}
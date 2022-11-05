/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;

namespace FacebookApiSharp.Classes.DeviceInfo
{
    [Serializable]
    public class AndroidDevice
    {
        public Guid PhoneGuid { get; set; }
        public Guid DeviceGuid { get; set; }
        public Guid GoogleAdId { get; set; } = Guid.NewGuid();
        public Guid RankToken { get; set; } = Guid.NewGuid();
        public Guid AdId { get; set; } = Guid.NewGuid();
        public Guid PigeonSessionId { get; set; } = Guid.NewGuid();
        public Guid PushDeviceGuid { get; set; } = Guid.NewGuid();
        public Guid FamilyDeviceGuid { get; set; }

        public AndroidVersion AndroidVer { get; set; } = AndroidVersion.GetRandomAndriodVersion();

        public string AndroidBoardName { get; set; }
        public string AndroidBootloader { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceModelBoot { get; set; } = "qcom";
        public string DeviceModelIdentifier { get; set; }
        public string FirmwareBrand { get; set; }
        public string FirmwareFingerprint { get; set; }
        public string FirmwareTags { get; set; }
        public string FirmwareType { get; set; }
        public string HardwareManufacturer { get; set; }
        public string HardwareModel { get; set; }
        public string Resolution { get; set; } = "1080x1812";
        public string Dpi { get; set; } = "480dpi";



        public const string CPU_ABI = "arm64-v8a:";

    }
}
/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace FacebookApiSharp.Classes.DeviceInfo
{
    public class AndroidDeviceGenerator
    {
        private static readonly List<string> DevicesNames = new List<string>
        {
            AndroidDevices.HONOR_8LITE,
            AndroidDevices.XIAOMI_MI_4W,
            AndroidDevices.XIAOMI_HM_1SW,
            AndroidDevices.HTC_ONE_PLUS
        };

        public static Dictionary<string, AndroidDevice> AndroidAndroidDeviceSets = new Dictionary<string, AndroidDevice>
        {
            {
                AndroidDevices.HONOR_8LITE,
                new AndroidDevice
                {
                    AndroidBoardName = "HONOR",
                    DeviceBrand = "HUAWEI",
                    HardwareManufacturer = "HUAWEI",
                    DeviceModel = "PRA-LA1",
                    DeviceModelIdentifier = "PRA-LA1",
                    FirmwareBrand = "HWPRA-H",
                    HardwareModel = "hi6250",
                    DeviceGuid = Guid.NewGuid(),
                    PhoneGuid = Guid.NewGuid(),
                    Resolution = "1080x1812",
                    Dpi = "480dpi",
                }
            },
            {
                AndroidDevices.XIAOMI_MI_4W,
                new AndroidDevice
                {
                    AndroidBoardName = "MI",
                    DeviceBrand = "Xiaomi",
                    HardwareManufacturer = "Xiaomi",
                    DeviceModel = "MI-4W",
                    DeviceModelIdentifier = "4W",
                    FirmwareBrand = "4W",
                    HardwareModel = "cancro",
                    DeviceGuid = Guid.NewGuid(),
                    PhoneGuid = Guid.NewGuid(),
                    Resolution = "1080x1920",
                    Dpi = "480dpi",
                }
            },
            {
                AndroidDevices.XIAOMI_HM_1SW,
                new AndroidDevice
                {
                    AndroidBoardName = "HM",
                    DeviceBrand = "Xiaomi",
                    HardwareManufacturer = "Xiaomi",
                    DeviceModel = "HM-1SW",
                    DeviceModelIdentifier = "1SW",
                    FirmwareBrand = "1SW",
                    HardwareModel = "armani",
                    DeviceGuid = Guid.NewGuid(),
                    PhoneGuid = Guid.NewGuid(),
                    Resolution = "720x1280",
                    Dpi = "320dpi",
                }
            },
            {
                AndroidDevices.HTC_ONE_PLUS,
                new AndroidDevice
                {
                    AndroidBoardName = "One",
                    DeviceBrand = "Htc",
                    HardwareManufacturer = "Htc",
                    DeviceModel = "One-Plus",
                    DeviceModelIdentifier = "Plus",
                    FirmwareBrand = "Plus",
                    HardwareModel = "A3010",
                    DeviceGuid = Guid.NewGuid(),
                    PhoneGuid = Guid.NewGuid(),
                    Resolution = "1080x1920",
                    Dpi = "380dpi",
                }
            }
        };

        static readonly Random Rnd = new Random();
        public static AndroidDevice GetRandomAndroidDevice()
        {
            var randomDeviceIndex = Rnd.Next(0, DevicesNames.Count);
            var device = AndroidAndroidDeviceSets.ElementAt(randomDeviceIndex).Value;
            device.FamilyDeviceGuid = device.PhoneGuid = Guid.NewGuid();
            device.DeviceGuid = Guid.NewGuid();
            device.PigeonSessionId = Guid.NewGuid();
            device.PushDeviceGuid = Guid.NewGuid();

            return device;
        }

        public static AndroidDevice GetByName(string name)
        {
            return AndroidAndroidDeviceSets[name];
        }

        public static AndroidDevice GetById(string deviceId)
        {
            return (from androidAndroidDeviceSet in AndroidAndroidDeviceSets
                    where androidAndroidDeviceSet.Value.DeviceId == deviceId
                    select androidAndroidDeviceSet.Value).FirstOrDefault();
        }
    }
}
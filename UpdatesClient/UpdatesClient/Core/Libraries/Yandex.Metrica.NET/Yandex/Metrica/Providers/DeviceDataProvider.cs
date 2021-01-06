using System;
using System.Management;
using System.Threading.Tasks;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
    internal class DeviceDataProvider : ADataProvider<Task<DeviceProperties>>
    {
#pragma warning disable IDE0051 // Удалите неиспользуемые закрытые члены
        private const string StartupPlatform = "dotnet";
#pragma warning restore IDE0051 // Удалите неиспользуемые закрытые члены

        protected override async Task<DeviceProperties> ProvideOrThrowException()
        {
            DeviceProperties deviceProperties = new DeviceProperties()
            {
                OSPlatform = Environment.OSVersion.Platform.ToString(),
                OSVersion = Environment.OSVersion.Version,
                StartupPlatform = "dotnet"
            };
            try
            {
                DeviceDataProvider.FillDeviceProperties(deviceProperties);
            }
            catch (Exception)
            {
            }
            try
            {
                DeviceDataProvider.FillDisplayProperties(deviceProperties);
            }
            catch (Exception)
            {
            }
            return await Task.FromResult<DeviceProperties>(deviceProperties);
        }

        private static void FillDeviceProperties(DeviceProperties deviceProperties)
        {
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher(new ManagementScope("\\\\.\\ROOT\\cimv2"), new ObjectQuery("SELECT * FROM Win32_ComputerSystemProduct")).Get().GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return;
                foreach (PropertyData property in enumerator.Current.Properties)
                {
                    string str = property.Value == null ? (string)null : property.Value.ToString();
                    string input = string.IsNullOrWhiteSpace(str) ? "unknown" : str;
                    string name = property.Name;
                    if (!(name == "Vendor"))
                    {
                        if (!(name == "Name"))
                        {
                            if (name == "UUID")
                            {
                                deviceProperties.Id = Guid.TryParse(input, out Guid result) ? result.ToString("N") : Identification.GetDeviceId();
                            }
                        }
                        else
                            deviceProperties.ModelName = input;
                    }
                    else
                        deviceProperties.Manufacturer = input;
                }
            }
        }

        private static void FillDisplayProperties(DeviceProperties deviceProperties)
        {
            deviceProperties.ScaleFactor = 1f;
            using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementObjectSearcher(new ManagementScope("\\\\.\\ROOT\\cimv2"), new ObjectQuery("SELECT * FROM CIM_VideoController")).Get().GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return;
                foreach (PropertyData property in enumerator.Current.Properties)
                {
                    object obj = property.Value;
                    if (obj != null)
                    {
                        string name = property.Name;
                        if (!(name == "CurrentHorizontalResolution"))
                        {
                            if (name == "CurrentVerticalResolution")
                                deviceProperties.ScreenHeight = (uint)obj;
                        }
                        else
                            deviceProperties.ScreenWidth = (uint)obj;
                    }
                }
            }
        }
    }
}

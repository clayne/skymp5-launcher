using System;

namespace Yandex.Metrica.Models
{
    internal class DeviceProperties
    {
        public DeviceProperties()
        {
            this.ScaleFactor = 1f;
        }

        public string Id { get; set; }

        public string Manufacturer { get; set; }

        public string ModelName { get; set; }

        public uint ScreenWidth { get; set; }

        public uint ScreenHeight { get; set; }

        public float ScaleFactor { get; set; }

        public string StartupPlatform { get; set; }

        public Version OSVersion { get; set; }

        public string OSPlatform { get; set; }

        public uint Dpi { get; set; }

        public float DisplayDiagonal { get; set; }

        public string GetDeviceType()
        {
            return ReportMessage.RequestParameters.DeviceType.DESKTOP.ToString();
        }
    }
}

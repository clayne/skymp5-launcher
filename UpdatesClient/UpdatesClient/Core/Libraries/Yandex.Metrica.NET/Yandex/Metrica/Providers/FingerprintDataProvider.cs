using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
    internal class FingerprintDataProvider : ADataProvider<Task<string>>
    {
        private static Task<string> GetDeviceFingerprint()
        {
            string publisherApps = FingerprintDataProvider.GetPublisherApps();
            ulong bootTime = DateTime.UtcNow.ToUnixTime() - (ulong)Environment.TickCount / 1000UL;
            FingerprintDataProvider.DiskSpace diskSpace = FingerprintDataProvider.GetDiskSpace();
            ulong? nullable1;
            if (diskSpace == null)
            {
                nullable1 = new ulong?();
            }
            else
            {
                ulong? totalSpace = diskSpace.TotalSpace;
                ulong num = 1024;
                nullable1 = totalSpace.HasValue ? new ulong?(totalSpace.GetValueOrDefault() / num) : new ulong?();
            }
            ulong? nullable2 = nullable1;
            ulong? nullable3;
            if (diskSpace == null)
            {
                nullable3 = new ulong?();
            }
            else
            {
                ulong? freeSpace = diskSpace.FreeSpace;
                ulong num = 1024;
                nullable3 = freeSpace.HasValue ? new ulong?(freeSpace.GetValueOrDefault() / num) : new ulong?();
            }
            ulong? nullable4 = nullable3;
            object[] objArray = new object[4];
            ulong? nullable5 = nullable2;
            objArray[0] = (object)(ulong)(nullable5.HasValue ? (long)nullable5.GetValueOrDefault() : 0L);
            nullable5 = nullable4;
            objArray[1] = (object)(ulong)(nullable5.HasValue ? (long)nullable5.GetValueOrDefault() : 0L);
            objArray[2] = (object)bootTime;
            objArray[3] = (object)publisherApps;
            return Task.FromResult(string.Format("{{\n    \"dfid\": {{\n        \"tds\": {0},\n        \"fds\": {1},\n        \"boot_time\": {2},\n        \"apps\": {{\n                \"version\": 0,\n                \"names\": [{3}]\n                }}\n    }}\n}}", objArray));
        }

        private static string GetPublisherApps()
        {
            List<string> source;
            try
            {
                source = new List<string>();
            }
            catch
            {
                source = new List<string>();
            }
            return source.Count <= 0 ? string.Empty : "\"" + source.Aggregate<string>((Func<string, string, string>)((a, b) => a + "\", \"" + b)) + "\"";
        }

        private static DiskSpace GetDiskSpace()
        {
            try
            {
                ulong? nullable1 = new ulong?();
                nullable1 = new ulong?(0UL);
                ulong? nullable3 = nullable1;
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                    {
                        ulong? nullable4 = nullable1;
                        ulong availableFreeSpace = (ulong)drive.AvailableFreeSpace;
                        nullable1 = nullable4.HasValue ? new ulong?(nullable4.GetValueOrDefault() + availableFreeSpace) : new ulong?();
                        ulong? nullable5 = nullable3;
                        ulong totalSize = (ulong)drive.TotalSize;
                        nullable3 = nullable5.HasValue ? new ulong?(nullable5.GetValueOrDefault() + totalSize) : new ulong?();
                    }
                }
                return new FingerprintDataProvider.DiskSpace()
                {
                    TotalSpace = nullable3,
                    FreeSpace = nullable1
                };
            }
            catch (Exception)
            {
                return (FingerprintDataProvider.DiskSpace)null;
            }
        }

        protected override Task<string> ProvideOrThrowException()
        {
            return FingerprintDataProvider.GetDeviceFingerprint();
        }

        private class DiskSpace
        {
            public ulong? FreeSpace { get; set; }

            public ulong? TotalSpace { get; set; }
        }
    }
}

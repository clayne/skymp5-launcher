using Newtonsoft.Json;
using Security;
using System;
using System.Threading.Tasks;
using System.Windows;
using UpdatesClient.Core;
using UpdatesClient.Modules.SelfUpdater.Models;

namespace UpdatesClient.Modules.SelfUpdater
{
    internal static class Security
    {
        internal static VersionStatus Status;
        internal static string UID;

        internal static bool CheckEnvironment()
        {
            Task<bool> checking = CheckVersion();
            checking.Wait();
            if (!checking.Result)
            {
                MessageBox.Show("The version is revoked\nPlease download the new version from skymp.io", "Is not a bug", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            UID = Hashing.GetMD5FromText(SystemFunctions.GetHWID());
            AesEncoder.Init();
#if (DEBUG)

#elif (BETA)
            if (!CheckInjection()) return false;
#else
            if (!CheckInjection()) return false;
#endif
            return true;
        }

#pragma warning disable IDE0051 // Удалите неиспользуемые закрытые члены
        private static bool CheckInjection()
#pragma warning restore IDE0051 // Удалите неиспользуемые закрытые члены
        {
            if (WinFunctions.GetModuleHandle("SbieDll.dll") != IntPtr.Zero) return false;
            return true;
        }

        private static async Task<bool> CheckVersion()
        {
            string jsn = await Core.Net.Request($"{Core.Net.URL_ApiLauncher}CheckVersion/{EnvParams.VersionFile}", "POST", false, null);
            Status = JsonConvert.DeserializeObject<VersionStatus>(jsn);

            return !(Status.Block && Status.Full);
        }
    }
}

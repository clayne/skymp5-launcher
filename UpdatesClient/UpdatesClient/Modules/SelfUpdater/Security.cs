using Security;
using System;

namespace UpdatesClient.Modules.SelfUpdater
{
    internal static class Security
    {
        internal static string UID;

        internal static bool CheckEnvironment()
        {
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
    }
}

using System;
using System.Management;

namespace UpdatesClient.Modules.SelfUpdater
{
    internal static class Security
    {
        internal static bool CheckEnvironment()
        {
#if (DEBUG)

#elif (BETA)
            if (!CheckInjection()) return false;
            if (!CheckHWID()) return false;
#else
            if (!CheckInjection()) return false;
            if (!CheckHWID()) return false;
#endif
            return true;
        }

        private static bool CheckInjection()
        {
            if (WinFunctions.GetModuleHandle("SbieDll.dll") != IntPtr.Zero) return false;

            return true;
        }
        private static bool CheckHWID()
        {
            try
            {
                ManagementObjectCollection mbsList = new ManagementObjectSearcher("Select ProcessorId From Win32_processor").Get();
                string id = "";
                foreach (ManagementObject mo in mbsList)
                {
                    id = mo["ProcessorId"].ToString();
                    break;
                }
                return id != "";
            }
            catch
            {
                return false;
            }
        }
    }
}

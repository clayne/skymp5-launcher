using System;
using System.Runtime.InteropServices;

namespace UpdatesClient.Modules.SelfUpdater
{
    internal static class WinFunctions
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}

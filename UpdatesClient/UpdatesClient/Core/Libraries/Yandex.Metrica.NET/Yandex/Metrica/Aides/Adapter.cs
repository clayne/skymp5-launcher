using System;
using System.Reflection;
using System.Text;

namespace Yandex.Metrica.Aides
{
    internal static class Adapter
    {
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }

        public static Assembly GetEntryAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        public static byte[] ExtractData(object o)
        {
            return o is UnhandledExceptionEventArgs exceptionEventArgs ? Encoding.UTF8.GetBytes(exceptionEventArgs.ExceptionObject.ToString()) : (byte[])null;
        }

        public static bool IsInternalException(object o)
        {
            return o is UnhandledExceptionEventArgs exceptionEventArgs && exceptionEventArgs.ExceptionObject.ToString().Contains("Yandex.Metrica");
        }

#pragma warning disable IDE0060 // Удалите неиспользуемый параметр
        public static void TryHandleException(object o)
#pragma warning restore IDE0060 // Удалите неиспользуемый параметр
        {
        }
    }
}

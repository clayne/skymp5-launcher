// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aides.Adapter
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

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
      return o is UnhandledExceptionEventArgs exceptionEventArgs ? Encoding.UTF8.GetBytes(exceptionEventArgs.ExceptionObject.ToString()) : (byte[]) null;
    }

    public static bool IsInternalException(object o)
    {
      return o is UnhandledExceptionEventArgs exceptionEventArgs && exceptionEventArgs.ExceptionObject.ToString().Contains("Yandex.Metrica");
    }

    public static void TryHandleException(object o)
    {
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aides.UnixTimestampConverter
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;

namespace Yandex.Metrica.Aides
{
  internal static class UnixTimestampConverter
  {
    public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

    public static DateTime ToDateTime(this uint value)
    {
      return UnixTimestampConverter.Epoch.AddSeconds((double) value);
    }

    public static DateTime ToDateTime(this ulong value)
    {
      return UnixTimestampConverter.Epoch.AddSeconds((double) value);
    }

    public static ulong ToUnixTime(this DateTime value)
    {
      return (ulong) (value.ToUniversalTime() - UnixTimestampConverter.Epoch).TotalSeconds;
    }
  }
}

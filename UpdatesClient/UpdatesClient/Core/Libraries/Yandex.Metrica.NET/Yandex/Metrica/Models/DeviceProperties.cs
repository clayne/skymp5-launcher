// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Models.DeviceProperties
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

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

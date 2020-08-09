// Decompiled with JetBrains decompiler
// Type: Windows.Devices.Geolocation.Geocoordinate
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;

namespace Windows.Devices.Geolocation
{
  public sealed class Geocoordinate
  {
    public double Accuracy { get; set; }

    public double? Altitude { get; set; }

    public double? AltitudeAccuracy { get; set; }

    public double? Heading { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public double? Speed { get; set; }

    public DateTimeOffset Timestamp { get; set; }
  }
}

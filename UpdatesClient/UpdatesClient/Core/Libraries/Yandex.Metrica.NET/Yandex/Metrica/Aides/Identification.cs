﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aides.Identification
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using Yandex.Metrica.Models;

namespace Yandex.Metrica.Aides
{
  internal static class Identification
  {
    public static string GetDeviceId()
    {
      return ServiceData.Device.Id ?? string.Empty;
    }

    public static string GetNetworkAdapterId()
    {
      try
      {
        System.Net.NetworkInformation.NetworkInterface[] networkInterfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
        System.Net.NetworkInformation.NetworkInterface networkInterface = ((IEnumerable<System.Net.NetworkInformation.NetworkInterface>) networkInterfaces).FirstOrDefault<System.Net.NetworkInformation.NetworkInterface>((Func<System.Net.NetworkInformation.NetworkInterface, bool>) (i => i.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)) ?? ((IEnumerable<System.Net.NetworkInformation.NetworkInterface>) networkInterfaces).FirstOrDefault<System.Net.NetworkInformation.NetworkInterface>((Func<System.Net.NetworkInformation.NetworkInterface, bool>) (i => i.NetworkInterfaceType == NetworkInterfaceType.Ethernet)) ?? ((IEnumerable<System.Net.NetworkInformation.NetworkInterface>) networkInterfaces).FirstOrDefault<System.Net.NetworkInformation.NetworkInterface>();
        if (networkInterface == null)
          return (string) null;
        string id = networkInterface.Id;
        return string.IsNullOrEmpty(id) ? (string) null : id;
      }
      catch
      {
        return (string) null;
      }
    }

    public static string GetAdvertisingId()
    {
      return (string) null;
    }

    public static ProductInfo GetProductInfo()
    {
      Assembly entryAssembly = Adapter.GetEntryAssembly();
      if (!(entryAssembly == (Assembly) null))
        return new ProductInfo()
        {
          Id = entryAssembly.GetName().Name,
          Version = entryAssembly.GetName().Version
        };
      return new ProductInfo()
      {
        Id = Config.Global.Id.ToString(),
        Version = new Version("0.0")
      };
    }
  }
}

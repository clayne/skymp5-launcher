// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Models.Critical
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;

namespace Yandex.Metrica.Models
{
  internal static class Critical
  {
    private static readonly object UuidChangesLock;

    static Critical()
    {
      if (Memory.ActiveBox.Check<Critical.CriticalConfig>((string) null))
      {
        Critical.Data = Memory.ActiveBox.Revive<Critical.CriticalConfig>((string) null);
      }
      else
      {
        Critical.Data = new Critical.CriticalConfig();
        Critical.SetDeviceId(Identification.GetDeviceId());
      }
      Critical.Data.ApiKeys = Critical.Data.ApiKeys ?? new List<Guid>();
      Critical.UuidChangesLock = new object();
    }

    private static Critical.CriticalConfig Data { get; }

    public static void SetUuid(string uuid)
    {
      if (uuid == null)
        return;
      bool flag = false;
      lock (Critical.UuidChangesLock)
      {
        if (Critical.Data.Uuid == null)
        {
          flag = true;
          Critical.Data.Uuid = uuid;
        }
      }
      if (!flag)
        return;
      Critical.Submit();
    }

    public static void SetDeviceId(string deviceId)
    {
      Critical.Data.DeviceId = deviceId;
      Critical.Submit();
    }

    public static void Submit()
    {
      Memory.ActiveBox.Keep<Critical.CriticalConfig>(Critical.Data, (string) null);
    }

    public static string GetUuid()
    {
      return Critical.Data.Uuid;
    }

    public static string GetDeviceId()
    {
      return Critical.Data.DeviceId;
    }

    public static bool IsUuidRequired()
    {
      return string.IsNullOrWhiteSpace(Critical.GetUuid());
    }

    public static bool IsDeviceIdRequired()
    {
      return string.IsNullOrWhiteSpace(Critical.GetDeviceId());
    }

    public static void AddApiKey(Guid apiKey)
    {
      Critical.Data.ApiKeys.Add(apiKey);
    }

    public static Guid[] GetApiKeys()
    {
      return Critical.Data.ApiKeys.ToArray();
    }

    [DataContract]
    internal class CriticalConfig
    {
      [DataMember]
      public string DeviceId { get; set; }

      [DataMember]
      public string Uuid { get; set; }

      [DataMember]
      public List<Guid> ApiKeys { get; set; }
    }
  }
}

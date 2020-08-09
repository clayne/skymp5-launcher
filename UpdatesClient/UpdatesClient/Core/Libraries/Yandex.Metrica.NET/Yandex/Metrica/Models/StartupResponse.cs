// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Models.StartupResponse
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Runtime.Serialization;

namespace Yandex.Metrica.Models
{
  [DataContract]
  internal class StartupResponse
  {
    [DataMember(Name = "query_hosts")]
    public StartupResponse.HostContainer QueryHosts { get; set; }

    [DataMember(Name = "uuid")]
    public StartupResponse.ValueContainer UuidContainer { get; set; }

    [DataMember(Name = "device_id")]
    public StartupResponse.ValueContainer DeviceIdContainer { get; set; }

    [DataMember]
    public TimeSpan ServerTimeOffset { get; set; }

    [DataMember]
    public DateTime ServerDateTime { get; set; }

    public string Uuid
    {
      get
      {
        return this.UuidContainer?.Value;
      }
    }

    public string DeviceId
    {
      get
      {
        return this.DeviceIdContainer?.Value;
      }
    }

    public string ReportUrl
    {
      get
      {
        return this.QueryHosts?.List?.Report?.Url;
      }
    }

    [DataContract]
    internal class ValueContainer
    {
      [DataMember(Name = "value")]
      public string Value { get; set; }
    }

    [DataContract]
    internal class HostContainer
    {
      [DataMember(Name = "list")]
      internal StartupResponse.HostContainer.HostList List { get; set; }

      [DataContract]
      internal class HostList
      {
        [DataMember(Name = "report")]
        public StartupResponse.HostContainer.HostList.Host Report { get; set; }

        [DataContract]
        public class Host
        {
          [DataMember(Name = "url")]
          public string Url { get; set; }
        }
      }
    }
  }
}

﻿// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Models.SessionModel
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.Runtime.Serialization;

namespace Yandex.Metrica.Models
{
  [DataContract]
  internal class SessionModel : ReportMessage.Session
  {
    [DataMember]
    public ulong EventCounter;

    public bool AsyncLocationLock { get; set; }

    [DataMember]
    public string ReportParameters { get; set; }

    [DataMember]
    public ulong? LastUpdateTimestamp { get; set; }

    [DataMember]
    public ulong? LastEventTimestamp { get; set; }

    [DataMember]
    public ulong? LastEventType { get; set; }
  }
}

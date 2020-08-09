// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Models.ReportPackage
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;

namespace Yandex.Metrica.Models
{
  [DataContract]
  internal class ReportPackage
  {
    public const int MaxReportPackageLength = 229376;
    private ReportMessage _reportMessage;
    private MemoryStream _rawStream;

    public ReportPackage(string urlParameters, IEnumerable<ReportMessage.Session> sessions)
    {
      this.Key = string.Format(Memory.ActiveBox.KeyFormat, (object) Guid.NewGuid());
      this.UrlParameters = urlParameters;
      this._reportMessage = ReportPackage.ToReportMessage(sessions);
      this._rawStream = this.GetRawStream();
    }

    ~ReportPackage()
    {
      this._rawStream?.Dispose();
    }

    [DataMember]
    public string Key { get; set; }

    [DataMember]
    public string UrlParameters { get; set; }

    public bool IsLarge
    {
      get
      {
        return this.Length > 229376L;
      }
    }

    public long Length
    {
      get
      {
        MemoryStream rawStream = this._rawStream;
        return rawStream == null ? Memory.ActiveBox.Storage.Length(this.Key) : rawStream.Length;
      }
    }

    public bool Exists()
    {
      return Memory.ActiveBox.Storage.HasKey(this.Key);
    }

    public void Fade()
    {
      Memory.ActiveBox.Storage.DeleteKey(this.Key);
      this._rawStream?.Dispose();
    }

    public void Keep()
    {
      if (this._reportMessage == null)
        return;
      using (Stream writeStream = Memory.ActiveBox.Storage.GetWriteStream(this.Key))
        ReportMessage.Serialize(writeStream, this._reportMessage);
    }

    public void Revive()
    {
      using (Stream readStream = Memory.ActiveBox.Storage.GetReadStream(this.Key))
        this._reportMessage = ReportMessage.Deserialize(readStream);
    }

    public MemoryStream GetRawStream()
    {
      if (this._rawStream != null)
      {
        this._rawStream.Position = 0L;
        return this._rawStream;
      }
      if (this.Exists())
        this.Revive();
      this._rawStream = new MemoryStream();
      ReportMessage reportMessage = this._reportMessage;
      if (reportMessage != null)
        reportMessage.Write((Stream) this._rawStream);
      this._rawStream.Position = 0L;
      return this._rawStream;
    }

    private static ReportMessage ToReportMessage(
      IEnumerable<ReportMessage.Session> sessions)
    {
      DateTimeOffset now = DateTimeOffset.Now;
      return new ReportMessage()
      {
        sessions = sessions.ToList<ReportMessage.Session>(),
        send_time = new ReportMessage.Time()
        {
          timestamp = now.DateTime.ToUnixTime(),
          time_zone = (int) now.Offset.TotalSeconds
        }
      };
    }

    public List<ReportPackage> Split()
    {
      int count = this._reportMessage.sessions.Count;
      int num = count / 2;
      if (count > 1)
        return new List<ReportPackage>()
        {
          new ReportPackage(this.UrlParameters, (IEnumerable<ReportMessage.Session>) this._reportMessage.sessions.GetRange(0, num)),
          new ReportPackage(this.UrlParameters, (IEnumerable<ReportMessage.Session>) this._reportMessage.sessions.GetRange(num, count - num))
        };
      if (this.IsLarge)
        return new List<ReportPackage>();
      return new List<ReportPackage>() { this };
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Models.EventFactory
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.Text;
using Yandex.Metrica.Aides;

namespace Yandex.Metrica.Models
{
  internal static class EventFactory
  {
    public static ReportMessage.Session.Event Create(
      ReportMessage.Session.Event.EventType type,
      byte[] value = null,
      string name = null,
      string environment = null)
    {
      return new ReportMessage.Session.Event()
      {
        type = (uint) type,
        name = name,
        value = value,
        environment = environment
      };
    }

    public static ReportMessage.Session.Event Create(
      ReportMessage.Session.Event.EventType type,
      string value,
      string name = null)
    {
      return EventFactory.Create(type, value == null ? (byte[]) null : Encoding.UTF8.GetBytes(value), name, (string) null);
    }

    public static ReportMessage.Session.Event Create(string name, string jsonData = null)
    {
      return EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_CLIENT, jsonData, name);
    }

    public static ReportMessage.Session.Event Create<TItem>(
      string name,
      TItem serializableItem)
    {
      return EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_CLIENT, ((object) serializableItem).ToJson(JsonProfile.GetFormatted(), (System.Type) null, 1), name);
    }
  }
}

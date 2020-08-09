// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Models.SessionExtensions
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;

namespace Yandex.Metrica.Models
{
  internal static class SessionExtensions
  {
    public const int MaxValueLength = 50000;
    public const int MaxNameLength = 1000;

    public static string Truncate(this string str, int maxLength, ref uint bytesTruncated)
    {
      if (str == null || str.Length <= maxLength)
        return str;
      string s = str.Substring(0, maxLength);
      bytesTruncated += (uint) (Encoding.UTF8.GetByteCount(str) - Encoding.UTF8.GetByteCount(s));
      return s;
    }

    public static byte[] Truncate(this byte[] bytes, int maxLength, ref uint bytesTruncated)
    {
      if (bytes == null || bytes.Length <= maxLength)
        return bytes;
      bytesTruncated += (uint) (bytes.Length - maxLength);
      return ((IEnumerable<byte>) bytes).Take<byte>(maxLength).ToArray<byte>();
    }

    public static void AggregateEvents(
      this SessionModel session,
      ulong currentUnixTime,
      params ReportMessage.Session.Event[] items)
    {
      if (session?.session_desc?.start_time == null)
        return;
      if (items.Length != 0)
      {
        session.LastEventTimestamp = new ulong?(currentUnixTime);
        session.LastEventType = new ulong?((ulong) ((IEnumerable<ReportMessage.Session.Event>) items).Last<ReportMessage.Session.Event>().type);
      }
      if (Config.Global.LocationTracking)
        session.AttachLocationAsync(items);
      foreach (ReportMessage.Session.Event @event in items)
      {
        uint bytesTruncated = 0;
        @event.name = @event.name.Truncate(1000, ref bytesTruncated);
        @event.value = @event.value.Truncate(50000, ref bytesTruncated);
        @event.bytes_truncated = new uint?(bytesTruncated);
        @event.time = currentUnixTime - session.session_desc.start_time.timestamp;
        @event.number = session.EventCounter++;
        session.events.Add(@event);
        @event.AttachNetworkInfoAsync();
      }
    }

    public static async void AttachLocationAsync(
      this SessionModel session,
      params ReportMessage.Session.Event[] items)
    {
      ReportMessage.Location location = Config.Global.CustomLocation;
      if (location == null)
      {
        session.AsyncLocationLock = true;
        await ServiceData.WaitExposeAsync();
        ReportMessage.Location location1 = location;
        location = await ServiceData.LocationTracker.Provide();
        session.AsyncLocationLock = false;
      }
      ((IEnumerable<ReportMessage.Session.Event>) items).ForEach<ReportMessage.Session.Event>((Action<ReportMessage.Session.Event>) (i => i.location = location));
    }

    public static async void AttachNetworkInfoAsync(this ReportMessage.Session.Event item)
    {
      await ServiceData.WaitExposeAsync();
      ReportMessage.Session.Event @event = item;
      ReportMessage.Session.Event.NetworkInfo networkInfo = await TaskEx.FromResult<ReportMessage.Session.Event.NetworkInfo>(ServiceData.NetworkTracker.Provide());
      @event.network_info = networkInfo;
      @event = (ReportMessage.Session.Event) null;
    }
  }
}

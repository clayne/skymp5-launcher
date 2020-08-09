// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Providers.NetworkDataProvider
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
  internal class NetworkDataProvider : ADataProvider<ReportMessage.Session.Event.NetworkInfo>
  {
    protected override ReportMessage.Session.Event.NetworkInfo ProvideOrThrowException()
    {
      if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        return (ReportMessage.Session.Event.NetworkInfo) null;
      return new ReportMessage.Session.Event.NetworkInfo()
      {
        connection_type = new ReportMessage.Session.ConnectionType?(ReportMessage.Session.ConnectionType.CONNECTION_WIFI)
      };
    }
  }
}

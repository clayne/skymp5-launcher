// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Providers.LocationDataProvider
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System.Threading.Tasks;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
  internal class LocationDataProvider : ADataProvider<Task<ReportMessage.Location>>
  {
    protected override Task<ReportMessage.Location> ProvideOrThrowException()
    {
      return Task.FromResult<ReportMessage.Location>((ReportMessage.Location) null);
    }
  }
}

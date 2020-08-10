using System.Threading.Tasks;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
    internal class LocationDataProvider : ADataProvider<Task<ReportMessage.Location>>
    {
        protected override Task<ReportMessage.Location> ProvideOrThrowException()
        {
            return Task.FromResult<ReportMessage.Location>((ReportMessage.Location)null);
        }
    }
}

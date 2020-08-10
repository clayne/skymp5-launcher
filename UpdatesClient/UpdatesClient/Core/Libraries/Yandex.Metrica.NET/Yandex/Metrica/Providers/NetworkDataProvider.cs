using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica.Providers
{
    internal class NetworkDataProvider : ADataProvider<ReportMessage.Session.Event.NetworkInfo>
    {
        protected override ReportMessage.Session.Event.NetworkInfo ProvideOrThrowException()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                return (ReportMessage.Session.Event.NetworkInfo)null;
            return new ReportMessage.Session.Event.NetworkInfo()
            {
                connection_type = new ReportMessage.Session.ConnectionType?(ReportMessage.Session.ConnectionType.CONNECTION_WIFI)
            };
        }
    }
}

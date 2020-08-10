using System;

namespace Yandex.Metrica
{
    public class YandexMetricaActivator
    {
        public string ApiKey
        {
            get
            {
                return YandexMetrica.Config.ApiKey.ToString();
            }
            set
            {
                if (YandexMetricaActivator.DesignMode)
                    return;
                YandexMetrica.Activate(new Guid(value));
            }
        }

        private static bool DesignMode
        {
            get
            {
                return false;
            }
        }
    }
}

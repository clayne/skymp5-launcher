namespace Yandex.Metrica
{
    public static class YandexMetricaFolder
    {
        public static string Current { get; private set; }

        static YandexMetricaFolder()
        {
            YandexMetricaFolder.Current = "";
            YandexMetricaFolder.Current = (string)null;
        }

        public static void SetCurrent(string path)
        {
            YandexMetricaFolder.Current = path ?? "";
        }
    }
}

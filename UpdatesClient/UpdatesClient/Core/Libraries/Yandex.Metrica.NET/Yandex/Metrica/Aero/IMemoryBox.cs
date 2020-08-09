namespace Yandex.Metrica.Aero
{
    internal interface IMemoryBox
    {
        void Keep<TItem>(TItem item, string key = null);

        bool Check<TItem>(string key = null);

        void Destroy<TItem>(string key = null);

        TItem Revive<TItem>(string key = null, params object[] constructorArgs);
    }
}

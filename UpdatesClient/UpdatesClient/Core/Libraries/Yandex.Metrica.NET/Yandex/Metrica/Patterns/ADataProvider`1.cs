using System;

namespace Yandex.Metrica.Patterns
{
    internal abstract class ADataProvider<TData>
    {
        public TData Provide()
        {
            try
            {
                return this.ProvideOrThrowException();
            }
            catch (Exception)
            {
                return this.ProvideDefault();
            }
        }

        protected abstract TData ProvideOrThrowException();

        protected virtual TData ProvideDefault()
        {
            return default;
        }
    }
}

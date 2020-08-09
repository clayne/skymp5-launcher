// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aero.Store
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace Yandex.Metrica.Aero
{
    internal static class Store
    {
        private static readonly object GlobalLock = new object();
        private static readonly Dictionary<Type, object> LocalLocks = new Dictionary<Type, object>();
        public static readonly Dictionary<Type, object> Container = new Dictionary<Type, object>();

        public static TItem Get<TItem>(params object[] constructorArgs) where TItem : class
        {
            Type key = typeof(TItem);
            if (Store.Container.TryGetValue(key, out object obj1))
                return (TItem)obj1;
            object obj2;
            lock (Store.GlobalLock)
            {
                if (!Store.LocalLocks.TryGetValue(key, out obj2))
                    Store.LocalLocks.Add(key, obj2 = new object());
            }
            lock (obj2)
            {
                if (!Store.Container.TryGetValue(key, out obj1))
                    Store.Container.Add(key, obj1 = Store.ReviveItem<TItem>(constructorArgs));
            }
            return (TItem)obj1;
        }

        private static TItem ReviveItem<TItem>(params object[] constructorArgs) where TItem : class
        {
            TItem obj = Memory.ActiveBox.Revive<TItem>((string)null, constructorArgs);
            if (!(obj is IExposable exposable))
                return obj;
            exposable.Expose();
            return obj;
        }

        public static void Snapshot()
        {
            ((IEnumerable<object>)Store.Container.Values.ToArray<object>()).ForEach<object>((Action<object>)(i => Memory.ActiveBox.Keep<object>(i, (string)null)));
        }

        public static void Snapshot(this object item)
        {
            Memory.ActiveBox.Keep<object>(item, (string)null);
        }
    }
}

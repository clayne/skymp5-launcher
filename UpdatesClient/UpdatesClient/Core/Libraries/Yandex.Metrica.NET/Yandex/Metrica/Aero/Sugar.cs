// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Aero.Sugar
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yandex.Metrica.Aero
{
  internal static class Sugar
  {
    public static T Of<T>(this object o)
    {
      return (T) o;
    }

    public static T As<T>(this object o) where T : class
    {
      return o as T;
    }

    public static bool Is<T>(this object o)
    {
      return o is T;
    }

    public static bool IsNull(this object o)
    {
      return o == null;
    }

    public static bool IsNotNull(this object o)
    {
      return o != null;
    }

    public static bool IsNullOrEmpty(this string value)
    {
      return string.IsNullOrEmpty(value);
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
      return collection == null || !collection.Any<T>();
    }

    public static StringBuilder Append<T>(this StringBuilder builder, params T[] args)
    {
      foreach (T obj in args)
        builder.Append((object) obj);
      return builder;
    }

    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
      foreach (T obj in collection)
        action(obj);
    }

    public static TException Try<TException>(Action action) where TException : Exception
    {
      try
      {
        action();
        return default (TException);
      }
      catch (TException ex)
      {
        return ex;
      }
    }

    public static Exception Catch<TException>(
      this Exception exception,
      Action<Exception> action)
      where TException : Exception
    {
      if ((object) exception.As<TException>() != null)
        action(exception);
      return exception;
    }

    public static Exception Try(Action action)
    {
      return Sugar.Try<Exception>(action);
    }

    public static Exception Catch(this Exception e, Action<Exception> action)
    {
      return e.Catch<Exception>(action);
    }

    public static void Finally(this Exception e, Action action)
    {
      action();
    }

    public static TResult With<TSource, TResult>(this TSource source, Func<TSource, TResult> action) where TSource : class
    {
      return (object) source != null ? action(source) : default (TResult);
    }

    public static TSource Do<TSource>(this TSource source, Action<TSource> action) where TSource : class
    {
      if ((object) source != null)
        action(source);
      return source;
    }

    public static IEnumerable<T> Turn<T>(this IList<T> items, int skip, int turnsCount = 0)
    {
      bool flag = skip < 0;
      int count = items.Count;
      skip = flag ? count + skip : skip;
      int take = turnsCount == 0 ? (flag ? -skip - 1 : count - skip) : count * turnsCount;
      return items.Ring<T>(skip, take);
    }

    public static IEnumerable<T> Ring<T>(this IList<T> items, int skip, int take)
    {
      bool reverse = take < 0;
      int count = ((ICollection<T>) items).Count;
      skip = skip < 0 ? count + skip : skip;
      skip = skip < count ? skip : skip % count;
      take = reverse ? -take : take;
      for (int i = 0; i < take; ++i)
      {
        int num1 = i < count ? i : i % count;
        int num2 = reverse ? skip - num1 : skip + num1;
        int num3 = num2 < 0 ? count + num2 : num2;
        yield return items[num3 < count ? num3 : num3 % count];
      }
    }

    public static IEnumerable<T> SkipByRing<T>(this IEnumerable<T> source, int count)
    {
      int originalCount = 0;
      bool flag = count < 0;
      count = flag ? -count : count;
      source = flag ? source.Reverse<T>() : source;
label_4:
      if (originalCount > 0)
        count %= originalCount;
      foreach (T obj in source)
      {
        ++originalCount;
        if (count > 0)
          --count;
        else
          yield return obj;
      }
      if (count != 0)
        goto label_4;
    }

    public static IEnumerable<T> TakeByRing<T>(this IEnumerable<T> source, int count)
    {
      bool flag = count < 0;
      count = flag ? -count : count;
      source = flag ? source.Reverse<T>() : source;
label_4:
      foreach (T obj in source)
      {
        if (count > 0)
        {
          --count;
          yield return obj;
        }
      }
      if (count != 0)
        goto label_4;
    }

    public static IEnumerable<T> SliceByRing<T>(
      this IEnumerable<T> source,
      int skipCount,
      int takeCount)
    {
      int originalCount = 0;
      bool flag1 = skipCount < 0;
      bool flag2 = takeCount < 0;
      skipCount = flag1 ? -skipCount : skipCount;
      takeCount = flag2 ? -takeCount : takeCount;
      source = flag2 ? source.Reverse<T>() : source;
      if (flag1 ^ flag2)
      {
        int num = source.Count<T>();
        skipCount = num - skipCount % num;
      }
label_5:
      if (originalCount > 0)
        skipCount %= originalCount;
      foreach (T obj in source)
      {
        ++originalCount;
        if (skipCount > 0)
          --skipCount;
        else if (takeCount > 0)
        {
          --takeCount;
          yield return obj;
        }
      }
      if (takeCount != 0)
        goto label_5;
    }
  }
}

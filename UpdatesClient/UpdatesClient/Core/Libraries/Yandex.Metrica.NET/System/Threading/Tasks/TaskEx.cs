// Decompiled with JetBrains decompiler
// Type: System.Threading.Tasks.TaskEx
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

namespace System.Threading.Tasks
{
  internal static class TaskEx
  {
    public static async Task Run(Action action)
    {
      await Task.Run(action);
    }

    public static async Task Delay(TimeSpan delay)
    {
      await Task.Delay(delay);
    }

    public static async Task Delay(TimeSpan delay, CancellationToken token)
    {
      await Task.Delay(delay, token);
    }

    public static async Task<TResult> FromResult<TResult>(TResult result)
    {
      return await Task.FromResult<TResult>(result);
    }
  }
}

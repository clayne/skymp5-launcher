// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.Lifecycler
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica
{
  internal class Lifecycler : IExposable, ILifecycler
  {
    ~Lifecycler()
    {
      this.End((object) this, EventArgs.Empty);
    }

    public Lifecycler()
    {
      this.Start((object) this, EventArgs.Empty);
    }

    public void Expose()
    {
      TaskScheduler.UnobservedTaskException += (EventHandler<UnobservedTaskExceptionEventArgs>) ((sender, args) => this.UnhandledException((object) this, (EventArgs) args));
      AppDomain.CurrentDomain.UnhandledException += (UnhandledExceptionEventHandler) ((sender, args) => this.UnhandledException(sender, (EventArgs) args));
    }

    public event EventHandler End = (sender, args) => {};

    public event EventHandler Start = (sender, args) => {};

    public event EventHandler Resume = (sender, args) => {};

    public event EventHandler Suspend = (sender, args) => {};

    public event EventHandler UnhandledException = (sender, args) => {};

    public bool IsBackgroundTask
    {
      get
      {
        return false;
      }
    }
  }
}

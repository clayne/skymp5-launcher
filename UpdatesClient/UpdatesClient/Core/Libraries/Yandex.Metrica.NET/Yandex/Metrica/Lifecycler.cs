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
            this.End((object)this, EventArgs.Empty);
        }

        public Lifecycler()
        {
            this.Start((object)this, EventArgs.Empty);
        }

        public void Expose()
        {
            TaskScheduler.UnobservedTaskException += (EventHandler<UnobservedTaskExceptionEventArgs>)((sender, args) => this.UnhandledException((object)this, (EventArgs)args));
            AppDomain.CurrentDomain.UnhandledException += (UnhandledExceptionEventHandler)((sender, args) => this.UnhandledException(sender, (EventArgs)args));
        }

        public event EventHandler End = (sender, args) => { };

        public event EventHandler Start = (sender, args) => { };

        public event EventHandler Resume = (sender, args) => { };

        public event EventHandler Suspend = (sender, args) => { };

        public event EventHandler UnhandledException = (sender, args) => { };

        public bool IsBackgroundTask
        {
            get
            {
                return false;
            }
        }
    }
}

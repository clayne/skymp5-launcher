using System;

namespace Yandex.Metrica.Patterns
{
    internal interface ILifecycler
    {
        event EventHandler End;

        event EventHandler Start;

        event EventHandler Resume;

        event EventHandler Suspend;

        event EventHandler UnhandledException;

        bool IsBackgroundTask { get; }
    }
}

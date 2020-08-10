using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;
using Yandex.Metrica.Patterns;

namespace Yandex.Metrica
{
    [DataContract]
    internal class LiteMetricaService : LiteMetricaCore
    {
        private CancellationTokenSource _tokenSource;

        [DataMember]
        public DateTime? LastFlushTime { get; set; }

        ~LiteMetricaService()
        {
            if (this.ActiveSessionLock == null)
                return;
            DateTime? lastLullTime = Config.Global.LastLullTime;
            if (lastLullTime.HasValue)
            {
                DateTime? nullable = lastLullTime;
                DateTime? lastWakeTime = Config.Global.LastWakeTime;
                if ((nullable.HasValue & lastWakeTime.HasValue ? (nullable.GetValueOrDefault() < lastWakeTime.GetValueOrDefault() ? 1 : 0) : 0) == 0)
                    return;
            }
            this.Lull();
        }

        public override void Expose()
        {
            base.Expose();
            this.Subscribe((ILifecycler)Store.Get<Lifecycler>());
            Task.Factory.StartNew(new Action(this.Postman));
        }

        private void Subscribe(ILifecycler lifecycler)
        {
            lifecycler.End += (EventHandler)((sender, args) => this.Lull());
            lifecycler.Resume += (EventHandler)((sender, args) => this.Wake(false, false));
            lifecycler.Suspend += (EventHandler)((sender, args) => this.Lull());
            lifecycler.UnhandledException += (EventHandler)((sender, args) =>
           {
               if (Adapter.IsInternalException((object)args))
                   return;
               if (Config.Global.CrashTracking)
               {
                   Config.Global.CrashTracking = false;
                   this.Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_CRASH, Adapter.ExtractData((object)args), (string)null, (string)null));
                   Config.Global.CrashTracking = true;
                   Store.Snapshot();
               }
               else
                   this.Lull();
           });
        }

        public override void TriggerForcedSend()
        {
            base.TriggerForcedSend();
            this._tokenSource?.Cancel();
        }

        private async Task Wait(TimeSpan delay)
        {
            try
            {
                if (this._tokenSource == null)
                {
                    await TaskEx.Delay(delay);
                }
                else
                {
                    if (this._tokenSource.IsCancellationRequested)
                        return;
                    await TaskEx.Delay(delay, this._tokenSource.Token);
                }
            }
            catch (Exception)
            {
            }
        }

        private async void Postman()
        {
            LiteMetricaService liteMetricaService = this;
            int fails = 0;
            while (true)
            {
                await ServiceData.WaitExposeAsync();
                try
                {
                    liteMetricaService._tokenSource = new CancellationTokenSource();
                    lock (liteMetricaService.PauseLock)
                    {
                        if (!liteMetricaService.IsPaused)
                            liteMetricaService.ActiveSession.LastUpdateTimestamp = new ulong?(DateTime.UtcNow.ToUnixTime());
                    }
                    int num;
                    if (!liteMetricaService.ForceSend && liteMetricaService.ReportedEventsCount < Config.Global.FlushThresholdEventsCounts && liteMetricaService.LastFlushTime.HasValue)
                    {
                        DateTime utcNow = DateTime.UtcNow;
                        DateTime? lastFlushTime = liteMetricaService.LastFlushTime;
                        TimeSpan? nullable = lastFlushTime.HasValue ? new TimeSpan?(utcNow - lastFlushTime.GetValueOrDefault()) : new TimeSpan?();
                        TimeSpan thresholdTimeout = Config.Global.FlushThresholdTimeout;
                        num = nullable.HasValue ? (nullable.GetValueOrDefault() > thresholdTimeout ? 1 : 0) : 0;
                    }
                    else
                        num = 1;
                    if (num != 0)
                    {
                        bool? nullable1 = await liteMetricaService.Refresh();
                        bool? nullable2 = liteMetricaService.Flush();
                        if (nullable2.HasValue && nullable2.Value)
                        {
                            liteMetricaService.LastFlushTime = new DateTime?(DateTime.UtcNow);
                            liteMetricaService.ReportedEventsCount = 0;
                            liteMetricaService.ForceSend = false;
                            fails = 0;
                        }
                        if (nullable2.HasValue)
                            liteMetricaService.Snapshot();
                        if (nullable2.HasValue && !nullable2.Value)
                            ++fails;
                        if (fails > 6)
                            fails = 6;
                        if (fails > 0)
                            await liteMetricaService.Wait(TimeSpan.FromSeconds(Math.Pow(2.0, (double)fails)));
                    }
                    await liteMetricaService.Wait(Config.Global.DispatchPeriod);
                }
                catch (Exception)
                {
                }
            }
        }
    }
}

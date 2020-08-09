// Decompiled with JetBrains decompiler
// Type: Yandex.Metrica.LiteMetricaCore
// Assembly: Yandex.Metrica.NET, Version=3.5.1.0, Culture=neutral, PublicKeyToken=21e4d3bd28ff137d
// MVID: 30D77F94-06C4-410D-9A5A-E6909B7FCAB1
// Assembly location: C:\Users\Felldoh\source\repos\skyrim-multiplayer\updates-client\UpdatesClient\UpdatesClient\bin\Debug\Yandex.Metrica.NET.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aides;
using Yandex.Metrica.Models;

namespace Yandex.Metrica
{
    [DataContract]
    internal class LiteMetricaCore : IExposable
    {
        protected object ActiveSessionLock;
        protected object PauseLock;

        public bool IsActivated
        {
            get
            {
                return Config.Global.ApiKey != Guid.Empty;
            }
        }

        public bool IsPaused { get; private set; }

        [DataMember]
        protected SessionModel ActiveSession { get; set; }

        [DataMember]
        protected List<SessionModel> CompletedSessions { get; set; }

        [DataMember]
        protected List<ReportPackage> ReportPackages { get; set; }

        [DataMember]
        public int ReportedEventsCount { get; set; }

        [DataMember]
        public bool ForceSend { get; set; }

        public virtual async void Expose()
        {
            this.ActiveSessionLock = new object();
            this.PauseLock = new object();
            this.ActiveSession = this.ActiveSession ?? LiteMetricaCore.CreateSession();
            this.ReportPackages = this.ReportPackages ?? new List<ReportPackage>();
            this.CompletedSessions = this.CompletedSessions ?? new List<SessionModel>();
            SessionModel sessionModel = this.ActiveSession;
            string str = this.ActiveSession.ReportParameters;
            if (str == null)
                str = "".GlueGetList(await ServiceData.GetReportParameters(), true);
            sessionModel.ReportParameters = str;
            sessionModel = (SessionModel)null;
            this.IsPaused = true;
        }

        public virtual void TriggerForcedSend()
        {
            this.ForceSend = true;
        }

        public async void Wake(bool activateApiKey = false, bool forceWake = false)
        {
            if (!this.IsActivated)
                return;
            bool flag1 = false;
            lock (this.PauseLock)
            {
                bool isPaused = this.IsPaused;
                this.IsPaused = false;
                DateTime? lastLullTime = Config.Global.LastLullTime;
                int num;
                if (lastLullTime.HasValue)
                {
                    DateTime utcNow = DateTime.UtcNow;
                    DateTime? nullable1 = lastLullTime;
                    TimeSpan? nullable2 = nullable1.HasValue ? new TimeSpan?(utcNow - nullable1.GetValueOrDefault()) : new TimeSpan?();
                    TimeSpan sessionTimeout = Config.Global.SessionTimeout;
                    num = nullable2.HasValue ? (nullable2.GetValueOrDefault() > sessionTimeout ? 1 : 0) : 0;
                }
                else
                    num = 0;
                bool flag2 = num != 0;
                if (!forceWake)
                {
                    if (!(isPaused & flag2))
                        goto label_11;
                }
                flag1 = true;
            }
            label_11:
            if (!flag1)
                return;
            SessionModel sessionModel = this.StartSession(activateApiKey);
            Dictionary<string, object> reportParameters = await ServiceData.GetReportParameters();
            sessionModel.ReportParameters = "".GlueGetList(reportParameters, true);
            sessionModel = (SessionModel)null;
            await this.ReportIdentityEvent();
        }

        public void Lull()
        {
            if (!this.IsActivated)
                return;
            lock (this.PauseLock)
            {
                this.IsPaused = true;
                this.PauseSession(new ulong?());
                Config.Global.LastLullTime = new DateTime?(DateTime.UtcNow);
            }
            Store.Snapshot();
        }

        public bool? Flush()
        {
            if (this.CompletedSessions.Count == 0 && this.ActiveSession.events.Count == 0 && this.ReportPackages.Count == 0)
                return new bool?();
            SessionModel activeSession;
            lock (this.ActiveSessionLock)
            {
                activeSession = this.ActiveSession;
                SessionModel sessionModel = new SessionModel
                {
                    id = this.ActiveSession.id,
                    events = new List<ReportMessage.Session.Event>(),
                    EventCounter = activeSession.EventCounter,
                    session_desc = activeSession.session_desc,
                    ReportParameters = activeSession.ReportParameters,
                    LastUpdateTimestamp = activeSession.LastUpdateTimestamp,
                    LastEventTimestamp = activeSession.LastEventTimestamp,
                    LastEventType = activeSession.LastEventType
                };
                this.ActiveSession = sessionModel;
            }
            return this.FlushCompletedSessions(activeSession);
        }

        private bool? FlushCompletedSessions(SessionModel sourceSession)
        {
            lock (this.CompletedSessions)
            {
                if (sourceSession.events.Count > 0)
                    this.CompletedSessions.Add(sourceSession);
                this.CompletedSessions.Where<SessionModel>((Func<SessionModel, bool>)(s => s.ReportParameters == null)).ForEach<SessionModel>((Action<SessionModel>)(async s =>
              {
                  SessionModel sessionModel = s;
                  Dictionary<string, object> reportParameters = await ServiceData.GetReportParameters();
                  sessionModel.ReportParameters = "".GlueGetList(reportParameters, true);
                  sessionModel = (SessionModel)null;
              }));
                List<SessionModel> list = this.CompletedSessions.Where<SessionModel>((Func<SessionModel, bool>)(s => !s.AsyncLocationLock)).ToList<SessionModel>();
                this.ReportPackages.AddRange((IEnumerable<ReportPackage>)list.ToReportPackages());
                list.ForEach((Action<SessionModel>)(s => this.CompletedSessions.Remove(s)));
                if (this.ReportPackages.Count == 0)
                    return new bool?(false);
                foreach (ReportPackage package in this.ReportPackages.ToArray())
                {
                    if (!Config.Global.OfflineMode && LiteMetricaCore.TryPostOrIgnore(package))
                    {
                        this.ReportPackages.Remove(package);
                        package.Fade();
                    }
                }
                this.RemoveOverflowedPackages(this.ReportPackages);
                this.ReportPackages.Where<ReportPackage>((Func<ReportPackage, bool>)(p => !p.Exists())).ForEach<ReportPackage>((Action<ReportPackage>)(p => p.Keep()));
                return new bool?(this.ReportPackages.Count == 0);
            }
        }

        private static bool TryPostOrIgnore(ReportPackage package)
        {
            try
            {
                if (package.Length <= 0L)
                    throw new Exception("Empty package");
                HttpResponseMessage result = package.PostAsync().Result;
                return result != null && (result.IsSuccessStatusCode || !LiteMetricaCore.IsValidRequestByStatusCode(result.StatusCode));
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static bool IsValidRequestByStatusCode(HttpStatusCode code)
        {
            return code != HttpStatusCode.BadRequest && code != HttpStatusCode.RequestEntityTooLarge && code != HttpStatusCode.RequestUriTooLong;
        }

        public void RemoveOverflowedPackages(List<ReportPackage> packages)
        {
            while (packages.Count > 0)
            {
                long num = packages.Aggregate<ReportPackage, long>(0L, (Func<long, ReportPackage, long>)((size, package) => size + package.Length));
                long maxCacheSize = Config.Global.MaxCacheSize;
                if (maxCacheSize <= 0L || num < maxCacheSize)
                    break;
                packages[0].Fade();
                packages.RemoveAt(0);
            }
        }

        protected async Task<bool?> Refresh()
        {
            int num = DateTime.UtcNow - Config.Global.StartupTimestamp < Config.Global.StartupExpirationTimeSpan ? 1 : 0;
            bool flag = Critical.IsUuidRequired() || Critical.IsDeviceIdRequired() || string.IsNullOrWhiteSpace(Config.Global.ReportUrl);
            if (num != 0 && !flag)
                return new bool?();
            if (!await LiteClient.RefreshStartupAsync())
                return new bool?(false);
            Config.Global.StartupTimestamp = DateTime.UtcNow;
            Config.Global.Snapshot();
            return new bool?(true);
        }

        private async Task ReportIdentityEvent()
        {
            LiteMetricaCore liteMetricaCore = this;
            if (!liteMetricaCore.IsActivated)
                return;
            bool flag;
            lock (liteMetricaCore.ActiveSessionLock)
            {
                if (liteMetricaCore.ActiveSession == null || liteMetricaCore.ActiveSession.EventCounter == 0UL)
                    return;
                flag = DateTime.UtcNow - Config.Global.IdentityTimestamp >= Config.Global.IdentitySendInterval;
                if (flag)
                    Config.Global.IdentityTimestamp = DateTime.UtcNow;
            }
            if (!flag)
                return;
            await ServiceData.WaitExposeAsync();
            byte[] bytes = Encoding.UTF8.GetBytes(ServiceData.DeviceFingerprint);
            liteMetricaCore.Report(EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_IDENTITY, bytes, (string)null, (string)null));
            liteMetricaCore.TriggerForcedSend();
        }

        private SessionModel StartSession(bool isFirstSession = false)
        {
            SessionModel activeSession;
            lock (this.ActiveSessionLock)
            {
                this.EnsureActiveSessionFinished();
                if (this.ActiveSession != null && this.ActiveSession.EventCounter > 0UL)
                {
                    lock (this.CompletedSessions)
                        this.CompletedSessions.Add(this.ActiveSession);
                }
                this.ActiveSession = LiteMetricaCore.CreateSession();
                ReportMessage.Session.Event[] eventArray;
                if (!isFirstSession)
                    eventArray = new ReportMessage.Session.Event[1]
                    {
            EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_START, (byte[]) null, (string) null, (string) null)
                    };
                else
                    eventArray = new ReportMessage.Session.Event[3]
                    {
            EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_FIRST, (byte[]) null, (string) null, (string) null),
            EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_START, (byte[]) null, (string) null, (string) null),
            EventFactory.Create(Config.Global.HandleFirstActivationAsUpdate ? ReportMessage.Session.Event.EventType.EVENT_UPDATE : ReportMessage.Session.Event.EventType.EVENT_INIT, (byte[]) null, (string) null, (string) null)
                    };
                this.Report(eventArray);
                Config.Global.LastWakeTime = new DateTime?(DateTime.UtcNow);
                activeSession = this.ActiveSession;
            }
            this.TriggerForcedSend();
            return activeSession;
        }

        private void PauseSession(ulong? pauseTimestamp = null)
        {
            lock (this.ActiveSessionLock)
            {
                if (this.ActiveSession == null || this.ActiveSession.EventCounter == 0UL)
                    return;
                ulong? nullable = pauseTimestamp;
                this.Report((ulong)(nullable.HasValue ? (long)nullable.GetValueOrDefault() : (long)DateTime.UtcNow.ToUnixTime()), EventFactory.Create(ReportMessage.Session.Event.EventType.EVENT_ALIVE, (byte[])null, (string)null, (string)null));
            }
        }

        private void EnsureActiveSessionFinished()
        {
            lock (this.ActiveSessionLock)
            {
                if (this.ActiveSession == null || this.ActiveSession.EventCounter == 0UL)
                    return;
                ulong? nullable1 = this.ActiveSession.LastEventType;
                if (nullable1.HasValue)
                {
                    nullable1 = this.ActiveSession.LastEventType;
                    if (nullable1.Value == 7UL)
                        return;
                }
                nullable1 = this.ActiveSession.LastUpdateTimestamp;
                ulong? nullable2 = nullable1.HasValue ? nullable1 : this.ActiveSession.LastEventTimestamp;
                if (!nullable2.HasValue)
                    return;
                nullable1 = this.ActiveSession.LastEventTimestamp;
                if (nullable1.HasValue)
                {
                    nullable1 = nullable2;
                    ulong? lastEventTimestamp = this.ActiveSession.LastEventTimestamp;
                    if ((nullable1.GetValueOrDefault() < lastEventTimestamp.GetValueOrDefault() ? (nullable1.HasValue & lastEventTimestamp.HasValue ? 1 : 0) : 0) != 0)
                        nullable2 = this.ActiveSession.LastEventTimestamp;
                }
                this.PauseSession(new ulong?(nullable2.Value));
            }
        }

        private static SessionModel CreateSession()
        {
            ulong unixTime = DateTime.UtcNow.ToUnixTime();
            SessionModel sessionModel = new SessionModel
            {
                id = unixTime,
                events = new List<ReportMessage.Session.Event>(),
                session_desc = new ReportMessage.Session.SessionDesc()
                {
                    locale = Config.GetLocale(),
                    session_type = new ReportMessage.Session.SessionDesc.SessionType?(ServiceData.Lifecycler.IsBackgroundTask ? ReportMessage.Session.SessionDesc.SessionType.SESSION_BACKGROUND : ReportMessage.Session.SessionDesc.SessionType.SESSION_FOREGROUND),
                    start_time = new ReportMessage.Time()
                    {
                        timestamp = unixTime,
                        time_zone = (int)DateTimeOffset.Now.Offset.TotalSeconds
                    }
                }
            };
            return sessionModel;
        }

        public void Report(params ReportMessage.Session.Event[] items)
        {
            this.Report(DateTime.UtcNow.ToUnixTime(), items);
        }

        public void Report(ulong timestamp, params ReportMessage.Session.Event[] items)
        {
            if (Config.Global.ApiKey == Guid.Empty)
                throw new ArgumentException("ApiKey is empty");
            lock (this.ActiveSessionLock)
            {
                this.ActiveSession.AggregateEvents(timestamp, items);
                this.ReportedEventsCount += items.Length;
            }
        }
    }
}

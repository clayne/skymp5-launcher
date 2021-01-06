using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Yandex.Metrica.Aero;
using Yandex.Metrica.Aero.Specific;
using Yandex.Metrica.Models;

namespace Yandex.Metrica.Legacy
{
    internal static class LegacyManager
    {
        private static readonly string[] LegacyKeys = new string[2]
        {
      "Yandex.Metrica.Config",
      "Yandex.Metrica.Sessions"
        };
#pragma warning disable IDE0051 // Удалите неиспользуемые закрытые члены
        private const string LegacyConfigFileName = "Yandex.Metrica.Config";
#pragma warning restore IDE0051 // Удалите неиспользуемые закрытые члены

        public static LegacyManager.MigrationData Data { get; private set; }

        public static async Task CompleteMigration()
        {
            if (Memory.ActiveBox.Check<LegacyManager.MigrationData>((string)null))
            {
                LegacyManager.Data = Memory.ActiveBox.Revive<LegacyManager.MigrationData>((string)null);
            }
            else
            {
                try
                {
                    Config config = await new IsolatedStreamStorage<Config>((IStreamSerializer<Config>)new ConfigProtoSerializer()).ReadAsync("Yandex.Metrica.Config");
                    Guid guid = new Guid(config.ApiKey);
                    if (!Yandex.Metrica.Models.Config.Global.KnownKeys.Contains(guid))
                        Yandex.Metrica.Models.Config.Global.KnownKeys.Add(guid);
                    Critical.SetUuid(config.UUID);
                    Memory.ActiveBox.Keep<Yandex.Metrica.Models.Config>(Yandex.Metrica.Models.Config.Global, (string)null);
                    Yandex.Metrica.Models.Config.Global.CrashTracking = config.ReportCrashesEnabled;
                    Yandex.Metrica.Models.Config.Global.LocationTracking = config.TrackLocationEnabled;
                    Yandex.Metrica.Models.Config.Global.CustomAppVersion = new Version(config.CustomAppVersion);
                }
                catch (Exception)
                {
                }
                Memory memory = new Memory((IStorage)new KeyFileStorage(), "{0}", true, "  ");
                bool flag = ((IEnumerable<string>)LegacyManager.LegacyKeys).Any<string>((Func<string, bool>)(k => memory.Check<object>(k)));
                LegacyManager.Data = new LegacyManager.MigrationData()
                {
                    MigratedFromVersion2 = flag,
                    IsCleaned = !flag
                };
                Memory.ActiveBox.Keep<LegacyManager.MigrationData>(LegacyManager.Data, (string)null);
            }
            if (LegacyManager.Data.IsCleaned)
                return;
            LegacyManager.Data.IsCleaned = LegacyManager.Clean();
            if (!LegacyManager.Data.IsCleaned)
                return;
            Memory.ActiveBox.Keep<LegacyManager.MigrationData>(LegacyManager.Data, (string)null);
        }

        private static bool Clean()
        {
            try
            {
                Memory memory = new Memory((IStorage)new KeyFileStorage(), "{0}", true, "  ");
                ((IEnumerable<string>)LegacyManager.LegacyKeys).Where<string>((Func<string, bool>)(key => memory.Check<object>(key))).ForEach<string>((Action<string>)(k => memory.Destroy<object>(k)));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [DataContract]
        internal class MigrationData
        {
            [DataMember]
            public bool MigratedFromVersion2 { get; set; }

            [DataMember]
            public bool IsCleaned { get; set; }
        }
    }
}

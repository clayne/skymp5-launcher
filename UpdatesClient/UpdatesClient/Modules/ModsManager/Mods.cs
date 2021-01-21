using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.ModsManager.Models;

namespace UpdatesClient.Modules.ModsManager
{
    public static class Mods
    {
        private static string Cache { get => Settings.PathToSkyrimMods + "\\Cache\\"; }
        private static string Tmp { get => Settings.PathToSkyrimMods + "\\Tmp\\"; }
        private static string List { get => Settings.PathToSkyrimMods + "\\mods.json"; }

        private static ModsModel mods = new ModsModel();

        public static void EnableMod()
        {
            //TODO: создание ссылки
        }

        public static void DisableMod()
        {
            //TODO: удаление ссылки
        }

        public static void AddMod(ModModel mod)
        {
            //TODO: создание директории
            //? TODO: распаковка мода
            //TODO: просчет хешей
        }

    }
}

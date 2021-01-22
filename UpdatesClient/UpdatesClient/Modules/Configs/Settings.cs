using Newtonsoft.Json;
using Security.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs.Models;

namespace UpdatesClient.Modules.Configs
{
    internal class Settings
    {
        private static SettingsFileModel model;

        public static readonly string VersionFile = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        public static readonly string VersionAssembly = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static bool Loaded { get; private set; } = false;

        #region Paths
        public static readonly string PathToLocal = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\UpdatesClient\\";
        public static readonly string PathToLocalSkyrim = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Skyrim Special Edition\\";
        public static readonly string PathToLocalTmp = $"{PathToLocal}tmp\\";
        public static readonly string PathToLocalDlls = $"{PathToLocal}dlls\\";
        public static readonly string PathToSettingsFile = $"{PathToLocal}{VersionAssembly}.json";
        public static readonly string PathToSavedServerList = $"{PathToLocalTmp}\\Servers.json";
        #endregion

        #region Skyrim
        public static string PathToSkyrim { get => model.PathToSkyrim; set => model.PathToSkyrim = value; }
        public static string PathToSkympClientSettings => $"{PathToSkyrim}\\Data\\Platform\\Plugins\\skymp5-client-settings.txt";
        public static string PathToSkyrimTmp { get => PathToSkyrim + "\\tmp\\"; }
        public static string PathToSkyrimMods { get => PathToSkyrim + "\\Mods\\"; }
        #endregion

        #region Launcher
        public static string LastVersion { get => model.LastVersion; private set => model.LastVersion = value; }
        public static int LastServerID { get => model.LastServerID ?? -1; set => model.LastServerID = value; }
        public static string Locale { get => model.Locale; set => model.Locale = value; }
        public static bool? ExperimentalFunctions { get => model.ExperimentalFunctions; set => model.ExperimentalFunctions = value; }
        #endregion

        #region User
        public static int UserId { get => model.UserId ?? -1; set => model.UserId = value; }
        public static string UserName { get; set; }
        public static bool RememberMe { get; set; } = true;
        private static SecureString userToken;
        public static string UserToken
        {
            get => RememberMe ? model.UserToken : userToken;
            set { if (RememberMe) model.UserToken = value; else userToken = value; }
        }
        #endregion


        internal static void Load()
        {
            try
            {
                if (File.Exists(PathToSettingsFile))
                {
                    model = JsonConvert.DeserializeObject<SettingsFileModel>(File.ReadAllText(PathToSettingsFile));
                }
                else
                {
                    model = new SettingsFileModel();
                }
            }
            catch (Exception e)
            {
                Logger.Error("Settings_Load", e);
                model = new SettingsFileModel();
            }
            Loaded = true;
        }
        internal static void Save()
        {
            try
            {
                if (!Directory.Exists(PathToLocal)) Directory.CreateDirectory(PathToLocal);
                File.WriteAllText(PathToSettingsFile, JsonConvert.SerializeObject(model));
            }
            catch (Exception e)
            {
                Logger.Error("Settings_Save", e);
            }
        }
        internal static void Reset()
        {
            try
            {
                if (File.Exists(PathToSettingsFile)) File.Delete(PathToSettingsFile);
                model = new SettingsFileModel();
            }
            catch (Exception e)
            {
                Logger.Error("Settings_Reset", e);
            }
            Loaded = true;
        }
    }
}

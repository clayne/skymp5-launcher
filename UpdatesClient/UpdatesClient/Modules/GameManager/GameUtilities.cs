using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using UpdatesClient.Core;
using UpdatesClient.Core.Models;
using UpdatesClient.Core.Network;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;
using UpdatesClient.Modules.Downloader;
using UpdatesClient.Modules.GameManager.Models.ServerManifest;
using UpdatesClient.Modules.ModsManager;
using UpdatesClient.UI.Controllers;
using UpdatesClient.UI.Pages.MainWindow;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient.Modules.GameManager
{
    public static class GameUtilities
    {
        public static async Task Play(ServerModel server, Window window)
        {
            string adressData;
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(Settings.PathToSkympClientSettings)) && File.Exists(Settings.PathToSkympClientSettings))
                {
                    File.SetAttributes(Settings.PathToSkympClientSettings, FileAttributes.Normal);
                }

                SetServer(server);
                string adress = server.Address;
                adressData = server.AddressData;

                object gameData = await Account.GetSession(adress);
                if (gameData == null) return;
                SetSession(gameData);
            }
            catch (JsonSerializationException)
            {
                NotifyController.Show(PopupNotify.Error, Res.Error, Res.ErrorReadSkyMPSettings);
                return;
            }
            catch (JsonReaderException)
            {
                NotifyController.Show(PopupNotify.Error, Res.Error, Res.ErrorReadSkyMPSettings);
                return;
            }
            catch (UnauthorizedAccessException)
            {
                FileAttributes attr = new FileInfo(Settings.PathToSkympClientSettings).Attributes;
                Logger.Error("Play_UAException", new UnauthorizedAccessException($"UnAuthorizedAccessException: Unable to access file. Attributes: {attr}"));
                NotifyController.Show(PopupNotify.Error, Res.Error, "UnAuthorizedAccessException: Unable to access file");
                return;
            }
            catch (Exception e)
            {
                Logger.Error("Play", e);
                NotifyController.Show(PopupNotify.Error, Res.Error, e.Message);
                return;
            }

            if (!await SetMods(adressData)) return;

            try
            {
                window.Hide();
                bool crash = await GameLauncher.StartGame();
                window.Show();

                if (crash)
                {
                    Logger.ReportMetricaEvent("CrashDetected");
                    await Task.Delay(500);
                    await DebuggerUtilities.ReportDmp();
                }
            }
            catch
            {
                Logger.ReportMetricaEvent("HasNotAccess");
                window.Close();
            }
        }

        public static async Task<ServerModsManifest> GetManifest(string adress)
        {
            string serverManifest = await Net.Request($"http://{adress}/manifest.json", "GET", false, null);
            return JsonConvert.DeserializeObject<ServerModsManifest>(serverManifest);
        }

        private static async Task<bool> SetMods(string adress)
        {
            string path = DefaultPaths.PathToLocalSkyrim + "Plugins.txt";
            string content = "";

            try
            {
                await Mods.DisableAll();
                ServerModsManifest mods = Mods.CheckCore(await GetManifest(adress));
                Dictionary<string, List<(string, uint)>> needMods = mods.GetMods();

                foreach (KeyValuePair<string, List<(string, uint)>> mod in needMods)
                {
                    if (!Mods.ExistMod(mod.Key) || !Mods.CheckMod(mod.Key, mod.Value))
                    {
                        string tmpPath = Mods.GetTmpPath();
                        string desPath = tmpPath + "\\Data\\";

                        IO.CreateDirectory(desPath);
                        string mainFile = null;
                        foreach (var file in mod.Value)
                        {
                            ServerList.ShowProgressBar = true;
                            await DownloadMod(desPath + file.Item1, adress, file.Item1);
                            ServerList.ShowProgressBar = false;
                            if (mods.LoadOrder.Contains(file.Item1)) mainFile = file.Item1;
                        }
                        await Mods.AddMod(mod.Key, "", tmpPath, true, mainFile);
                    }
                    await Mods.EnableMod(Path.GetFileNameWithoutExtension(mod.Key));
                }

                foreach (var item in mods.LoadOrder)
                {
                    content += $"*{item}\n";
                }
            }
            catch (WebException)
            {
                if (NetworkSettings.CompatibilityMode)
                {
                    NotifyController.Show(PopupNotify.Normal, Res.Attempt, "Вероятно целевой сервер устарел, используется режим совместимости");
                    if (Mods.ExistMod("Farm"))
                        await Mods.OldModeEnable();
                    await Task.Delay(3000);
                    content = @"*FarmSystem.esp";
                }
                else
                {
                    NotifyController.Show(PopupNotify.Error, Res.Attempt, "Возможно целевой сервер устарел, так как не ответил на запрос");
                    return false;
                }
            }
            catch (FileNotFoundException)
            {
                NotifyController.Show(PopupNotify.Error, Res.Error, "Один или несколько модов не удалось загрузить с сервера");
                return false;
            }
            catch (Exception e)
            {
                Logger.Error("EnablerMods", e);
                NotifyController.Show(e);
                return false;
            }

            try
            {
                if (!Directory.Exists(DefaultPaths.PathToLocalSkyrim)) Directory.CreateDirectory(DefaultPaths.PathToLocalSkyrim);
                if (File.Exists(path) && File.GetAttributes(path) != FileAttributes.Normal) File.SetAttributes(path, FileAttributes.Normal);
                File.WriteAllText(path, content);
            }
            catch (UnauthorizedAccessException)
            {
                FileAttributes attr = new FileInfo(path).Attributes;
                Logger.Error("Write_Plugin_UAException", new UnauthorizedAccessException($"UnAuthorizedAccessException: Unable to access file. Attributes: {attr}"));
            }
            catch (Exception e)
            {
                Logger.Error("Write_Plugin_txt", e);
            }
            return true;
        }
        private static async Task DownloadMod(string destinationPath, string adress, string file)
        {
            string url = $"http://{adress}/{file}";
            await DownloadManager.DownloadFile(destinationPath, url, null, null);
        }
        private static void SetServer(ServerModel server)
        {
            if (!Directory.Exists(Path.GetDirectoryName(Settings.PathToSkympClientSettings)))
                Directory.CreateDirectory(Path.GetDirectoryName(Settings.PathToSkympClientSettings));

            SkympClientSettingsModel oldServer;

            if (File.Exists(Settings.PathToSkympClientSettings))
            {
                oldServer = JsonConvert.DeserializeObject<SkympClientSettingsModel>(File.ReadAllText(Settings.PathToSkympClientSettings));
            }
            else
            {
                oldServer = new SkympClientSettingsModel
                {
                    IsEnableConsole = false,
                    IsShowMe = false
                };
            }

            ServerModel newServer = server;
            if (newServer.IsSameServer(oldServer)) return;
            File.WriteAllText(Settings.PathToSkympClientSettings, JsonConvert.SerializeObject(newServer.ToSkympClientSettings(oldServer), Formatting.Indented));
            Settings.Save();
        }
        private static void SetSession(object gameData)
        {
            SkympClientSettingsModel settingsModel = JsonConvert.DeserializeObject<SkympClientSettingsModel>(File.ReadAllText(Settings.PathToSkympClientSettings));
            settingsModel.GameData = gameData;
            File.WriteAllText(Settings.PathToSkympClientSettings, JsonConvert.SerializeObject(settingsModel, Formatting.Indented));
        }
    }
}

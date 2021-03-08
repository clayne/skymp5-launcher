using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.Debugger;
using UpdatesClient.Modules.Downloader;
using UpdatesClient.UI.Controllers;
using Res = UpdatesClient.Properties.Resources;

namespace UpdatesClient.Modules.ModsManager
{
    public static class ModUtilities
    {
        public static async Task<bool> GetSKSE()
        {
            try
            {
                string url = await Net.GetUrlToSKSE();
                string destinationPath = $@"{Settings.PathToSkyrimTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

                string path = Mods.GetTmpPath();

                bool ok = await DownloadManager.DownloadFile(destinationPath, url, Res.DownloadingSKSE, () =>
                {
                    try
                    {
                        Unpacker.UnpackArchive(destinationPath, path, Path.GetFileNameWithoutExtension(destinationPath));
                    }
                    catch (Exception e)
                    {
                        Logger.Error("ExtractSKSE", e);
                        NotifyController.Show(e);
                    }
                }, Res.ExtractingSKSE);

                if (ok)
                {
                    await Mods.AddMod("SKSE", "SKSEHash", path, false);
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("InstallSKSE", e);
            }
            return false;
        }
        public static async Task<bool> GetRuFixConsole()
        {
            try
            {
                const string url = Net.URL_Mod_RuFix;
                string destinationPath = $@"{Settings.PathToSkyrimTmp}{url.Substring(url.LastIndexOf('/'), url.Length - url.LastIndexOf('/'))}";

                string path = Mods.GetTmpPath();

                bool ok = await DownloadManager.DownloadFile(destinationPath, url, Res.DownloadingSSERuFixConsole, () =>
                {
                    try
                    {
                        Unpacker.UnpackArchive(destinationPath, path + "\\Data");
                    }
                    catch (Exception e)
                    {
                        Logger.Error("ExtractRuFix", e);
                        NotifyController.Show(e);
                    }
                }, Res.Extracting);
                
                if (ok)
                {
                    await Mods.AddMod("RuFixConsole", "RuFixConsoleHash", path, false);
                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.Error("InstallRuFixConsole", e);
            }
            return false;
        }
        public static async Task<bool> GetClient()
        {
            (string, string) url = (null, null);
            try
            {
                url = await Net.GetUrlToClient();
            }
            catch (WebException we)
            {
                NotifyController.Show(we);
                return false;
            }

            string destinationPath = $"{Settings.PathToSkyrimTmp}client.zip";

            try
            {
                IO.DeleteFile(destinationPath);
            }
            catch (Exception e)
            {
                Logger.Error("DelClientZip", e);
            }

            string path = Mods.GetTmpPath();

            bool ok = await DownloadManager.DownloadFile(destinationPath, url.Item1, Res.DownloadingClient, () =>
            {
                try
                {
                    if (Unpacker.UnpackArchive(destinationPath, path, "client"))
                    {
                        NotifyController.Show(PopupNotify.Normal, Res.InstallationCompleted, Res.HaveAGG);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Extract", e);
                    NotifyController.Show(e);
                }
            }, Res.ExtractingClient, url.Item2);

            if (ok)
            {
                await Mods.AddMod("SkyMPCore", url.Item2, path, false);
                await Mods.EnableMod("SkyMPCore");

                return true;
            }
            return false;
        }
    }
}

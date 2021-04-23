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
        public static async Task<bool> InstallSKSE()
        {
            try
            {
                if (!Mods.ExistMod("SKSE"))
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
                    }
                }

                await Mods.EnableMod("SKSE");
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("InstallSKSE", e);
                return false;
            }
        }
        public static async Task<bool> InstallRuFixConsole()
        {
            try
            {
                if (!Mods.ExistMod("RuFixConsole"))
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
                    }
                }

                await Mods.EnableMod("RuFixConsole");
                return true;
            }
            catch (Exception e)
            {
                Logger.Error("InstallRuFixConsole", e);
                return false;
            }
        }
        public static async Task<bool> InstallClient()
        {
            if (!Mods.ExistMod("SkyMPCore"))
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
                        Unpacker.UnpackArchive(destinationPath, path, "client");
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
                    return true;
                }
            }
            await Mods.EnableMod("SkyMPCore");
            return true;
        }

        public static async Task<bool> ActivateCoreMod()
        {
            try
            {
                if (Mods.ExistMod("SKSE")) await Mods.EnableMod("SKSE");
                if (Mods.ExistMod("RuFixConsole")) await Mods.EnableMod("RuFixConsole");
                if (Mods.ExistMod("SkyMPCore")) await Mods.EnableMod("SkyMPCore");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

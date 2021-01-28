using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Core;
using UpdatesClient.Core.Helpers;
using UpdatesClient.Core.Models.ServerManifest;
using UpdatesClient.Modules.Configs;
using UpdatesClient.Modules.ModsManager.Models;
using UpdatesClient.Modules.SelfUpdater;

namespace UpdatesClient.Modules.ModsManager
{
    public static class Mods
    {
        private static string Cache { get => Settings.PathToSkyrimMods + "Cache\\"; }
        private static string Tmp { get => Settings.PathToSkyrimMods + "Tmp\\"; }
        private static string List { get => Settings.PathToSkyrimMods + "mods.json"; }

        private static ModsModel mods = new ModsModel();

        public static void Init()
        {
            IO.CreateDirectory(Settings.PathToSkyrimMods);
            mods = mods.Load<ModsModel>(List);
        }

        #region Old
        public static void OldModeEnable()
        {
            DisableMod("SkyMPCore");
            EnableMod("SkyMPCore");
        }
        #endregion 


        public static ServerModsManifest CheckCore(ServerModsManifest mods)
        {
            List<string> WhiteList = new List<string>();
            WhiteList.Add("Skyrim.esm");
            WhiteList.Add("Update.esm");
            WhiteList.Add("Dawnguard.esm");
            WhiteList.Add("HearthFires.esm");
            WhiteList.Add("Dragonborn.esm");

            var arrMods = mods.Mods.ToArray();

            foreach (ServerModModel mod in arrMods)
            {
                if (WhiteList.Contains(mod.FileName))
                {
                    string path = $"{Settings.PathToSkyrim}\\Data\\{mod.FileName}";
                    if (File.Exists(path))
                    {
                        uint lhash = Hashing.GetCRC32FromBytes(File.ReadAllBytes(path));
                        //if (mod.CRC32 == lhash)
                            mods.Mods.Remove(mod);
                    }
                }
            }
            return mods;
        }
        public static bool CheckMod(string modName, List<(string, uint)> files)
        {
            if (!ExistMod(modName)) throw new FileNotFoundException("Mod not found", modName);
            string pathToMod = Settings.PathToSkyrimMods + modName + "\\";
            ModModel mod = new ModModel();
            mod = mod.Load<ModModel>(pathToMod + "mod.json");

            bool valid = true;

            if (files.Count != mod.Files.Count) return false;

            foreach(var file in files)
            {
                if (file.Item2 != mod.Files[$"Data\\{file.Item1}"]) valid = false;
            }

            return valid;
        }

        public static bool ExistMod(string modName)
        {
            bool m = mods.Mods.Contains(modName);
            if (m)
            {
                m = Directory.Exists(Settings.PathToSkyrimMods + modName);
                if (!m)
                {
                    if (mods.EnabledMods.Contains(modName)) mods.EnabledMods.Remove(modName);
                    mods.Mods.Remove(modName);
                    mods.Save(List);
                }
            }
            return m;
        }

        public static bool IsEnableMod(string modName)
        {
            return ExistMod(modName) && mods.EnabledMods.Contains(modName);
        }

        public static void EnableMod(string modName)
        {
            if (!ExistMod(modName)) throw new FileNotFoundException("Mod not found", modName);
            if (IsEnableMod(modName)) return;

            ModModel mod = new ModModel();
            mod = mod.Load<ModModel>(Settings.PathToSkyrimMods + modName + "\\mod.json");

            foreach (string file in mod.Files.Keys)
            {
                if (!Directory.Exists($"{Settings.PathToSkyrim}\\{Path.GetDirectoryName(file)}"))
                {
                    string[] dirs = Path.GetDirectoryName(file).Split('\\');
                    string tpmdir = "";
                    foreach (string dir in dirs)
                    {
                        tpmdir += dir + "\\";
                        if (!Directory.Exists($"{Settings.PathToSkyrim}\\{tpmdir}"))
                        {
                            WinFunctions.CreateSymbolicLink($"{Settings.PathToSkyrim}\\{tpmdir}",
                                $@"{Settings.PathToSkyrimMods}{modName}\{tpmdir}", Enums.SymbolicLink.Directory);
                            break;
                        }
                    }
                }
                else if (!File.Exists($"{Settings.PathToSkyrim}\\{file}"))
                {
                    WinFunctions.CreateSymbolicLink($"{Settings.PathToSkyrim}\\{file}",
                    $@"{Settings.PathToSkyrimMods}{modName}\{file}", Enums.SymbolicLink.File);
                }
            }

            mods.EnabledMods.Add(modName);
            mods.Save(List);
        }

        public static void DisableMod(string modName)
        {
            if (!ExistMod(modName)) throw new FileNotFoundException("Mod not found", modName);
            if (!IsEnableMod(modName)) return;

            ModModel mod = new ModModel();
            mod = mod.Load<ModModel>(Settings.PathToSkyrimMods + modName + "\\mod.json");

            foreach (string file in mod.Files.Keys)
            {
                if (Directory.Exists($"{Settings.PathToSkyrim}\\{Path.GetDirectoryName(file)}"))
                {
                    string[] dirs = Path.GetDirectoryName(file).Split('\\');
                    string tpmdir = "";
                    foreach (string dir in dirs)
                    {
                        tpmdir += dir + "\\";
                        DirectoryInfo di = new DirectoryInfo($"{Settings.PathToSkyrim}\\{tpmdir}");
                        if (di.Attributes.HasFlag(FileAttributes.ReparsePoint))
                        {
                            Directory.Delete($"{Settings.PathToSkyrim}\\{tpmdir}");
                            break;
                        }
                    }
                }

                if (Directory.Exists($"{Settings.PathToSkyrim}\\{Path.GetDirectoryName(file)}")
                    && File.Exists($"{Settings.PathToSkyrim}\\{file}")
                    && File.GetAttributes($"{Settings.PathToSkyrim}\\{file}").HasFlag(FileAttributes.ReparsePoint))
                {
                    File.Delete($"{Settings.PathToSkyrim}\\{file}");
                }
            }

            mods.EnabledMods.Remove(modName);
            mods.Save(List);
        }

        public static void DisableAll(bool ignoreWL = false)
        {
            List<string> WhiteList = new List<string>();
            WhiteList.Add("SKSE");
            WhiteList.Add("SkyMPCore");
            WhiteList.Add("RuFixConsole");

            string[] enMods = mods.EnabledMods.ToArray();

            foreach (string mod in enMods)
            {
                if (!ignoreWL && WhiteList.Contains(mod)) continue;
                DisableMod(mod);
            }
        }


        public static string GetTmpPath()
        {
            string path = Tmp + Hashing.GetMD5FromText(DateTime.UtcNow.ToString());
            IO.CreateDirectory(path);
            return path;
        }

        public static void AddMod(string modName, string hash, string pathTmp, string mainFile = null)
        {
            ModModel mod = new ModModel();
            mod.Name = modName;
            mod.Hash = hash;

            if (mainFile != null)
            {
                mod.HasMainFile = true;
                mod.MainFile = mainFile;
            }

            if (ExistMod(modName))
            {
                //IO.RemoveDirectory(Settings.PathToSkyrimMods + mod.Name);
            }

            IO.RecursiveHandleFile(pathTmp, (file) =>
            {
                string filePath = file.Replace(pathTmp + "\\", "");
                uint fileHash = Hashing.GetCRC32FromBytes(File.ReadAllBytes(file));
                mod.Files.Add(filePath, fileHash);
            });

            IO.CreateDirectory(Settings.PathToSkyrimMods + mod.Name);
            IO.RecursiveCopy(pathTmp, Settings.PathToSkyrimMods + mod.Name);

            Directory.Delete(pathTmp, true);

            if (!mods.Mods.Contains(mod.Name))
            {
                mods.Mods.Add(mod.Name);
            }
            
            mod.Save(Settings.PathToSkyrimMods + mod.Name + "\\mod.json");
            mods.Save(List);
        }
    }
}

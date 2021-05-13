using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using UpdatesClient.Core;
using UpdatesClient.Modules.Recovery.Models;
using UpdatesClient.Modules.SelfUpdater;

namespace UpdatesClient.Modules.Recovery
{
    public class GameCleaner
    {
        public static void CreateGameManifest(string pathToGame)
        {
            if (File.Exists($"{pathToGame}\\SkyrimSE.exe"))
            {
                string vers = FileVersionInfo.GetVersionInfo($"{pathToGame}\\SkyrimSE.exe")?.FileVersion;
                if (!string.IsNullOrEmpty(vers))
                {
                    GameManifestModel model = new GameManifestModel
                    {
                        Version = "1.0",
                        GameVersion = vers
                    };

                    IO.RecursiveHandleFile(pathToGame, (file) =>
                    {
                        string path = file.Replace(pathToGame, "");
                        uint hash = Hashing.GetCRC32FromBytes(File.ReadAllBytes(file));
                        model.Files.Add(path, hash);
                    });

                    File.WriteAllText("game.manifest.json", JsonConvert.SerializeObject(model));
                }

            }
        }
    }
}

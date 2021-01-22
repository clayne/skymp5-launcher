using System.IO;
using UpdatesClient.Core;
using UpdatesClient.Modules.Configs;

namespace UpdatesClient.Modules.GameManager.Helpers
{
    public static class GameCleaner
    {
        private static string[] Directories = new string[] { "src", "tmp", "Data\\Interface", "Data\\meshes", "Data\\Platform", "Data\\Scripts",
            "Data\\ShaderCache", "Data\\SKSE", "Data\\textures" };
        private static string[] Files = new string[] { "README.txt", "skse64_1_5_97.dll", "skse64_loader.exe", "skse64_readme.txt",
            "skse64_steam_loader.dll", "skse64_whatsnew.txt", "version.json", "Data\\FarmSystem.esp" };

        public static void Clear()
        {
            foreach (string path in Directories)
            {
                try
                {
                    IO.RemoveDirectory(Settings.PathToSkyrim + "\\" + path);
                }
                catch { }
            }

            foreach (string path in Files)
            {
                try
                {
                    File.Delete(Settings.PathToSkyrim + "\\" + path);
                }
                catch { }
            }
        }
    }
}

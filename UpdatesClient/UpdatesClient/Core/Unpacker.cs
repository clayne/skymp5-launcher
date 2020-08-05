using System.IO;
using System.IO.Compression;

namespace UpdatesClient.Core
{
    public class Unpacker
    {
        public static bool Unpack(string file, string extractTo)
        {
            if (!File.Exists(file)) return false;

            string tmpFiles = $"\\tmp\\files\\";
            Create(tmpFiles);

            ZipFile.ExtractToDirectory(file, tmpFiles);
            CopyToDir(tmpFiles + "\\client\\", extractTo);

            Delete(tmpFiles);
            File.Delete(file);

            return true;
        }

        private static bool CopyToDir(string fromDir, string toDir)
        {
            foreach (DirectoryInfo dir in new DirectoryInfo(fromDir).GetDirectories())
            {
                Create($"{toDir}\\{dir.Name}");
                CopyToDir(dir.FullName, $"{toDir}\\{dir.Name}");
            }

            foreach (string file in Directory.GetFiles(fromDir))
            {
                string NameFile = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                File.Copy(file, $"{toDir}\\{NameFile}", true);
            }
            return true;
        }

        private static void Create(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }
        private static void Delete(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }
    }
}

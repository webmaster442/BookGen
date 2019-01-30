using BookGen.Domain;
using System.IO;
using System.Reflection;

namespace BookGen.Utilities
{
    internal static class FsUtils
    {
        public static void CopyDirectory(this FsPath sourceDirectory, FsPath TargetDir)
        {
            if (!Directory.Exists(TargetDir.ToString()))
                Directory.CreateDirectory(TargetDir.ToString());

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourceDirectory.ToString(), "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(sourceDirectory.ToString(), TargetDir.ToString()));

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourceDirectory.ToString(), "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(sourceDirectory.ToString(), TargetDir.ToString()), true);
        }

        public static void WriteFile(this FsPath target, string content)
        {
            using (var writer = File.CreateText(target.ToString()))
            {
                writer.Write(content);
            }
        }

        public static string ReadFile(this FsPath path)
        {
            if (path.IsEmbeded)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"BookGen.{path.ToString().Replace('\\', '.')}";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string result = reader.ReadToEnd();
                    }
                }
            }

            using (var reader = File.OpenText(path.ToString()))
            {
                return reader.ReadToEnd();
            }
        }
    }
}

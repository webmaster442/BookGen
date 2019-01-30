//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using System.IO;
using System.Reflection;

namespace BookGen.Utilities
{
    internal static class FsUtils
    {
        public static FsPath ToPath(this string s)
        {
            return new FsPath(s);
        }

        public static void CreateDir(this FsPath path)
        {
            Directory.CreateDirectory(path.ToString());
        }

        public static string GetName(this FsPath path)
        {
            return Path.GetFileName(path.ToString());
        }

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
            FileInfo fileInfo = new FileInfo(target.ToString());

            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

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
                var resourceName = $"BookGen.{path.ToString().Substring(6).Replace('/', '.')}";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
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
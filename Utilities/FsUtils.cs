//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Domain;
using System.IO;
using System.Text;

namespace BookGen.Utilities
{
    internal static class FsUtils
    {
        public static FsPath ToPath(this string s)
        {
            return new FsPath(s);
        }

        public static void CreateDir(this FsPath path, ILog log)
        {
            log?.Detail("Creating directory: {0}", path);
            Directory.CreateDirectory(path.ToString());
        }

        public static string GetName(this FsPath path)
        {
            return Path.GetFileName(path.ToString());
        }

        public static void CopyDirectory(this FsPath sourceDirectory, FsPath TargetDir, ILog log)
        {
            if (!Directory.Exists(TargetDir.ToString()))
                Directory.CreateDirectory(TargetDir.ToString());

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourceDirectory.ToString(), "*.*",
                SearchOption.AllDirectories))
            {
                var targetfile = newPath.Replace(sourceDirectory.ToString(), TargetDir.ToString());
                log?.Detail("Copy file: {0} to {1}", newPath, targetfile);
                File.Copy(newPath, targetfile, true);
            }
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
            using (var reader = File.OpenText(path.ToString()))
            {
                return reader.ReadToEnd();
            }
        }

        public static void ProtectDirectory(this FsPath directory)
        {
            var outp = directory.Combine("index.html");
            StringBuilder sb = new StringBuilder(4096);
            for (int i=0; i<256; i++)
            {
                sb.Append("                ");
            }
            outp.WriteFile(sb.ToString());
        }

        public static string[] GetAllFiles(this FsPath directory)
        {
            return Directory.GetFiles(directory.ToString(), "*.*", SearchOption.AllDirectories);
        }
    }
}
//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace BookGen.Core
{
    public static class FsUtils
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
            {
                log.Detail("Creating directory: {0}", TargetDir);
                Directory.CreateDirectory(TargetDir.ToString());
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourceDirectory.ToString(), "*.*",
                SearchOption.AllDirectories))
            {
                var targetfile = newPath.Replace(sourceDirectory.ToString(), TargetDir.ToString());
                log?.Detail("Copy file: {0} to {1}", newPath, targetfile);
                File.Copy(newPath, targetfile, true);
            }
        }

        public static void Copy(this FsPath source, FsPath target, ILog log)
        {
            if (!source.IsExisting)
            {
                log.Detail("Source doesn't exist, skipping: {0}");
                return;
            }

            var dir = Path.GetDirectoryName(target.ToString());
            if (!Directory.Exists(dir))
            {
                log.Detail("Creating directory: {0}", dir);
                Directory.CreateDirectory(dir);
            }

            File.Copy(source.ToString(), target.ToString());
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

        public static void WriteFile(this FsPath target, params string[] contents)
        {
            FileInfo fileInfo = new FileInfo(target.ToString());

            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

            using (var writer = File.CreateText(target.ToString()))
            {
                foreach (var content in contents)
                {
                    writer.Write(content);
                }
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

        public static FsPath GetRelativePathTo(this FsPath path, FsPath file)
        {
            string filespec = path.ToString();
            string folder = Path.GetDirectoryName(file.ToString());

            Uri pathUri = new Uri(filespec);
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);

            var ret = Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
            return new FsPath(ret);
        }

        public static void SerializeXml<T>(this FsPath path, T obj, IList<Tuple<string, string>> nslist = null)
        {
            FileInfo fileInfo = new FileInfo(path.ToString());

            if (!fileInfo.Exists)
                Directory.CreateDirectory(fileInfo.Directory.FullName);

            XmlSerializerNamespaces xnames = null;
            if (nslist != null)
            {
                xnames = new XmlSerializerNamespaces();
                foreach (var ns in nslist)
                {
                    xnames.Add(ns.Item1, ns.Item2);
                }
            }
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (var writer = File.Create(path.ToString()))
            {
                if (xnames == null)
                    xs.Serialize(writer, obj);
                else
                    xs.Serialize(writer, obj, xnames);
            }
        }

        public static FsPath GetAbsolutePathRelativeTo(this FsPath path, FsPath file)
        {
            string filespec = path.ToString();
            string folder = Path.GetDirectoryName(file.ToString());
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }

            Uri pathUri = new Uri(new Uri(folder), filespec);
            return new FsPath(pathUri.ToString().Replace("file:///", "").Replace("/", "\\"));
        }
    }
}
//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace BookGen.Editor.Services
{
    internal static class FileSystemServices
    {
        public static IEnumerable<FileItem> ListDirectory(string path, string filter = null)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                FileInfo fi = new FileInfo(file);

                if ((fi.Attributes & FileAttributes.Hidden) != 0) continue;

                if (string.IsNullOrEmpty(filter))
                {
                    yield return new FileItem
                    {
                        FullPath = file,
                        Name = fi.Name,
                        Size = fi.Length,
                        FileType = fi.Extension,
                        LastModified = fi.LastWriteTime
                    };
                }
                else if (fi.Name.Contains(filter))
                {
                    yield return new FileItem
                    {
                        FullPath = file,
                        Name = fi.Name,
                        Size = fi.Length,
                        FileType = fi.Extension,
                        LastModified = fi.LastWriteTime
                    };
                }
            }
        }

        public static IEnumerable<string> GetImagesInWorkDir()
        {
            var files = Directory.GetFiles(App.WorkFolder, "*.*", SearchOption.AllDirectories);
            var query = from file in files
                        where
                             IsImage(file)
                        select
                             file;

            return query;
        }

        private static bool IsImage(string file)
        {
            var ext = Path.GetExtension(file).ToLower();
            switch (ext)
            {
                case ".jpg":
                case ".png":
                case ".jpeg":
                case ".gif":
                    return true;
                default:
                    return false;
            }
        }

        public static IList<DirectoryItem> GetDirectories(string path)
        {
            List<DirectoryItem> ret = new List<DirectoryItem>();

            foreach (var directory in Directory.GetDirectories(path))
            {
                DirectoryInfo di = new DirectoryInfo(directory);
                if ((di.Attributes & FileAttributes.Hidden) != 0) continue;

                DirectoryItem info = new DirectoryItem
                {
                    Name = di.Name,
                    FullPath = di.FullName,
                    FileCount = Directory.GetFiles(directory).Length,
                    SubDirs = GetDirectories(directory)
                };
                ret.Add(info);
            }

            return ret;
        }

        public static void CreateFolder(string path)
        {
            var namedialog = new Dialogs.TextInput("Enter folder name");
            if (namedialog.ShowDialog() == true)
            {
                var name = Path.Combine(path, namedialog.Inputstring);
                if (!Directory.Exists(name))
                    Directory.CreateDirectory(name);
            }
        }

        public static void CreateFile(string path)
        {
            var namedialog = new Dialogs.TextInput("Enter file name");
            if (namedialog.ShowDialog() == true)
            {
                var name = Path.Combine(path, namedialog.Inputstring);
                using (var fs = File.Create(name)) { }
            }
        }

        public static void DeleteFile(string path)
        {
            if (!File.Exists(path)) return;
            var result = MessageBox.Show("Delete File? This can't be undone\n" + path, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                File.Delete(path);
            }
        }

        public static void RenameFile(string path)
        {
            if (!File.Exists(path)) return;
            var namedialog = new Dialogs.TextInput("Enter new file name");

            namedialog.Inputstring = Path.GetFileName(path);

            if (namedialog.ShowDialog() == true)
            {
                var dir = Path.GetDirectoryName(path);
                var newName = Path.Combine(dir, namedialog.Inputstring);
                File.Move(path, newName);
            }
        }
    }
}

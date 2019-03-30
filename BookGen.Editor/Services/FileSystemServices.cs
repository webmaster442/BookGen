//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Model;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace BookGen.Editor.Services
{
    internal static class FileSystemServices
    {
        public static IEnumerable<FileItem> ListDirectory(string path)
        {
            foreach (var file in Directory.GetFiles(path))
            {
                FileInfo fi = new FileInfo(file);

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

        public static IList<DirectoryItem> GetDirectories(string path)
        {
            List<DirectoryItem> ret = new List<DirectoryItem>();

            foreach (var directory in Directory.GetDirectories(path))
            {
                DirectoryItem info = new DirectoryItem
                {
                    Name = Path.GetFileName(directory),
                    FullPath = directory,
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

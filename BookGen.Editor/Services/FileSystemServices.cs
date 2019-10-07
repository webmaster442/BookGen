//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------
using BookGen.Editor.Models;
using BookGen.Editor.ServiceContracts;
using BookGen.Editor.Views.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace BookGen.Editor.Services
{
    internal class FileSystemServices: IFileSystemServices
    {
        public IEnumerable<FileItem> ListDirectory(string path, string filter = null)
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

        public IEnumerable<string> GetImagesInWorkDir(string sessiondir)
        {
            var files = Directory.GetFiles(sessiondir, "*.*", SearchOption.AllDirectories);
            var query = from file in files
                        where
                             IsImage(file)
                        select
                             file;

            return query;
        }

        private bool IsImage(string file)
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

        public IList<DirectoryItem> GetDirectories(string path)
        {
            List<DirectoryItem> ret = new List<DirectoryItem>();
            DirectoryInfo root = new DirectoryInfo(path);
            ret.Add(new DirectoryItem
            {
                Name = root.Name,
                FullPath = root.FullName,
                FileCount = Directory.GetFiles(path).Length,
                SubDirs = GetSubDirectories(path)
            });
            return ret;
        }

        public IList<DirectoryItem> GetSubDirectories(string path)
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
                    SubDirs = GetSubDirectories(directory)
                };
                ret.Add(info);
            }

            return ret;
        }

        public void CreateFolder(string path)
        {
            var namedialog = new TextInputDialog("Enter folder name");
            if (namedialog.ShowDialog() == true)
            {
                var name = Path.Combine(path, namedialog.Inputstring);
                if (!Directory.Exists(name))
                    Directory.CreateDirectory(name);
            }
        }

        public void CreateFile(string path)
        {
            var namedialog = new TextInputDialog("Enter file name");
            if (namedialog.ShowDialog() == true)
            {
                var name = Path.Combine(path, namedialog.Inputstring);
                using (var fs = File.Create(name))
                {
                    //empty
                }
            }
        }

        public void DeleteFile(string path)
        {
            if (!File.Exists(path)) return;
            var result = MessageBox.Show("Delete File? This can't be undone\n" + path, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                File.Delete(path);
            }
        }

        public void RenameFile(string path)
        {
            if (!File.Exists(path)) return;
            var namedialog = new TextInputDialog("Enter new file name");

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

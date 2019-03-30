//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Model;
using System;
using System.Collections.Generic;
using System.IO;

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

            ret.Add(new DirectoryItem
            {
                Name = "[ROOT]",
                FullPath = path,
                FileCount = Directory.GetFiles(path).Length,
                SubDirs = null
            });

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
    }
}

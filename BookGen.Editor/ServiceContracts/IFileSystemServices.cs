using BookGen.Editor.Models;
using System.Collections.Generic;

namespace BookGen.Editor.ServiceContracts
{
    internal interface IFileSystemServices
    {
        IEnumerable<FileItem> ListDirectory(string path, string filter = null);
        IEnumerable<string> GetImagesInWorkDir(string sessiondir);
        IList<DirectoryItem> GetDirectories(string path);
        IList<DirectoryItem> GetSubDirectories(string path);
        void CreateFolder(string path);
        void CreateFile(string path);
        void DeleteFile(string path);
        void RenameFile(string path);
    }
}

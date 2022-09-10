using System.Collections.Generic;
using System.IO;

namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal static class ItemViewModelFactory
    {
        public static IEnumerable<FileBrowserItemViewModel> CreateModels(string path)
        {
            foreach (var subdir in Directory.GetDirectories(path))
                yield return CreateDirectoryModel(subdir);

            foreach (var file in Directory.GetFiles(path))
                yield return CreateFileModel(file);

        }

        private static FileBrowserItemViewModel CreateFileModel(string file)
        {
            FileInfo fi = new FileInfo(file);

            return new FileBrowserItemViewModel
            {
                ModificationDate = fi.LastWriteTime,
                Extension = fi.Extension,
                FullPath = file,
                IsFolder = false,
                Size = fi.Length,
            };
        }

        private static FileBrowserItemViewModel CreateDirectoryModel(string subdir)
        {
            return new FileBrowserItemViewModel
            {
                IsFolder = true,
                FullPath = subdir,
                ModificationDate = Directory.GetLastAccessTime(subdir),
                Extension = string.Empty,
                Size = 0
            };
        }
    }
}

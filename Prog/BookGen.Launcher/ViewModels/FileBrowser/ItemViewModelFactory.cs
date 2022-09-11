using System.Collections.Generic;
using System.IO;

namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal static class ItemViewModelFactory
    {
        public static IEnumerable<FileBrowserItemViewModel> CreateModels(string path)
        {
            foreach (string subdir in Directory.GetDirectories(path))
                yield return CreateDirectoryModel(subdir);

            foreach (string file in Directory.GetFiles(path))
                yield return CreateFileModel(file);

        }

        private static FileBrowserItemViewModel CreateFileModel(string file)
        {
            var fi = new FileInfo(file);

            return new FileBrowserItemViewModel
            {
                ModificationDate = fi.LastWriteTime,
                Extension = fi.Extension,
                FullPath = file,
                IsFolder = false,
                Size = fi.Length,
                Name = fi.Name,
            };
        }

        private static FileBrowserItemViewModel CreateDirectoryModel(string subdir)
        {
            return new FileBrowserItemViewModel
            {
                Name = Path.GetFileName(subdir),
                IsFolder = true,
                FullPath = subdir,
                ModificationDate = Directory.GetLastAccessTime(subdir),
                Extension = string.Empty,
                Size = 0
            };
        }
    }
}

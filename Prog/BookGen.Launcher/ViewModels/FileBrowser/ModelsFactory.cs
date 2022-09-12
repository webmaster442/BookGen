//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal static class ModelsFactory
    {
        public static IEnumerable<FileBrowserItemViewModel> CreateItemModels(string path)
        {
            foreach (string file in Directory.GetFiles(path))
                yield return CreateFileModel(file);
        }

        public static IEnumerable<FileBrowserTreeViewModel> CreateTreeModels(string path)
        {
            yield return new FileBrowserTreeViewModel
            {
                FullPath = path,
                Name = Path.GetFileName(path),
                SubItems = GetSubItems(path),
            };
        }

        private static FileBrowserTreeViewModel[] GetSubItems(string path)
        {
            return Directory.GetDirectories(path).Select(x =>
                new FileBrowserTreeViewModel
                {
                    FullPath = x,
                    Name = Path.GetFileName(x),
                    SubItems = GetSubItems(x)
                }
            ).ToArray();
        }

        private static FileBrowserItemViewModel CreateFileModel(string file)
        {
            var fi = new FileInfo(file);

            return new FileBrowserItemViewModel
            {
                ModificationDate = fi.LastWriteTime,
                Extension = fi.Extension,
                FullPath = file,
                Size = fi.Length,
                Name = fi.Name,
            };
        }
    }
}

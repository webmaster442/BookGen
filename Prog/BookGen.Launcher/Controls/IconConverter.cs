//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Launcher.ViewModels.FileBrowser;
using System.Windows.Controls;

namespace BookGen.Launcher.Controls
{
    internal sealed class IconConverter : ConverterBase<FileBrowserItemViewModel, Canvas>
    {
        private static Canvas GetResource(string name)
        {
            return (Canvas)Application.Current.FindResource(name);
        }

        protected override Canvas ConvertToTTo(FileBrowserItemViewModel tFrom, object parameter)
        {
            return tFrom.Extension switch
            {
                ".c" or ".h" => GetResource("icon-c"),
                ".cpp" or ".hpp" => GetResource("icon-cpp"),
                ".cs" => GetResource("icon-csharp"),
                ".css" => GetResource("icon-css"),
                ".htm" or ".html" => GetResource("icon-html"),
                ".js" => GetResource("icon-js"),
                ".json" => GetResource("icon-json"),
                ".xml" => GetResource("icon-xml"),
                ".jpg" or ".jpeg" => GetResource("icon-jpg"),
                ".png" => GetResource("icon-png"),
                ".webp" or ".bmp" or ".tiff" => GetResource("icon-image"),
                ".svg" => GetResource("icon-svg"),
                ".md" => GetResource("icon-md"),
                ".doc" or ".docx" or ".xls" or ".xlsx" or ".ppt" or ".pptx" => GetResource("icon-office"),
                ".xaml" => GetResource("icon-xaml"),
                ".exe" => GetResource("icon-exe"),
                ".py" => GetResource("icon-python"),
                ".php" => GetResource("icon-php"),
                ".zip" or ".7z" or ".rar" or ".gz" or ".tar" => GetResource("icon-pack"),
                _ => GetGenericIcon(tFrom.Extension)
            };
        }

        private static Canvas GetGenericIcon(string extension)
        {
            char? first = extension?.Skip(1)?.FirstOrDefault();

            if (first == null)
                return GetResource("icon-file");

            if (first > 'a' && first < 'z')
                return GetResource($"icon-dot-{first}");

            if (first > '0' && first < '9')
                return GetResource($"icon-dot-{first}");

            return GetResource("icon-file");
        }
    }
}

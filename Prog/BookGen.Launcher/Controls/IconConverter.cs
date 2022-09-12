//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Launcher.ViewModels.FileBrowser;
using System.Windows.Controls;

namespace BookGen.Launcher.Controls
{
    internal class IconConverter : ConverterBase<FileBrowserItemViewModel, Canvas>
    {
        private static Canvas GetResource(string name)
        {
            return (Canvas)Application.Current.FindResource(name);
        }

        protected override Canvas ConvertToTTo(FileBrowserItemViewModel tFrom, object parameter)
        {
            return tFrom.Extension switch
            {
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
                _ => GetResource("icon-file"),
            };
        }
    }
}

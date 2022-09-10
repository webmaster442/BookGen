using System.Windows;
using System.Windows.Controls;

namespace BookGen.Launcher.Controls
{
    internal class IconConverter : ConverterBase<string, Canvas>
    {
        private static Canvas GetResource(string name)
        {
            return (Canvas)Application.Current.FindResource(name);
        }

        protected override Canvas ConvertToTTo(string tFrom, object parameter)
        {
            return tFrom switch
            {
                ".css" => GetResource("icon-css"),
                ".htm" or ".html" => GetResource("icon-html"),
                ".js" => GetResource("icon-js"),
                ".xml" => GetResource("icon-xml"),
                ".jpg" or ".jpeg" => GetResource("icon-jpg"),
                ".png" => GetResource("icon-png"),
                ".webp" or ".bmp" or ".tiff" => GetResource("icon-image"),
                ".svg" => GetResource("icon-svg"),
                _ => GetResource("icon-file"),
            };
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookGen.Editor.Infrastructure
{
    public class IconConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string ext)) return Binding.DoNothing;
            switch (ext.ToLower())
            {
                case ".gitignore":
                case ".gitattributes":
                case ".gitkeep":
                    return App.Current.FindResource("FileType-Git");
                case ".sh":
                case ".psh":
                case ".bat":
                case ".cmd":
                    return App.Current.FindResource("FileType-Shell");
                case ".css":
                    return App.Current.FindResource("FileType-CSS");
                case ".txt":
                case ".log":
                    return App.Current.FindResource("FileType-TXT");
                case ".jpg":
                case ".png":
                case ".jpeg":
                case ".gif":
                    return App.Current.FindResource("FileType-Image");
                case ".js":
                case ".json":
                    return App.Current.FindResource("FileType-Javascript");
                case ".md":
                    return App.Current.FindResource("FileType-Markdown");
                case ".avi":
                case ".mp4":
                case ".m4v":
                case ".webm":
                    return App.Current.FindResource("FileType-Movie");
                case ".xml":
                    return App.Current.FindResource("FileType-XML");
                case ".pdf":
                    return App.Current.FindResource("FileType-Pdf");
                case ".htm":
                case ".html":
                    return App.Current.FindResource("FileType-Html");
                default:
                    return App.Current.FindResource("FileType-Generic");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

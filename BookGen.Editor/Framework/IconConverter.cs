//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookGen.Editor.Framework
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
                    return "pack://application:,,,/BookGen.Editor;component/Icons/icons8-git-48.png";
                case ".sh":
                case ".psh":
                case ".bat":
                case ".cmd":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-console-48.png";
                case ".css":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-css-filetype-48.png";
                case ".txt":
                case ".log":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-document-48.png";
                case ".jpg":
                case ".png":
                case ".jpeg":
                case ".gif":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-image-file-48.png";
                case ".js":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-javascript-48.png";
                case ".json":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-json-48.png";
                case ".md":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-markdown-48.png";
                case ".avi":
                case ".mp4":
                case ".m4v":
                case ".webm":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-movie-48.png";
                case ".xml":
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-xml-file-48.png";
                default:
                    return "pack://application:,,,/BookGen.Editor;component/Icons/files/icons8-file-48.png";
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

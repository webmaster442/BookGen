using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

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
            switch (tFrom)
            {
                case ".css":
                    return GetResource("icon-css");
                case ".htm":
                case ".html":
                    return GetResource("icon-html");
                case ".js":
                    return GetResource("icon-js");
                case ".xml":
                    return GetResource("icon-xml");
                default:
                    return GetResource("icon-file");
            }
        }
    }
}

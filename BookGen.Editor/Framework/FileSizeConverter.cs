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
    public class FileSizeConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GetFileSize(value);
        }

        public static string GetFileSize(object value)
        {
            double val = System.Convert.ToDouble(value);
            string unit = "Byte";
            if (val > 1125899906842624)
            {
                val /= 1125899906842624;
                unit = "EiB";
            }
            else if (val > 1099511627776D)
            {
                val /= 1099511627776D;
                unit = "TiB";
            }
            else if (val > 1073741824D)
            {
                val /= 1073741824D;
                unit = "GiB";
            }
            else if (val > 1048576D)
            {
                val /= 1048576D;
                unit = "MiB";
            }
            else if (val > 1024D)
            {
                val /= 1024D;
                unit = "kiB";
            }

            return string.Format("{0:0.###} {1}", val, unit);
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

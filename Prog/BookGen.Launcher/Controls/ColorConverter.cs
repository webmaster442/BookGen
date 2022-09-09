using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using MahApps.Metro;

namespace BookGen.Launcher.Controls
{
    internal class ColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                int hue = CalculateHueHash(str);
            }
            return Binding.DoNothing;
        }

        private static int CalculateHueHash(string str)
        {
            ulong val = 14695981039346656037L;
            foreach (var chr in str)
            {
                val = val * 1099511628211 ^ chr;
            }
            return (int)(val % 360);
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

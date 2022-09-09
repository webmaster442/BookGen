using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookGen.Launcher.Controls
{
    internal class BooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool invert = false;
            if (parameter != null)
                invert = true;

            if (value is bool boolValue)
            {
                if (!invert && boolValue) return Visibility.Visible;
                else if (invert && !boolValue) return Visibility.Visible;
                else return Visibility.Collapsed;
            }

            return Binding.DoNothing;
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

//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookGen.Launch.Code
{
    public class DialogButtonConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MessageBoxButton buttons
                && parameter is string ButtonName)
            {
                if (buttons == MessageBoxButton.OK && ButtonName == "BtnOk")
                {
                    return Visibility.Visible;
                }
                else if (buttons == MessageBoxButton.OKCancel
                    && (ButtonName == "BtnOk" || ButtonName == "BtnCancel"))
                {
                    return Visibility.Visible;
                }
                else if (buttons == MessageBoxButton.YesNoCancel
                    && (ButtonName == "BtnYes" || ButtonName == "BtnNo" || ButtonName == "BtnCancel"))
                {
                    return Visibility.Visible;
                }
                else if (buttons == MessageBoxButton.YesNo
                    && (ButtonName == "BtnYes" || ButtonName == "BtnNo"))
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;

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

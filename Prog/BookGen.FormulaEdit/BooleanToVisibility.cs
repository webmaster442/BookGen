//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookGen.FormulaEdit;

internal class BooleanToVisibility : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool bValue)
        {
            if (parameter != null)
            {
                return bValue
                    ? System.Windows.Visibility.Collapsed
                    : System.Windows.Visibility.Visible;
            }

            return bValue
                 ? System.Windows.Visibility.Visible
                 : System.Windows.Visibility.Collapsed;
        }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is System.Windows.Visibility visibility)
        {
            if (parameter != null)
            {
                return visibility == System.Windows.Visibility.Collapsed;
            }

            return visibility == System.Windows.Visibility.Visible;
        }
        return Binding.DoNothing;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
        => this;
}

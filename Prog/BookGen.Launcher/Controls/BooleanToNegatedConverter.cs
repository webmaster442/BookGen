//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------
namespace BookGen.Launcher.Controls;

internal sealed class BooleanToNegatedConverter : MarkupExtension, IValueConverter
{
    private static object Negate(object value)
    {
        return value is bool b
            ? !b 
            : Binding.DoNothing;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => Negate(value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => Negate(value);

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}

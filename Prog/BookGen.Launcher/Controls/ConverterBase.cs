using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookGen.Launcher.Controls
{
    internal abstract class ConverterBase<TFrom, TTo> : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TFrom tFrom)
            {
                return ConvertToTTo(tFrom, parameter)!;
            }
            return Binding.DoNothing;
        }

        protected abstract TTo ConvertToTTo(TFrom tFrom, object parameter);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}

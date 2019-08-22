//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace BookGen.Editor.Infrastructure
{
    public class EditorInfoConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = value as string;

            var lines = input?.Count(x => x == '\n') ?? 0;
            var words = input?.Count(x => x == ' ') ?? 0;
            var chars = input?.Length ?? 0;
            var size = FileSizeConverter.GetFileSize(chars);

            return $"{chars} chars | {lines} lines | {words} words | ~ {size}";
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
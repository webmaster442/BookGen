//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BookGen.Cli.ArgumentParsing
{
    internal static class ValueConverter
    {
        private static bool TryGetNullableType(Type input, [NotNullWhen(true)] out Type? result)
        {
            result = Nullable.GetUnderlyingType(input);
            return result != null;
        }

        public static object? Convert(string? value, Type propertyType)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            Type currenttype = propertyType;
            if (TryGetNullableType(currenttype, out Type? nullable))
            {
                currenttype = nullable;
            }

            if (currenttype.IsEnum
                && Enum.TryParse(currenttype, value, true, out object? parsed))
            {
                return parsed;
            }

            if (currenttype == typeof(FsPath))
            {
                return new FsPath(Path.GetFullPath(value));
            }

            try
            {
                object converted = System.Convert.ChangeType(value, currenttype, CultureInfo.InvariantCulture);
                return converted;
            }
            catch (Exception ex)
                  when (ex is InvalidCastException
                       or FormatException
                       or OverflowException
                       or ArgumentException)
            {
                Debugger.Break();
                return null;
            }

        }
    }
}

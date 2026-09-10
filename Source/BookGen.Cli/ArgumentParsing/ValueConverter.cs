//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BookGen.Cli.ArgumentParsing;

internal static class ValueConverter
{
    private static bool TryGetNullableType(Type input, [NotNullWhen(true)] out Type? result)
    {
        result = Nullable.GetUnderlyingType(input);
        return result != null;
    }

    public static object? Convert(string? value, Type targetPropertyType)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        Type currentType = targetPropertyType;
        if (TryGetNullableType(currentType, out Type? nullable))
        {
            currentType = nullable;
        }

        if (currentType.IsEnum
            && Enum.TryParse(currentType, value, true, out object? parsed))
        {
            return parsed;
        }

        try
        {
            checked
            {
                object converted = System.Convert.ChangeType(value, currentType, CultureInfo.InvariantCulture);
                return converted;
            }
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

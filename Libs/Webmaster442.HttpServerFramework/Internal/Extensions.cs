// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2023 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Globalization;

namespace Webmaster442.HttpServerFramework.Internal;

internal static class Extensions
{
    public static string ToHeaderFormat(this DateTime dateTime)
    {
        var culture = new CultureInfo("en-US");
        if (dateTime.Kind != DateTimeKind.Utc)
        {
            dateTime = dateTime.ToUniversalTime();
        }
        return $"{dateTime.ToString("ddd, dd MMM yyyy HH:mm:ss", culture)} GMT";
    }
}

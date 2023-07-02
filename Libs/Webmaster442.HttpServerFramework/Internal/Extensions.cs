// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2023 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Globalization;

using Webmaster442.HttpServerFramework.Domain;

namespace Webmaster442.HttpServerFramework.Internal;

internal static class Extensions
{
    public static string ToLastModifiedHeaderFormat(this DateTime dateTime)
    {
        var culture = new CultureInfo("en-US");
        if (dateTime.Kind != DateTimeKind.Utc)
        {
            dateTime = dateTime.ToUniversalTime();
        }
        return $"{dateTime.ToString("ddd, dd MMM yyyy HH:mm:ss", culture)} GMT";
    }

    public static bool GetAcceptedEncodings(this HttpRequest request, out EncodingType encodingType)
    {
        if (!request.Headers.ContainsKey("Accept-Encoding"))
        {
            encodingType = EncodingType.None;
            return false;
        }

        EncodingType results = EncodingType.None;

        var values = request.Headers["Accept-Encoding"]
            .Split(',', StringSplitOptions.TrimEntries);

        foreach (string value in values)
        {
            if (!TryParseKnownEncoding(value, out encodingType))
            {
                encodingType = EncodingType.None;
                return false;
            }

            results |= encodingType;
        }

        encodingType = results;
        return true;
    }

    private static bool TryParseKnownEncoding(string value, out EncodingType encodingType)
    {
        switch (value)
        {
            case "gzip":
                encodingType = EncodingType.Gzip;
                return true;
            case "deflate":
                encodingType = EncodingType.Deflate;
                return true;
            case "br":
                encodingType = EncodingType.Brotli;
                return true;
            case "compress":
            case "identity":
            case "*":
                encodingType = EncodingType.None;
                return true;
            default:
                encodingType = EncodingType.None;
                return false;
        }
    }
}

using Webmaster442.HttpServerFramework.Domain;

namespace Webmaster442.HttpServerFramework.Internal;

internal static class Extensions
{
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

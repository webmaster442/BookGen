using System;

namespace BookGen.Launcher
{
    internal static class Extensions
    {
        public static bool IsUrl(this string htmlText)
        {
            return
                !string.IsNullOrEmpty(htmlText)
                && htmlText.StartsWith("http")
                && Uri.TryCreate(htmlText, UriKind.RelativeOrAbsolute, out _);
        }
    }
}

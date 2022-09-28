//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

using System;

namespace ColorCode.Formatting
{
    public static class HttpUtility
    {
        public static string HtmlEncode(string value)
        {
            return System.Net.WebUtility.HtmlEncode(value);
        }
    }
}
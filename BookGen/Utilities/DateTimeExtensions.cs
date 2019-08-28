//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Utilities
{
    public static class DateTimeExtensions
    {
        private const string w3cTime = "yyyy-MM-ddTHH:mm:ss.fffffffzzz";
        private const string wpTime = "E, d MMM yyyy HH:mm:ss Z";

        public static string ToW3CTimeFormat(this DateTime dt)
        {
            return dt.ToString(w3cTime);
        }

        public static string ToWpTimeFormat(this DateTime dt)
        {
            return dt.ToString(wpTime);
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;

namespace BookGen.DomainServices
{
    public static class DateTimeExtensions
    {
        private const string w3cTime = "yyyy-MM-ddTHH:mm:sszzz";
        private const string w3zTime = "yyyy-MM-ddTHH:mm:ss";
        private const string wpTime = "ddd, d MMM yyyy HH:mm:ss"; //Tue, 27 Aug 2019 18:31:33 +0000

        public static string ToW3CTimeFormat(this DateTime dt)
        {
            return dt.ToString(w3cTime);
        }

        public static string ToW3CZTimeFormat(this DateTime dt)
        {
            return dt.ToString(w3zTime) + "Z";
        }

        public static string ToWpTimeFormat(this DateTime dt)
        {
            return dt.ToString(wpTime, new CultureInfo("en-US")) + " +0000";
        }

        public static string ToWpPostDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss");//2011-04-07 13:08:53
        }
    }
}

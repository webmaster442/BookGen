//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Web;

namespace BookGen.Utilities
{
    internal static class UrlOpener
    {
        internal static bool OpenUrl(string url)
        {
            try
            {
                using (var process = new System.Diagnostics.Process())
                {
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.FileName = url;
                    process.Start();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool OpenUrlWithParameters(string url, params string[] parameters)
        {
            string full = string.Format(url, parameters);
            string encoded = HttpUtility.UrlEncode(full);
            return OpenUrl(encoded);
        }
    }
}

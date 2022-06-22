//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
            var encoded = parameters.Select(x => HttpUtility.UrlEncode(x)).ToArray();
            string full = string.Format(url, encoded);
            return OpenUrl(full);
        }
    }
}

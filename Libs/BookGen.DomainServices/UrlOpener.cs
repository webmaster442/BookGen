//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Web;

namespace BookGen.DomainServices
{
    public static class UrlOpener
    {
        public static bool OpenUrl(string url)
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
    }
}

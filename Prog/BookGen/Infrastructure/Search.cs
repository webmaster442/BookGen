//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Threading;

namespace BookGen.Infrastructure;
public static class Search
{
    public static void Perform(string url, string terms, ILog log)
    {
        if (!UrlOpener.OpenUrlWithParameters(url, terms))
        {
            log.Warning("Coudn't open: {0}", url);
        }
        Thread.Sleep(70);
    }
}

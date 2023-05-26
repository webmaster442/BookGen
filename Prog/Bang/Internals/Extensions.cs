//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Web;

using Bang.Model;

namespace Bang.Internals;

internal static class Extensions
{
    public static IEnumerable<string> KnownBangs(this Bangs bangs)
    {
        var sites = bangs.Sites.Select(site => site.Name);
        var aliases = bangs.Aliases.Select(alias => alias.Name);

        return sites.Concat(aliases).Order();
    }

    public static bool TryGetSearchUrls(this Bangs bangs, string bangName, string terms, out IReadOnlyList<string> urls)
    {
        var site = bangs.Sites.FirstOrDefault(site => site.Name == bangName);
        var aliases = bangs.Aliases.FirstOrDefault(alias => alias.Name == bangName);

        if (site != null)
        {
            urls = new[] { CreateUrl(site, terms) };
            return true;
        }
        else if (aliases != null)
        {
            var list = new List<string>();
            foreach (var alias in aliases.SiteNames)
            {
                list.Add(CreateUrl(bangs.Sites.First(site => site.Name == alias), terms));
            }
            urls = list;
            return true;
        }
        else
        {
            urls = Array.Empty<string>();
            return false;
        }
    }

    private static string CreateUrl(Site site, string terms)
    {
        var spaceEncoded = terms.Replace(" ", site.Space);
        var parameter = HttpUtility.UrlEncode(spaceEncoded);
        return string.Format(site.Url, parameter);
    }
}
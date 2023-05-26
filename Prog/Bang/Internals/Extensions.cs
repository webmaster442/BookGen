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

    public static IReadOnlyList<string> GetSearchUrls(this Bangs bangs, string bangName, string terms)
    {
        var site = bangs.Sites.FirstOrDefault(site => site.Name == bangName);
        var aliases = bangs.Aliases.FirstOrDefault(alias => alias.Name == bangName);

        if (site != null)
        {
            return new[] { CreateUrl(site, terms) };
        }
        else if (aliases != null)
        {
            var list = new List<string>();
            foreach (var alias in aliases.SiteNames)
            {
                list.Add(CreateUrl(bangs.Sites.First(site => site.Name == alias), terms));
            }
            return list;
        }
        else
        {
            return Array.Empty<string>();
        }
    }

    private static string CreateUrl(Site site, string terms)
    {
        var spaceEncoded = terms.Replace(" ", site.Space);
        var parameter = HttpUtility.UrlEncode(spaceEncoded);
        return string.Format(site.Url, parameter);
    }
}
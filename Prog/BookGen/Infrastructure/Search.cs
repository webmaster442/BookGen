//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Threading;
using System.Web;

namespace BookGen.Infrastructure;
public static class Search
{
    public record class SearchModel(string Url, string Space);

    public readonly static SearchModel Pexels = new("https://www.pexels.com/search/{0}/", "%20");
    public readonly static SearchModel Unsplash = new("https://unsplash.com/s/photos/{0}", "-");
    public readonly static SearchModel Pixabay = new("https://pixabay.com/images/search/{0}/", "%20");

    public static void Perform(SearchModel model, string terms, ILog log)
    {
        string encodedTerms = terms.Replace(" ", model.Space);
        encodedTerms = HttpUtility.UrlEncode(terms);
        string fullUrl = string.Format(model.Url, encodedTerms);
        if (!UrlOpener.OpenUrl(fullUrl))
        {
            log.Warning("Coudn't perform search on: {0}", model.Url);
        }
        Thread.Sleep(70);
    }
}

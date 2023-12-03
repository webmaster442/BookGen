using System.Diagnostics.CodeAnalysis;

using BookGen.Api;
using BookGen.Interfaces;

namespace BookGen.DomainServices;
public static class TocUtils
{
    public static IEnumerable<Link> GetLinks(FsPath directory, IEnumerable<string> exclude, ILog log)
    {
        var contents = directory.GetAllFiles(true, "*.md");

        var excludes = exclude.ToHashSet();

        foreach (var file in contents)
        {
            if (excludes.Contains(file.Filename))
                continue;

            var content = file.ReadFile(log);
            var title = MarkdownUtils.GetDocumentTitle(content, log, file);
            yield return new Link(title, file.GetRelativePathRelativeTo(directory).ToString().Replace(@"\", "/"));
        }
    }

    public static readonly IEqualityComparer<Link> LinkTargetComparer = new LinkByUrlEqualityComparer();

    public static readonly IEqualityComparer<Link> LinkTitleComparer = new LinkByTitleComparer();

    public static string ToMarkdownLink(Link link)
    {
        return $"[{link.Text}]({link.Url})";
    }

    private class LinkByTitleComparer : IEqualityComparer<Link>
    {
        public bool Equals(Link? x, Link? y)
        {
            return x?.Text == y?.Text;
        }

        public int GetHashCode([DisallowNull] Link obj)
        {
            return obj.Text.GetHashCode();
        }
    }

    private class LinkByUrlEqualityComparer : IEqualityComparer<Link>
    {
        public bool Equals(Link? x, Link? y)
        {
            return x?.Url == y?.Url;
        }

        public int GetHashCode([DisallowNull] Link obj)
        {
            return obj.Url.GetHashCode();
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;
using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;

using BookGen.Vfs;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Microsoft.Extensions.Logging;

using YamlDotNet.Serialization;

namespace Bookgen.Lib.Internals;

internal static class Extensions
{
    private const string W3cTime = "yyyy-MM-ddTHH:mm:sszzz";
    private const string W3zTime = "yyyy-MM-ddTHH:mm:ss";
    private const string WorpdressTime = "ddd, d MMM yyyy HH:mm:ss";
    private const string WordpressPostDate = "yyyy-MM-dd HH:mm:ss";

    extension(DateTime dt)
    {
        public string ToW3CTimeFormat()
            => dt.ToString(W3cTime);

        public string ToW3CZTimeFormat()
            => dt.ToString(W3zTime) + "Z";

        public string ToWordpressTime()
            => dt.ToString(WorpdressTime, new CultureInfo("en-US")) + " +0000";

        public string ToWordpressPostDate()
            => dt.ToString(WordpressPostDate);
    }

    extension(IReadOnlyFileSystem folder)
    {
        public async Task<string?> GetCoverFileName(TableOfContents tableOfContents, ILogger logger)
        {
            var contents = await folder.ReadAllTextAsync(tableOfContents.IndexFile);
            foreach (Block block in Markdig.Markdown.Parse(contents))
            {
                if (block is ParagraphBlock paragraph && paragraph.Inline != null)
                {
                    foreach (Inline inline in paragraph.Inline)
                    {
                        if (inline is LinkInline link && link.IsImage)
                        {
                            return link.Url;
                        }
                    }
                }
            }
            logger.LogWarning("No cover image found in {file}", tableOfContents.IndexFile);
            return null;
        }
    }
}

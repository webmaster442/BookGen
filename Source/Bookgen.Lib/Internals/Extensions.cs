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

    private static async Task<(string content, FrontMatter frontMatter)> GetFileContents(IReadOnlyFileSystem folder,
                                                                                     string file,
                                                                                     ILogger logger)
    {
        static FrontMatter CreateDefaultFrontMatter(string diskPath, ILogger log)
        {
            log.LogWarning("{file} didn't contain any frontmatter data, using default", diskPath);
            return new FrontMatter
            {
                Data = new(),
                Tags = string.Empty,
                Title = diskPath,
            };
        }

        IDeserializer yamlDeserializer = YamlSerializerFactory.CreateDeserializer();
        StringBuilder content = new StringBuilder();
        StringBuilder yaml = new StringBuilder();

        using var reader = folder.OpenTextReader(file);

        string? line;
        bool inYaml = false;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line == "---")
            {
                inYaml = !inYaml;
            }
            else
            {
                if (inYaml)
                    yaml.AppendLine(line);
                else
                    content.AppendLine(line);
            }
        }

        FrontMatter frontMatter = yaml.Length > 0 ? yamlDeserializer.Deserialize<FrontMatter>(yaml.ToString()) : CreateDefaultFrontMatter(file, logger);

        return (content.ToString(), frontMatter);
    }

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
        public async Task<SourceFile> GetSourceFile(string file, ILogger logger)
        {
            (string content, FrontMatter frontMatter) = await GetFileContents(folder, file, logger);

            return new SourceFile
            {
                FileNameInToc = file,
                LastModified = folder.GetLastModifiedUtc(file),
                Content = content,
                FrontMatter = frontMatter,
            };
        }

        public async Task<string?> GetCoverFileName(TableOfContents tableOfContents, ILogger logger)
        {
            var contents = await folder.ReadAllTextAsync(tableOfContents.IndexFile);
            foreach (var block in Markdig.Markdown.Parse(contents))
            {
                if (block is ParagraphBlock paragraph && paragraph.Inline != null)
                {
                    foreach (var inline in paragraph.Inline)
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

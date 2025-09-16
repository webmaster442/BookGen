//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Pipeline;

using BookGen.Vfs;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Microsoft.Extensions.Logging;

using YamlDotNet.Serialization;

namespace Bookgen.Lib.Internals;

internal static class IReadOnlyFileSystemExtensions
{
    public static async Task<SourceFile> GetSourceFile(this IReadOnlyFileSystem folder, string file, ILogger logger)
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

    public static async Task<string?> GetCoverFileName(this IReadOnlyFileSystem folder, TableOfContents tableOfContents, ILogger logger)
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


    private static async Task<(string content, FrontMatter frontMatter)> GetFileContents(IReadOnlyFileSystem folder, string file, ILogger logger)
    {
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

    private static FrontMatter CreateDefaultFrontMatter(string diskPath, ILogger log)
    {
        log.LogWarning("{file} didn't contain any frontmatter data, using default", diskPath);
        return new FrontMatter
        {
            Data = new(),
            Tags = string.Empty,
            Title = diskPath,
        };
    }
}

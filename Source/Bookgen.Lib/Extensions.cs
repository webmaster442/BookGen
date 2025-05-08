
using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.VFS;

using Microsoft.Extensions.Logging;

using YamlDotNet.Serialization;

namespace Bookgen.Lib;
public static class Extensions
{
    public static SourceFile[] GetSourceFiles(this TocChapter tocEntry, IFolder folder, ILogger logger)
    {
        SourceFile[] results = new SourceFile[tocEntry.Files.Length];
        for (int i=0; i<tocEntry.Files.Length; i++)
        {
            results[i] = GetSourceFiles(folder, tocEntry.Files[i], logger);
        }
        return results;
    }

    private static SourceFile GetSourceFiles(IFolder folder, string file, ILogger logger)
    {
        (string content, FrontMatter frontMatter) = GetFileContents(folder, file, logger);

        return new SourceFile
        {
            FileNameInToc = file,
            LastModified = folder.GetLastModifiedUtc(file),
            Content = content,
            FrontMatter = frontMatter,
        };
    }

    private static (string content, FrontMatter frontMatter) GetFileContents(IFolder folder, string file, ILogger logger)
    {
        IDeserializer yamlDeserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();
        StringBuilder content = new StringBuilder();
        StringBuilder yaml = new StringBuilder();

        using var reader = folder.OpenText(file);

        string? line;
        bool inYaml = false;
        while ((line = reader.ReadLine()) != null)
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

        FrontMatter frontMatter = yaml.Length > 0 ? CreateDefaultFrontMatter(file, logger) : yamlDeserializer.Deserialize<FrontMatter>(yaml.ToString());

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

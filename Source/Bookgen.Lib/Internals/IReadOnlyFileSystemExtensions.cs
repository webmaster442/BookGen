
using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;

using BookGen.Vfs;

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

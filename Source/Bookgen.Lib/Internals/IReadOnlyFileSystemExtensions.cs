
using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using YamlDotNet.Serialization;

namespace Bookgen.Lib.Internals;

internal static class IReadOnlyFileSystemExtensions
{
    public static SourceFile GetSourceFile(this IReadOnlyFileSystem folder, string file, ILogger logger)
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

    private static (string content, FrontMatter frontMatter) GetFileContents(IReadOnlyFileSystem folder, string file, ILogger logger)
    {
        IDeserializer yamlDeserializer = YamlSerializerFactory.CreateDeserializer();
        StringBuilder content = new StringBuilder();
        StringBuilder yaml = new StringBuilder();

        using var reader = folder.OpenTextReader(file);

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

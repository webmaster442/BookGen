//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using YamlDotNet.Serialization;

namespace Bookgen.Lib.Rendering;

public static class MarkdownLoader
{
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

        using TextReader reader = folder.OpenTextReader(file);

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
    }
}

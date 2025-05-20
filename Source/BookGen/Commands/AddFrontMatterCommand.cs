using System.Text;

using Bookgen.Lib;
using Bookgen.Lib.Domain.IO;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Markdig;
using Markdig.Syntax;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("addfrontmatter")]
internal class AddFrontMatterCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;
    private readonly ILogger _logger;
    private readonly ProgramInfo _programInfo;

    public AddFrontMatterCommand(IWritableFileSystem writableFileSystem, ILogger logger, ProgramInfo programInfo)
    {
        _writableFileSystem = writableFileSystem;
        _logger = logger;
        _programInfo = programInfo;
    }

    public override async Task<int> Execute(BookGenArgumentBase arguments, string[] context)
    {
        _programInfo.EableVerboseLogging(arguments.Verbose);

        int modified = 0;
        _writableFileSystem.Scope = arguments.Directory;
        var files = _writableFileSystem.GetFiles(arguments.Directory, "*.md", true).ToArray();
        _logger.LogInformation("Found {count} markdown files in {directory}", files.Length, arguments.Directory);

        var serializer = YamlSerializerFactory.CreateSerializer();

        foreach (var file in files)
        {
            string content = await _writableFileSystem.ReadAllTextAsync(file);
            if (content.StartsWith("---\r\n") || content.StartsWith("---\n"))
            {
                //File has yaml frontmatter, ignore it
                continue;
            }

            _logger.LogDebug("Adding front matter to: {file}...", file);

            var firstHedding = Markdown.Parse(content).OfType<HeadingBlock>().FirstOrDefault();

            string title = firstHedding?.Inline != null
                ? string.Join("", firstHedding.Inline)
                : Path.GetFileName(file);

            FrontMatter frontMatter = new()
            {
                Title = title,
                Tags = string.Empty
            };

            const string divider = "---";

            StringBuilder result = new();
            result.AppendLine(divider)
                  .Append(serializer.Serialize(frontMatter))
                  .AppendLine(divider).AppendLine()
                  .Append(content);

            await _writableFileSystem.WriteAllTextAsync(file, result.ToString());
            ++modified;
        }

        _logger.LogInformation("Added frontMatter to {count} files", modified);

        return ExitCodes.Succes;
    }
}

using Bookgen.Lib;
using Bookgen.Lib.Markdown;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Loging;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("links")]
internal sealed class LinksCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _soruce;
    private readonly ILogger _logger;
    private readonly IAssetSource _assetSource;

    public LinksCommand(IWritableFileSystem soruce, ILogger logger, IAssetSource assetSource)
    {
        _soruce = soruce;
        _logger = logger;
        _assetSource = assetSource;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _soruce.Scope = arguments.Directory;

        using var env = new BookEnvironment(_soruce, _soruce, _assetSource);
        EnvironmentStatus status = await env.Initialize(autoUpgrade: true);

        if (!status.IsOk)
        {
            _logger.EnvironmentStatus(status);
            return ExitCodes.ConfigError;
        }

        Dictionary<string, string[]> allLinks = new();

        foreach (var chapter in env.TableOfContents.Chapters)
        {
            _logger.LogInformation("Scanning {chapter} for links...", chapter.Title);

            HashSet<string> chapterLinks = new();
            foreach (var file in chapter.Files)
            {
                _logger.LogDebug("Scanning {file}...", file);
                var text = await _soruce.ReadAllTextAsync(file);
                chapterLinks.UnionWith(MarkdownProcesor.GetLinks(text));
            }
            allLinks.Add(chapter.Title, chapterLinks.ToArray());
            chapterLinks.Clear();
        }

        MarkdownBuilder markdown = new();
        foreach (var linkData in allLinks)
        {
            markdown.Heading(2, linkData.Key);
            markdown.UnorderedList(linkData.Value);
        }

        _logger.LogInformation("Writing links.md...");
        await _soruce.WriteAllTextAsync("links.md", markdown.ToString());

        return ExitCodes.Success;
    }
}

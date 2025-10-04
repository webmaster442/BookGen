//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;
using Bookgen.Lib;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Loging;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("links")]
internal sealed partial class LinksCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _soruce;
    private readonly ILogger _logger;

    public LinksCommand(IWritableFileSystem soruce, ILogger logger)
    {
        _soruce = soruce;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        _soruce.Scope = arguments.Directory;

        using var env = new BookEnvironment(_soruce, _soruce);
        EnvironmentStatus status = await env.Initialize();

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
                chapterLinks.UnionWith(GetLinks(text));
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

    [GeneratedRegex(@"https?://[^\s\)\]\}""'<>]+")]
    private partial Regex Links { get; }

    private IEnumerable<string> GetLinks(string text)
    {
        var links = Links.Matches(text);
        foreach (Match link in links)
        {
            yield return link.Value;
        }
    }
}

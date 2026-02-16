//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Net;
using System.Text.RegularExpressions;

using Bookgen.Lib;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Loging;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("links")]
internal sealed partial class LinksCommand : AsyncCommand<LinksCommand.LinkArguments>
{
    public sealed class LinkArguments : BookGenArgumentBase
    {
        [Switch("vf", "verify")]
        public bool Verify { get; set; }
    }


    private readonly IWritableFileSystem _soruce;
    private readonly ILogger _logger;

    public LinksCommand(IWritableFileSystem soruce, ILogger logger)
    {
        _soruce = soruce;
        _logger = logger;
    }

    public override async Task<int> ExecuteAsync(LinkArguments arguments, IReadOnlyList<string> context)
    {
        _soruce.Scope = arguments.Directory;

        using var env = new BookEnvironment(_soruce, _soruce);
        EnvironmentStatus status = await env.Initialize(arguments.ConfigOverlay);

        if (!status.IsOk)
        {
            _logger.EnvironmentStatus(status);
            return ExitCodes.ConfigError;
        }

        Dictionary<string, string[]> allLinks = new();
        Dictionary<string, string[]> badLinks = new();

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

            if (arguments.Verify)
            {
                _logger.LogInformation("Verifying links in {chapter}...", chapter.Title);
                ConcurrentDictionary<string, HttpStatusCode> linksWithIssues = await VerifyLinks(chapterLinks);
                if (linksWithIssues.Count > 0)
                {
                    _logger.LogWarning("Found {count} links with issues in {chapter}:", linksWithIssues.Count, chapter.Title);
                    badLinks.Add(chapter.Title, linksWithIssues.Select(kvp => $"{kvp.Key} - {kvp.Value}").ToArray());
                }
            }

            allLinks.Add(chapter.Title, chapterLinks.ToArray());
            chapterLinks.Clear();
        }

        await WriteMarkdown(allLinks, "links.md");

        if (arguments.Verify)
        {
            await WriteMarkdown(badLinks, "links.issues.md");
        }

        return ExitCodes.Success;
    }

    private async Task WriteMarkdown(Dictionary<string, string[]> dataSet, string fileName)
    {
        MarkdownBuilder markdown = new();
        foreach (var linkData in dataSet)
        {
            markdown.Heading(2, linkData.Key);
            markdown.UnorderedList(linkData.Value);
        }

        _logger.LogInformation("Writing {file}...", fileName);
        await _soruce.WriteAllTextAsync(fileName, markdown.ToString());
    }

    [GeneratedRegex(@"https?://[^\s\)\]\}""'<>]+")]
    private static partial Regex Links { get; }

    private static IEnumerable<string> GetLinks(string text)
    {
        var links = Links.Matches(text);
        foreach (Match link in links)
        {
            yield return link.Value;
        }
    }

    private async Task<ConcurrentDictionary<string, HttpStatusCode>> VerifyLinks(HashSet<string> chapterLinks)
    {
        ConcurrentDictionary<string, HttpStatusCode> linksWithIssues = new();

        await Parallel.ForEachAsync(chapterLinks, async (link, cancellationToken) =>
        {
            using var client = CreateHttpClient();

            try
            {
                _logger.LogDebug("Verifying {link}...", link);
                using var request = new HttpRequestMessage(HttpMethod.Head, link);

                using var response = await client.SendAsync(request,
                                                            HttpCompletionOption.ResponseHeadersRead,
                                                            cancellationToken);

                // If HEAD is not allowed, fall back to a minimal GET
                if (response.StatusCode == HttpStatusCode.MethodNotAllowed ||
                    response.StatusCode == HttpStatusCode.NotImplemented)
                {
                    using var getRequest = new HttpRequestMessage(HttpMethod.Get, link);
                    using var getResponse = await client.SendAsync(getRequest,
                                                                   HttpCompletionOption.ResponseHeadersRead,
                                                                   cancellationToken);

                    if (!getResponse.IsSuccessStatusCode)
                    {
                        linksWithIssues.TryAdd(link, getResponse.StatusCode);
                    }

                    return;
                }

                if (!response.IsSuccessStatusCode)
                {
                    linksWithIssues.TryAdd(link, response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying link: {link}", link);
                linksWithIssues.TryAdd(link, HttpStatusCode.ServiceUnavailable);
            }
        });

        return linksWithIssues;
    }

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5),
        };

        client.DefaultRequestHeaders.UserAgent.Add(
            new System.Net.Http.Headers.ProductInfoHeaderValue("BookGenLinkChecker", "1.0"));

        return client;
    }

}

//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text.RegularExpressions;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Loging;
using BookGen.Lib;
using BookGen.Lib.AppSettings;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Lib.Rendering.Images;
using BookGen.Lib.Rendering.Markdown;
using BookGen.Lib.Rendering.Markdown.RenderInterop;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands.Folder;

[CommandName("search")]
[Description("Search for a given text in the markdown files of the book and print the results to the console.")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
[ExitCode(ExitCodes.GeneralError, "Search produced no results.")]
[ExitCode(ExitCodes.ConfigError, "Project has config issues.")]
internal sealed class SearchCommand : AsyncCommand<SearchCommand.Arguments>
{
    public sealed class Arguments : BookGenArgumentBase
    {
        [Argument(0)]
        [Description("Required argument. The text to search for. The command will search for the given text in all markdown files in the book and will print the results to the console.")]
        public string Query { get; set; } = string.Empty;

        [Switch("r", "regex", Required = false)]
        [Description("Optional switch. If specified, the query will be treated as a regular expression.")]
        public bool Regex { get; set; }

        public override ValidationResult Validate(IValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(Query))
            {
                return ValidationResult.Error("Query cannot be empty.");
            }

            return base.Validate(context);
        }
    }

    private readonly IWritableFileSystem _soruce;
    private readonly ILogger _logger;
    private readonly IAssetSource _assetSource;
    private readonly IProgramPathResolver _programPathResolver;

    public SearchCommand(IWritableFileSystem soruce, IProgramPathResolver programPathResolver, ILogger logger, IAssetSource assetSource)
    {
        _soruce = soruce;
        _programPathResolver = programPathResolver;
        _logger = logger;
        _assetSource = assetSource;
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        _soruce.Scope = arguments.Directory;


        _soruce.Scope = arguments.Directory;
        IWritableFileSystem target = new ReadOnlyWritableFileSystem();

        using var env = new BookEnvironment(_soruce, target, _programPathResolver, _assetSource);
        EnvironmentStatus status = await env.Initialize(arguments.ConfigOverlay);

        if (!status.IsOk)
        {
            _logger.EnvironmentStatus(status);
            return ExitCodes.ConfigError;
        }

        var imageConfig = new ImageConfig();

        var imgService = new ImgService(env.Source, _logger, imageConfig);

        using var settings = new MarkdownRenderSettings(imgService)
        {
            HostUrl = string.Empty,
            DeleteFirstH1 = false,
            CssClasses = new CssClasses(),
            OffsetHeadingsBy = 0,
            AutoEmbedSupportedLinks = false,
            RenderInterop = new RenderInterop(_assetSource, _programPathResolver, imageConfig),
        };
        settings.RenderInterop.PreRenderCode = false;

        using var markdownConverter = new MarkdownConverter(settings);

        ConcurrentDictionary<string, string> searchResults = new();

        if (arguments.Regex)
        {
            Regex regex = new Regex(arguments.Query, RegexOptions.Compiled, TimeSpan.FromSeconds(5));
            await Parallel.ForEachAsync(env.TableOfContents.GetFiles(), async (file, ct) =>
            {
                string markdown = await env.Source.ReadAllTextAsync(file);
                string plain = markdownConverter.RenderToPlainText(markdown);
                if (Search.RegexContains(plain, regex, out string? context))
                {
                    searchResults.TryAdd(file, context);
                }
            });
        }
        else
        {
            await Parallel.ForEachAsync(env.TableOfContents.GetFiles(), async (file, ct) =>
            {
                string markdown = await env.Source.ReadAllTextAsync(file);
                string plain = markdownConverter.RenderToPlainText(markdown);

                if (Search.Contains(plain, arguments.Query, 0.8f, out string? context))
                {
                    searchResults.TryAdd(file, context);
                }
            });
        }

        if (searchResults.IsEmpty)
        {
            _logger.LogError("No results found for query: {Query}", arguments.Query);
            return ExitCodes.GeneralError;
        }

        foreach (KeyValuePair<string, string> result in searchResults.OrderBy(x => x.Key))
        {
            _logger.LogInformation("Found in {File}: {Context}", result.Key, result.Value);
        }

        return ExitCodes.Success;
    }
}

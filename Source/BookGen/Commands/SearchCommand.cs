//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Concurrent;

using Bookgen.Lib;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.JsInterop;
using Bookgen.Lib.Markdown;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Loging;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("search")]
internal sealed class SearchCommand : AsyncCommand<SearchCommand.SearchArguments>
{
    public sealed class SearchArguments : BookGenArgumentBase
    {
        [Argument(0)]
        public string Query { get; set; } = string.Empty;

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

    public SearchCommand(IWritableFileSystem soruce, ILogger logger, IAssetSource assetSource)
    {
        _soruce = soruce;
        _logger = logger;
        _assetSource = assetSource;
    }

    public override async Task<int> ExecuteAsync(SearchArguments arguments, IReadOnlyList<string> context)
    {
        _soruce.Scope = arguments.Directory;


        _soruce.Scope = arguments.Directory;
        IWritableFileSystem target = new ReadOnlyWritableFileSystem();

        using var env = new BookEnvironment(_soruce, target, _assetSource);
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
            PrismJsInterop = null,
            ImageRenderJsInterop = new ImageRenderJsInterop(_assetSource, imageConfig)
        };

        using var markdownConverter = new MarkdownConverter(settings);

        ConcurrentDictionary<string, string> searchResults = new();

        await Parallel.ForEachAsync(env.TableOfContents.GetFiles(), async (file, ct) =>
        {
            string markdown = await env.Source.ReadAllTextAsync(file);
            string plain = markdownConverter.RenderToPlainText(markdown);

            if (Search.Contains(plain, arguments.Query, 0.8f, out string? context))
            {
                searchResults.TryAdd(file, context);
            }
        });

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

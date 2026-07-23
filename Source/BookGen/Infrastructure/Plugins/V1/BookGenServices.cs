//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.V1;
using BookGen.Lib;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen.Infrastructure.Plugins.V1;

internal class BookGenServices : IBookgenServices
{
    private readonly BookEnvironment _environment;
    private readonly IMemoryCache _cache;
    private readonly ILogger _logger;

    public BookGenServices(BookEnvironment environment,
                           IMemoryCache cache,
                           ILogger logger,
                           IDynamicDocumentGenerator dynamicDocumentGenerator)
    {
        OutputFolder = new FileSystem(environment.Output);
        AssetSource = new AssetSource(environment);
        Logger = new PluginLogger(logger);
        DynamicDocumentation = new DynamicDocumentation(dynamicDocumentGenerator);
        _environment = environment;
        _cache = cache;
        _logger = logger;
    }

    public IFileSystem OutputFolder { get; }

    public IAssetSource AssetSource { get; }

    public IPluginLogger Logger { get; }

    public IDynamicDocumentation DynamicDocumentation { get; }

    public IRenderer CreateRenderer(RendererOptions rendererOptions)
        => new Renderer(rendererOptions, _environment, _cache, _logger);
}

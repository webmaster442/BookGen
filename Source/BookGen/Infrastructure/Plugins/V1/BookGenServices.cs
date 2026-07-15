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

    public BookGenServices(BookEnvironment environment, IMemoryCache cache, ILogger logger)
    {
        OutputFolder = new FileSystem(environment.Output);
        AssetSource = new AssetSource(environment);
        _environment = environment;
        _cache = cache;
        _logger = logger;
    }

    public IFileSystem OutputFolder { get; }

    public IAssetSource AssetSource { get; }

    public IRenderer Create(RendererOptions rendererOptions)
        => new Renderer(rendererOptions, _environment, _cache, _logger);
}

//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Pipeline;

using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("buildwp")]
internal sealed class BuildWordpressCommand : BuildCommandBase
{
    public BuildWordpressCommand(IWritableFileSystem soruce,
                                 IWritableFileSystem target,
                                 ILogger logger,
                                 IAssetSource assetSource,
                                 IMemoryCache memoryCache) 
        : base(soruce, target, logger, assetSource, memoryCache)
    {
    }

    public override Pipeline GetPipeLine()
        => Pipeline.CreateWordpressPipeLine(_memoryCache);
}

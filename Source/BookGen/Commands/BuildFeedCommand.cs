//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using Bookgen.Lib.AppSettings;
using Bookgen.Lib.Pipeline;

using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("buildfeed")]
[Description("Build an RSS 2.0 and an Atom 1.0 feed from the book.")]
internal sealed class BuildFeedCommand : BuildCommandBase
{
    public BuildFeedCommand(IWritableFileSystem soruce,
                            IWritableFileSystem target,
                            IProgramPathResolver programPathResolver,
                            ILogger logger,
                            IAssetSource assetSource,
                            IMemoryCache memoryCache) 
        : base(soruce, target, programPathResolver, logger, assetSource, memoryCache)
    {
    }

    public override Pipeline GetPipeLine()
        => Pipeline.CreateFeedPipeline(_memoryCache);
}

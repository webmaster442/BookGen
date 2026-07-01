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

[CommandName("buildprint")]
[Description("Build a printable HTML & XHTML file from the book.")]
internal sealed class BuildPrintCommand : BuildCommandBase
{
    public BuildPrintCommand(IWritableFileSystem soruce,
                             IWritableFileSystem target,
                             IProgramPathResolver programPathResolver,
                             ILogger logger,
                             IAssetSource assetSource,
                             IMemoryCache memoryCache) 
        : base(soruce, target, programPathResolver, logger, assetSource, memoryCache)
    {
    }

    public override Pipeline GetPipeLine()
        => Pipeline.CratePrintPipeLine(_memoryCache);
}

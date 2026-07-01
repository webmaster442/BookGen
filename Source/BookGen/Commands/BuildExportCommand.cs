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

[CommandName("buildexport")]
[Description("Build a JSON file with schema for post processing of the book.")]
internal sealed class BuildExportCommand : BuildCommandBase
{
    public BuildExportCommand(IWritableFileSystem soruce,
                              IWritableFileSystem target,
                              IProgramPathResolver programPathResolver,
                              ILogger logger,
                              IAssetSource assetSource,
                              IMemoryCache memoryCache)
        : base(soruce, target, programPathResolver, logger, assetSource, memoryCache)
    {
    }

    public override Pipeline GetPipeLine()
        => Pipeline.CreatePostProcessPipeLine(_memoryCache);
}

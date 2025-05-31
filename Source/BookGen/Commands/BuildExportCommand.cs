using Bookgen.Lib.Pipeline;

using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("buildexport")]
internal class BuildExportCommand : BuildCommandBase
{
    public BuildExportCommand(IWritableFileSystem soruce,
                           IWritableFileSystem target,
                           ILogger logger,
                           IAssetSource assetSource)
        : base(soruce, target, logger, assetSource)
    {
    }

    public override Pipeline GetPipeLine()
        => Pipeline.CreatePostProcessPipeLine();
}

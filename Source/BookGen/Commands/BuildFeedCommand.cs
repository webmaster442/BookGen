using Bookgen.Lib.Pipeline;

using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("buildfeed")]
internal sealed class BuildFeedCommand : BuildCommandBase
{
    public BuildFeedCommand(IWritableFileSystem soruce,
                            IWritableFileSystem target,
                            ILogger logger,
                            IAssetSource assetSource)
        : base(soruce, target, logger, assetSource)
    {
    }

    public override Pipeline GetPipeLine()
        => Pipeline.CreateFeedPipeline();
}

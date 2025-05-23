using Bookgen.Lib.Pipeline;

using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("buildweb")]
internal class BuildWebCommand : BuildCommandBase
{
    public BuildWebCommand(IWritableFileSystem soruce,
                           IWritableFileSystem target,
                           ILogger logger,
                           IAssetSource assetSource)
        : base(soruce, target, logger, assetSource)
    {
    }

    public override Pipeline GetPipeLine()
        => Pipeline.CreateWebPipeLine();
}

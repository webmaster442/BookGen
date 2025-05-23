using Bookgen.Lib.Pipeline;

using BookGen.Cli.Annotations;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("buildwp")]
internal class BuildWordpressCommand : BuildCommandBase
{
    public BuildWordpressCommand(IWritableFileSystem soruce, IWritableFileSystem target, ILogger logger) : base(soruce, target, logger)
    {
    }

    public override Pipeline GetPipeLine()
        => Pipeline.CreateWordpressPipeLine();
}

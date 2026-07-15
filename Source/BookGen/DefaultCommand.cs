using System;
using System.Collections.Generic;
using System.Text;

using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen;

[CommandName("default")]
internal sealed class DefaultCommand : Command
{
    public override int Execute(IReadOnlyList<string> context)
    {
        return ExitCodes.Success;
    }
}

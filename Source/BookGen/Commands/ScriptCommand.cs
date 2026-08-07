using System;
using System.Collections.Generic;
using System.Text;

using BookGen.Cli;

namespace BookGen.Commands;

internal sealed class ScriptCommand : AsyncCommand<ScriptCommand.Arguments>
{
    public sealed class Arguments : ArgumentsBase
    {

    }

    public override Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}

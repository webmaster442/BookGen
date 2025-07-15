using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class Initialize : PipeLineStep<EpubState>
{
    public Initialize(EpubState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var name = Path.Combine(environment.Output.Scope, "book.epub");
        State.Initialize(new ZipBuilder(name));
        return Task.FromResult(StepResult.Success);
    }
}

﻿//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.RenderEngine.Internals;

using Microsoft.Extensions.Logging;

namespace BookGen.RenderEngine.Functions;

internal sealed class InlineFile : Function, IInjectable
{
    private ILogger _log = null!;

    public override string Execute(FunctionArguments arguments)
    {
        string? name = arguments.GetArgumentOrThrow<string>("file");

        _log.LogDebug("Inlineing {name}...", name);

        return File.ReadAllText(name);
    }

    public void Inject(FunctionServices functionServices)
    {
        _log = functionServices.Log;
    }

    protected override FunctionInfo GetInformation()
    {
        return new FunctionInfo
        {
            Name = "InlineFile",
            Description = "Inline a file into the output",
            ArgumentInfos = new[]
            {
                new Internals.ArgumentInfo
                {
                    Name = "File",
                    Optional = false,
                    Description = "The file to inline"
                }
            },
        };
    }
}

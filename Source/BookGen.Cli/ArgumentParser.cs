//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.ArgumentParsing;

using Microsoft.Extensions.Logging;

namespace BookGen.Cli;
public class ArgumentParser<TArgs> where TArgs : ArgumentsBase, new()
{
    private readonly ArgumentParser _internalParser;

    public ArgumentParser(ILogger log)
    {
        _internalParser = new ArgumentParser(typeof(TArgs), log);
    }

    public TArgs Parse(IReadOnlyList<string> arguments)
    {
        return (TArgs)_internalParser.Fill(arguments);
    }
}

//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.ArgumentParsing;

namespace BookGen.Cli;
public class ArgumentParser<TArgs> where TArgs : ArgumentsBase, new()
{
    private readonly ArgumentParser _internalParser;

    public ArgumentParser()
    {
        _internalParser = new ArgumentParser(typeof(TArgs));
    }

    public TArgs Parse(IReadOnlyList<string> arguments)
    {
        return (TArgs)_internalParser.Fill(arguments);
    }
}

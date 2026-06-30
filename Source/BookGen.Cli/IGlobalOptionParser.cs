//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace BookGen.Cli;

public abstract class GlobalOptionParser
{
    public string LongName { get; }
    public string ShortName { get; }

    public GlobalOptionParser(string shortName, string longName)
    {
        if (longName.StartsWith('-'))
            throw new ArgumentException("Long name cannot start with '--'", nameof(longName));

        if (shortName.StartsWith('-'))
            throw new ArgumentException("Short name cannot start with '-'", nameof(shortName));
        
        ShortName = $"-{shortName}";
        LongName = $"--{longName}";
    }

    public bool TryParseGlobalOption(string[] args, [NotNullWhen(true)] out string? parsedOne)
    {
        bool handle = false;
        parsedOne = null;
        if (args.Contains(ShortName))
        {
            handle = true;
            parsedOne = ShortName;
        }
        else if (args.Contains(LongName))
        {
            handle = true;
            parsedOne = LongName;
        }

        if (handle)
        {
            OnOptionWasPresent();
        }

        return handle;
    }

    protected abstract void OnOptionWasPresent();
}

//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public abstract class GlobalOptionParser
{
    public string LongName { get; }
    public string ShortName { get; }
    public bool NeedsValue { get; }

    public GlobalOptionParser(string shortName, string longName, bool needsValue = false)
    {
        if (longName.StartsWith('-'))
            throw new ArgumentException("Long name cannot start with '--'", nameof(longName));

        if (shortName.StartsWith('-'))
            throw new ArgumentException("Short name cannot start with '-'", nameof(shortName));

        ShortName = $"-{shortName}";
        LongName = $"--{longName}";
        NeedsValue = needsValue;
    }

    public bool TryParseGlobalOption(IReadOnlyList<string> args, out List<string> parsedValues)
    {
        bool handle = false;
        parsedValues = new List<string>();

        int optionIndex = 0;

        for (int i=0; i<args.Count; i++)
        {
            if (args[i] == ShortName || args[i] == LongName)
            {
                handle = true;
                parsedValues.Add(args[i]);
                optionIndex = i;
                break;
            }
        }

        if (handle)
        {
            string value = string.Empty;
            if (NeedsValue)
            {
                int index = optionIndex + 1;
                if (index < args.Count)
                {
                    value = args[index];
                }
                else
                {
                    Console.WriteLine($"Ignored Option '{parsedValues[0]}': requires a value, but none was provided.");
                    handle = false;
                }
            }
            if (handle)
            {
                OnOptionWasPresent(value);
                parsedValues.Add(value);
            }
        }

        return handle;
    }

    protected abstract void OnOptionWasPresent(string value);
}

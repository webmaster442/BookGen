//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace BookGen.Cli.Internals;

internal static class Helpers
{
    public static SupportedOs GetCurrentOs()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return SupportedOs.Windows;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return SupportedOs.Linux;
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return SupportedOs.OsX;
        else
            return SupportedOs.None;
    }

    public static void ConfigureUtfSupport(bool enableUtf8Output)
    {
        if (enableUtf8Output)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
        }
    }

    // Skip the first argument (command name) and any parsed global options
    public static List<string> GetArgsToParse(IReadOnlyList<string> args,
                                              HashSet<string> parsedGlobals,
                                              int skipCount)
    {
        List<string> results = new();
        for (int i = skipCount; i < args.Count; i++)
        {
            if (!parsedGlobals.Contains(args[i]))
            {
                results.Add(args[i]);
            }
        }
        return results;
    }
}

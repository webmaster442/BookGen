//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.DomainServices;

public static class ShellAutoCompleteFilter
{
    public static IEnumerable<string> DoFilter(IReadOnlyList<string> candidates, string input, int cursorposition)
    {
        if (candidates.Count < 1
            || string.IsNullOrEmpty(input)
            || cursorposition < 0 
            || cursorposition > input.Length)
        {
            return Array.Empty<string>();
        }

        string prefix = input[..cursorposition];
        int prefixLength = cursorposition >= prefix.Length ? prefix.Length - 1: cursorposition;

        var filteredCommands = candidates
            .Where(cmd => cmd.StartsWith(prefix))
            .Select(cmd => cmd.Substring(prefixLength).TrimStart());

        return filteredCommands;
    }
}

//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shell.Shared;

public static class ShellAutoCompleteFilter
{
    public static IEnumerable<string> FilterCommandNames(IReadOnlyList<string> candidates,
                                                         string input,
                                                         int cursorposition,
                                                         StringComparison comparison = StringComparison.Ordinal)
    {
        if (candidates.Count < 1
            || string.IsNullOrEmpty(input)
            || cursorposition < 0
            || cursorposition > input.Length)
        {
            yield break;
        }

        string prefix = input[..cursorposition];
        int prefixLength = cursorposition >= prefix.Length ? prefix.Length - 1 : cursorposition;

        IEnumerable<string> filteredCommands = candidates
            .Where(cmd => cmd.StartsWith(prefix, comparison));

        foreach (var filtered in filteredCommands)
        {
            (int start, int _) = GetWordPositions(filtered).FirstOrDefault(p => cursorposition >= p.start && cursorposition <= p.end);
            yield return filtered[start..];
        }
    }

    public static IEnumerable<string> FilterSwitchesAndArgs(IEnumerable<string> candidates,
                                                            string input,
                                                            int cursorposition,
                                                            StringComparison comparison)
    {
        string currentWord = cursorposition > 0 && cursorposition <= input.Length
            ? input[..cursorposition].Split(' ').LastOrDefault() ?? string.Empty
            : string.Empty;

        if (string.IsNullOrEmpty(currentWord))
            yield break;

        foreach (var candidate in candidates)
        {
            if (!input.Contains(candidate, comparison))
            {
                yield return candidate;
            }
        }
    }

    public static IEnumerable<(int start, int end)> GetWordPositions(string str)
    {
        int start = 0;
        int pos = 0;
        foreach (var c in str)
        {
            if (char.IsWhiteSpace(c))
            {
                (int start, int pos) item = (start, pos);
                start = pos + 1;
                yield return item;
            }
            ++pos;
        }
        yield return (start, pos);
    }
}

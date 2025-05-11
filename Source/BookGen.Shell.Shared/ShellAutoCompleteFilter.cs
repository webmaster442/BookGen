//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shell.Shared;

public static class ShellAutoCompleteFilter
{
    public static IEnumerable<string> DoFilter(IReadOnlyList<string> candidates, string input, int cursorposition)
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

        var filteredCommands = candidates
            .Where(cmd => cmd.StartsWith(prefix));

        foreach (var filtered in filteredCommands)
        {
            var (start, _) = GetWordPositions(filtered).FirstOrDefault(p => cursorposition >= p.start && cursorposition <= p.end);
            yield return filtered[start..];
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
                var item = (start, pos);
                start = pos + 1;
                yield return item;
            }
            ++pos;
        }
        yield return (start, pos);
    }
}

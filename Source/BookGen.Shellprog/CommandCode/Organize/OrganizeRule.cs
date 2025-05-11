//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace BookGen.Domain.Organize;

public sealed class OrganizeRule
{
    public required HashSet<string> Patterns { get; init; }
    public required string Destination { get; init; }

    public Regex GetRegex()
    {
        var rules = string.Join('|', Patterns.Select(pattern => $"({WildcardToRegex(pattern)})"));
        return new Regex("^"+rules, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    private static string WildcardToRegex(string pattern)
        => $"{Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".")}$";

}

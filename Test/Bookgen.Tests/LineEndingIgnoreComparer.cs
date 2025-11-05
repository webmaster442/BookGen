//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Tests;

internal sealed class LineEndingIgnoreComparer : EqualityComparer<string?>
{
    private static string? Normalize(string? input)
        => input?.Replace("\r\n", "\n");

    public override bool Equals(string? x, string? y)
        => Normalize(x) == Normalize(y);

    public override int GetHashCode([DisallowNull] string obj)
        => Normalize(obj)!.GetHashCode();
}

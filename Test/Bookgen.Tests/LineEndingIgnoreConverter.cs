using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Tests;

internal sealed class LineEndingIgnoreConverter : EqualityComparer<string?>
{
    private static string? Normalize(string? input)
        => input?.Replace("\r\n", "\n");

    public override bool Equals(string? x, string? y)
        => Normalize(x) == Normalize(y);

    public override int GetHashCode([DisallowNull] string obj)
        => Normalize(obj)!.GetHashCode();
}

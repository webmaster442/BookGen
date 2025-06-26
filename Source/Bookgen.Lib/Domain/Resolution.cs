using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Lib.Domain;

public record struct Resolution : IParsable<Resolution>
{
    public int Width { get; set; }
    public int Height { get; set; }

    public static Resolution Parse(string s, IFormatProvider? provider)
    {
        var parts = s.Split('x');
        return new Resolution
        {
            Width = int.Parse(parts[0], provider),
            Height = int.Parse(parts[1], provider)
        };
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Resolution result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = default;
            return false;
        }

        var parts = s.Split('x');

        if (parts.Length != 2
            || !int.TryParse(parts[0], out var width)
            || !int.TryParse(parts[1], out var height))
        {
            result = default;
            return false;
        }

        result = new Resolution
        {
            Width = width,
            Height = height
        };
        return true;
    }
}

namespace BookGen.WebGui.Extensions;

public static class BrowserItemExtensions
{
    public static string ToHlslColorString(this string str)
    {
        int hash = 0;
        for (int index = 0; index < str.Length; index++)
        {
            hash += str[index] * (index + 1);
        }
        hash %= 360;
        return $"hsl({hash}, 80%, 50%)";
    }

    private const long KiB = 1024;
    private const long MiB = KiB * 1024;
    private const long GiB = MiB * 1024;

    public static string ToHumanReadableSize(this long size)
    {
        if (size < KiB)
        {
            return $"{size} Byte";
        }
        if (size < MiB)
        {
            return $"{size / KiB} KiB";
        }
        if (size < GiB)
        {
            return $"{size / MiB} MiB";
        }
        return $"{size / GiB} GiB";
    }
}
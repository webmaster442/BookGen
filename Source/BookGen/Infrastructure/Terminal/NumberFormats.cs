namespace BookGen.Infrastructure.Terminal;
internal static class NumberFormats
{
    public static string ToFileSize(this long value)
    {
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Value must be non-negative.");

        string[] sizes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        double len = value;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }
        return $"{len:0.##} {sizes[order]}";
    }
}

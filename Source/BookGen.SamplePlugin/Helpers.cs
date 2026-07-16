namespace BookGen.SamplePlugin;

internal static class Helpers
{
    public static uint GetId(string filePath)
    {
        uint result = 2166136261;
        foreach (char c in filePath)
        {
            result ^= c;
            result *= 16777619;
        }
        return result;
    }

    public static string ReadEmbeddedFile(string fileName)
    {
        using Stream stream = typeof(Helpers).Assembly.GetManifestResourceStream(fileName)
            ?? throw new InvalidOperationException($"Embedded resource '{fileName}' not found.");

        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}

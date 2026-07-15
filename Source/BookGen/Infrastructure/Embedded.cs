namespace BookGen.Infrastructure;

internal static class Embedded
{
    public static string ReadEmbeddedResource(string resourceName)
    {
        using var stream = typeof(Embedded).Assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new InvalidOperationException($"Resource '{resourceName}' not found.");
        }

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}

namespace BookGen.Infrastructure.Plugins;

internal static class PluginPathResolver
{
    public const string PluginFolder = "plugins";
    public const string PluginFileExtension = "*.plugin";
    public const string DllFileExtension = "*.dll";

    public static IEnumerable<string> GetPluginPackages()
    {
        string folder = Path.Combine(AppContext.BaseDirectory, PluginFolder);
        if (!Directory.Exists(folder))
        {
            yield break;
        }

        foreach (string file in Directory.EnumerateFiles(folder, PluginFileExtension))
        {
            yield return file;
        }
    }

    public static string? Resolve(string actualFolder, string requestedFile, bool isDevMode = false)
    {
        if (isDevMode)
        {
            return Path.GetFullPath(requestedFile, actualFolder);
        }

        string probePath = Path.GetFullPath(requestedFile, actualFolder);
        if (File.Exists(probePath))
        {
            return probePath;
        }

        probePath = Path.Combine(AppContext.BaseDirectory, PluginFolder, requestedFile);
        return File.Exists(probePath)
            ? probePath
            : null;
    }
}

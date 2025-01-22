using System.Text.Json;
using System.Text.Json.Serialization;

namespace Webmaster442.WindowsTerminal;
public static class WindowsTerminal
{
    private static readonly string _localFragments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows Terminal", "Fragments");

    private static readonly JsonSerializerOptions _serializerOptions = CreateOptions();

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
        };
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        return options;
    }

    public static string[] GetLocalFragments()
        => Directory.GetFiles(_localFragments, "*.json", SearchOption.AllDirectories);

    public static async Task<bool> TryInstallFragmentAsync(string appName, string fragmentName, TerminalFragment terminalFragment)
    {
        try
        {
            var fragmentFolder = Path.Combine(_localFragments, appName);
            if (!Directory.Exists(fragmentFolder))
            {
                Directory.CreateDirectory(fragmentFolder);
            }
            var filePath = Path.Combine(fragmentFolder, Path.ChangeExtension(fragmentName, ".json"));
            using var stream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(stream, terminalFragment, _serializerOptions);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static async Task<TerminalFragment?> ReadFragmentAsync(string appName, string fragmentName)
    {
        var filePath = Path.Combine(_localFragments, appName, Path.ChangeExtension(fragmentName, ".json"));
        if (!File.Exists(filePath))
        {
            return null;
        }
        using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<TerminalFragment>(stream, _serializerOptions);
    }
}

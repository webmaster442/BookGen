using System.Text.Json;
using System.Text.Json.Serialization;

namespace Webmaster442.WindowsTerminal;

/// <summary>
/// Windows terminal interaction class
/// </summary>
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

    /// <summary>
    /// Get all local fragment files
    /// </summary>
    /// <returns>an array of terminal json fragments present in the LocalApplicationData</returns>
    public static string[] GetLocalFragments()
        => Directory.GetFiles(_localFragments, "*.json", SearchOption.AllDirectories);

    /// <summary>
    /// Try to install a terminal fragment in the LocalApplicationData
    /// </summary>
    /// <param name="appName">App name</param>
    /// <param name="fragmentName">fragment json file name</param>
    /// <param name="terminalFragment">terminal fragment data</param>
    /// <returns>true, if installation was successfull</returns>
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

    /// <summary>
    /// Read a terminal fragment from the LocalApplicationData
    /// </summary>
    /// <param name="appName">App name</param>
    /// <param name="fragmentName">fragment json file name</param>
    /// <returns>Terminal fragment data</returns>
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

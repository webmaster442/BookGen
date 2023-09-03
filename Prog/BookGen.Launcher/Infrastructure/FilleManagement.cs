using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BookGen.Launcher.Infrastructure;

internal static class FilleManagement
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static bool ReadJson<T>(string fileName, [NotNullWhen(true)] out T? output) where T : class
    {
        if (!File.Exists(fileName))
        {
            output = default;
            return false;
        }

        try
        {
            using (var stream = File.OpenRead(fileName))
            {
                output = JsonSerializer.Deserialize<T>(stream, _jsonSerializerOptions);
                return output != null;
            }
        }
        catch (Exception ex)
        {
            Dialog.ShowMessageBox(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            output = default;
            return false;
        }

    }

    public static void WriteJson<T>(string tempName, string finalName, T content)
    {
        try
        {
            using (var stream = File.OpenWrite(tempName))
            {
                JsonSerializer.Serialize<T>(stream, content, _jsonSerializerOptions);
            }
            File.Move(tempName, finalName);
        }
        catch (Exception ex)
        {
            Dialog.ShowMessageBox(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

}

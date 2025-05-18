using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Bookgen.Lib.AppSettings;

public abstract class AppSettingsBase
{
    private readonly Dictionary<string, string> _storage;
    private readonly string _fileName;
    private readonly JsonSerializerOptions _options;

    public AppSettingsBase(string fileName)
    {
        _fileName = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), fileName);
        _storage = new Dictionary<string, string>();
        _options = new JsonSerializerOptions()
        {
            WriteIndented = true,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
        InitDefaults();
        LoadFromFile();
        RemoveUnusedSettings();
    }

    /// <summary>
    /// For migration. These are the obsolete keys, that are automatically removed
    /// </summary>
    protected abstract HashSet<string> NotNeededKeys { get; }

    /// <summary>
    /// Add all settings here that need to be present
    /// </summary>
    protected abstract void InitDefaults();

    public void Set<T>(T value, [CallerMemberName]string? propertyName = null) where T : IParsable<T>
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        string valueToSet;

        if (value is double d)
            valueToSet = d.ToString("G17", CultureInfo.InvariantCulture); //https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#GFormatString
        else if (value is float f)
            valueToSet = f.ToString("G9", CultureInfo.InvariantCulture);
        else if (value is IFormattable formattable)
            valueToSet = formattable.ToString(null, CultureInfo.InvariantCulture);
        else
            valueToSet = value.ToString() ?? throw new InvalidOperationException($"{nameof(ToString)} returned null");

        _storage[propertyName] = valueToSet;
    }

    protected T Get<T>([CallerMemberName] string? propertyName = null) where T : IParsable<T>
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        return T.Parse(_storage[propertyName], CultureInfo.InvariantCulture);
    }

    public void Save()
    {
        if (File.Exists(_fileName))
            File.Move(_fileName, Path.ChangeExtension(_fileName, ".bak"));

        using FileStream target = File.Create(_fileName);

        JsonSerializer.Serialize(target, _storage, _options);
    }

    private void LoadFromFile()
    {
        if (!File.Exists(_fileName))
            return;

        using FileStream stream = File.OpenRead(_fileName);

        Dictionary<string, string>? loaded = JsonSerializer.Deserialize<Dictionary<string, string>>(stream, _options);

        if (loaded == null)
            return;

        foreach (var item in loaded)
        {
            if (_storage.ContainsKey(item.Key))
                _storage[item.Key] = item.Value;

            _storage.Add(item.Key, item.Value);
        }
    }

    private void RemoveUnusedSettings()
    {
        foreach (var key in NotNeededKeys)
        {
            _storage.Remove(key);
        }
    }
}

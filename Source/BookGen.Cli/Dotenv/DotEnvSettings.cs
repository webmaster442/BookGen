using System.Diagnostics.CodeAnalysis;

namespace BookGen.Cli.Dotenv;

public sealed class DotEnvSettings
{
    private readonly Dictionary<string, string> _keyValues;

    public DotEnvSettings(Dictionary<string, string> keyValues)
    {
        _keyValues = keyValues;
    }

    public DotEnvSettings()
    {
        _keyValues = new Dictionary<string, string>();
    }

    public void Merge(DotEnvSettings other)
    {
        foreach (KeyValuePair<string, string> kvp in other._keyValues)
        {
            _keyValues[kvp.Key] = kvp.Value;
        }
    }

    public void Add(string key, string value)
        => _keyValues.Add(key, value);

    public void Clear()
        => _keyValues.Clear();

    public IEnumerable<string> Keys
        => _keyValues.Keys;

    public bool ContainsKey(string key)
        => _keyValues.ContainsKey(key);


    public TValue GetValueOrDefault<TValue>(string key, TValue defaultValue)
        where TValue : IParsable<TValue>
    {
        if (_keyValues.TryGetValue(key, out string? value)
            && TValue.TryParse(value, null, out TValue? parsedValue))
        {
            return parsedValue;
        }
        return defaultValue;
    }

    public bool TryGetValue<TValue>(string key, [MaybeNullWhen(false)] out TValue? value)
        where TValue : IParsable<TValue>
    {
        if (_keyValues.TryGetValue(key, out string? strValue)
            && TValue.TryParse(strValue, null, out TValue? parsedValue))
        {
            value = parsedValue;
            return true;
        }
        value = default;
        return false;
    }
}

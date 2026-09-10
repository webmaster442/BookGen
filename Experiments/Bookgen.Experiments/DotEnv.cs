using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Experiments;

internal sealed class DotEnv : IDotEnv
{
    private readonly Dictionary<string, string> _keyValues;

    internal DotEnv(Dictionary<string, string> keyValues)
    {
        _keyValues = keyValues;
    }

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

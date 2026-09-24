using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Experiments;

public interface IDotEnv
{
    IEnumerable<string> Keys { get; }
    bool ContainsKey(string key);
    TValue GetValueOrDefault<TValue>(string key, TValue defaultValue) where TValue : IParsable<TValue>;
    bool TryGetValue<TValue>(string key, [MaybeNullWhen(false)] out TValue? value) where TValue : IParsable<TValue>;
}

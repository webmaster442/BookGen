using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

using Bookgen.Lib.Pipeline;

namespace Bookgen.Lib;

public sealed class Cache : ICache
{
    private readonly ConcurrentDictionary<string, string> _cache;

    public Cache()
    {
        _cache = new ConcurrentDictionary<string, string>();
    }

    public void Add(string key, string value)
        => _cache.TryAdd(key, value);

    public void Clear()
        => _cache.Clear();

    public void Remove(string key)
        => _cache.Remove(key, out _);

    public bool TryGet(string key, [NotNullWhen(true)] out string? value)
        => _cache.TryGetValue(key, out value);
}

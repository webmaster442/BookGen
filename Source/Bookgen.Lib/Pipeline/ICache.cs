using System.Diagnostics.CodeAnalysis;

namespace Bookgen.Lib.Pipeline;

public interface ICache
{
    void Clear();
    void Add(string key, string value);
    void Remove(string key);
    bool TryGet(string key, [NotNullWhen(true)]out string? value);
}
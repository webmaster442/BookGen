//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Globalization;

namespace BookGen.RenderEngine.Internals;

internal sealed class FunctionArguments : IEnumerable<KeyValuePair<string, string>>
{
    private readonly Dictionary<string, string> _arguments;

    public FunctionArguments()
    {
        _arguments = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
    }

    public void Add(string key, string value) 
        => _arguments.Add(key, value);

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        => _arguments.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() 
        => _arguments.GetEnumerator();

    public bool HasArgument(string key)
        => _arguments.ContainsKey(key);

    public TReturn GetArgumentOrFallback<TReturn>(string argument, TReturn fallback) where TReturn: IParsable<TReturn>
    {
        return _arguments.TryGetValue(argument, out string? value)
            ? TReturn.Parse(value, CultureInfo.InvariantCulture) 
            : fallback;
    }

    public TReturn GetArgumentOrThrow<TReturn>(string argument) where TReturn : IParsable<TReturn>
    {
        return _arguments.TryGetValue(argument, out string? value)
            ? TReturn.Parse(value, CultureInfo.InvariantCulture)
            : throw new ArgumentException($"{argument} was not found");
    }
}

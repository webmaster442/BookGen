using System.Collections.Concurrent;

using Bookgen.Lib.Domain;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class StaticWebState
{
    public ConcurrentDictionary<string, SourceFile> SourceFiles { get; } = new();
}

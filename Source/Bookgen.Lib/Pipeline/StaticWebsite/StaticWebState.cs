using System.Collections.Concurrent;

using Bookgen.Lib.Domain.IO;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal sealed class StaticWebState
{
    public ConcurrentDictionary<string, FrontMatter> FrontMatterData { get; } = new();
}

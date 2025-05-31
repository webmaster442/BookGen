using Bookgen.Lib.Templates;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

public sealed class StaticViewData : ViewData
{
    public required string Toc { get; init; }
}

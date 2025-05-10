using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.JsInterop;

namespace Bookgen.Lib.Markdown;

public sealed class RenderSettings : IDisposable
{
    public required string? HostUrl { get; init; }
    public required PrismJsInterop? PrismJsInterop { get; init; }
    public required CssClasses CssClasses { get; init; }
    public required bool DeleteFirstH1 { get; init; }
    public int OffsetHeadingsBy { get; init; } = 0;

    public void Dispose()
    {
        PrismJsInterop?.Dispose();
    }
}

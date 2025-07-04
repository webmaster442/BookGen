using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.VsCode;

public sealed class Presentation
{
    [JsonPropertyName("reveal")]
    public required Reveal Reveal { get; set; }
    [JsonPropertyName("panel")]
    public required PresentationPanel Panel { get; set; }
    [JsonPropertyName("clear")]
    public bool Clear { get; set; }
    [JsonPropertyName("showReuseMessage")]
    public bool ShowReuseMessage { get; set; }
}

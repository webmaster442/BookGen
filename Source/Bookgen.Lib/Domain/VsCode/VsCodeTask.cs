using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.VsCode;

public sealed class VsCodeTask
{
    [JsonPropertyName("label")]
    public required string Label { get; set; }
    [JsonPropertyName("detail")]
    public required string Detail { get; set; }
    [JsonPropertyName("command")]
    public required string Command { get; set; }
    [JsonPropertyName("type")]
    public required TaskType Type { get; set; }
    [JsonPropertyName("args")]
    public required string[] Args { get; set; }
    [JsonPropertyName("group")]
    public required Group Group { get; set; }
    [JsonPropertyName("presentation")]
    public required Presentation Presentation { get; set; }
}

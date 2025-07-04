using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.VsCode;

public sealed class VsCodeTasks
{
    [JsonPropertyName("version")]
    public string Version { get; } = "2.0.0";
    [JsonPropertyName("tasks")]
    public required List<VsCodeTask> Tasks { get; set; }
}

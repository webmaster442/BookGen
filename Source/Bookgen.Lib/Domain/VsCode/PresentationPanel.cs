using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.VsCode;

public enum PresentationPanel
{
    [JsonStringEnumMemberName("shared")]
    Shared,
    [JsonStringEnumMemberName("dedicated")]
    Dedicated,
    [JsonStringEnumMemberName("new")]
    New
}
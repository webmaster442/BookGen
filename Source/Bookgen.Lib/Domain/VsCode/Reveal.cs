using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.VsCode;

public enum Reveal
{
    [JsonStringEnumMemberName("never")]
    Never,
    [JsonStringEnumMemberName("always")]
    Always,
    [JsonStringEnumMemberName("silent")]
    Silent
}

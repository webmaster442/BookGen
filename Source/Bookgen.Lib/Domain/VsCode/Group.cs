using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.VsCode;

public enum Group
{
    [JsonStringEnumMemberName("none")]
    None,
    [JsonStringEnumMemberName("build")]
    Build,
    [JsonStringEnumMemberName("test")]
    Test,
}

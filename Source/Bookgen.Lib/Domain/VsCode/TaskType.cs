using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.VsCode;

public enum TaskType
{
    [JsonStringEnumMemberName("process")]
    Process,
    [JsonStringEnumMemberName("shell")]
    Shell
}

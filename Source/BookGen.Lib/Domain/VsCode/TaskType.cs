//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace BookGen.Lib.Domain.VsCode;

public enum TaskType
{
    [JsonStringEnumMemberName("process")]
    Process,
    [JsonStringEnumMemberName("shell")]
    Shell
}

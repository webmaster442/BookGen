//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

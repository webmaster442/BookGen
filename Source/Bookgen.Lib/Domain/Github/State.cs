//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.Github;

/// <summary>
/// State of the release asset.
/// </summary>
public enum State
{
    [JsonStringEnumMemberName("open")]
    Open,
    [JsonStringEnumMemberName("uploaded")]
    Uploaded
};

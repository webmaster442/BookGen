//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace BookGen.Lib.Domain.Github;

/// <summary>
/// A release.
/// </summary>
public class Release
{
    [JsonPropertyName("assets")]
    public required ReleaseAsset[] Assets { get; set; }
}

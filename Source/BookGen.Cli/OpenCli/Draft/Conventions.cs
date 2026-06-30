//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace BookGen.Cli.OpenCli.Draft;

/// <summary>
/// The conventions used by the CLI
/// </summary>
public sealed class Conventions
{
    /// <summary>
    /// Whether or not grouping of short options are allowed
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("groupOptions")]
    public bool? GroupOptions { get; set; }

    /// <summary>
    /// The option argument separator
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("optionSeparator")]
    public string? OptionSeparator { get; set; }
}

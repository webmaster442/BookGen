//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace BookGen.Cli.OpenCli.Draft;

public sealed class Document
{
    /// <summary>
    /// The root command
    /// </summary>
    [JsonPropertyName("command")]
    public required Command Command { get; set; }

    [JsonPropertyName("commands")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Command>? Commands { get; set; }

    /// <summary>
    /// The conventions used by the CLI
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("conventions")]
    public Conventions? Conventions { get; set; }

    /// <summary>
    /// Information about the CLI
    /// </summary>
    [JsonPropertyName("info")]
    public required CliInfo Info { get; set; }

    /// <summary>
    /// The OpenCLI version number
    /// </summary>
    [JsonPropertyName("opencli")]
    public required string Opencli { get; set; } = "0.1";
}

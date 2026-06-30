//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace BookGen.Cli.OpenCli.Draft;

/// <summary>
/// The root command
/// </summary>
public sealed class Command
{
    /// <summary>
    /// The command aliases
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("aliases")]
    public List<string>? Aliases { get; set; }

    /// <summary>
    /// The command arguments
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("arguments")]
    public List<Argument>? Arguments { get; set; }

    /// <summary>
    /// The command's sub commands
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("commands")]
    public List<Command>? Commands { get; set; }

    /// <summary>
    /// The command description
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Examples of how to use the command
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("examples")]
    public List<string>? Examples { get; set; }

    /// <summary>
    /// The command's exit codes
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("exitCodes")]
    public List<ExitCode>? ExitCodes { get; set; }

    /// <summary>
    /// Whether or not the command is hidden
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("hidden")]
    public bool? Hidden { get; set; }

    /// <summary>
    /// Indicate whether or not the command requires interactive input
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("interactive")]
    public bool? Interactive { get; set; }

    /// <summary>
    /// Custom metadata
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("metadata")]
    public List<Metadata>? Metadata { get; set; }

    /// <summary>
    /// The command name
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The command options
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("options")]
    public List<Option>? Options { get; set; }
}

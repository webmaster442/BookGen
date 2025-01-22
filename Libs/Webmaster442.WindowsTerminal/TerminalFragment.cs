using System.Text.Json.Serialization;

namespace Webmaster442.WindowsTerminal;

/// <summary>
/// Represents a terminal fragment
/// </summary>
public class TerminalFragment
{
    /// <summary>
    /// Profiles in the fragment
    /// </summary>
    [JsonPropertyName("profiles")]
    public List<TerminalProfile> Profiles { get; } = new();

    /// <summary>
    /// Schemes in the fragment
    /// </summary>
    [JsonPropertyName("schemes")]
    public List<TerminalScheme> Schemes { get; } = new();
}

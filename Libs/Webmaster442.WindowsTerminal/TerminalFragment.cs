using System.Text.Json.Serialization;

namespace Webmaster442.WindowsTerminal;

public class TerminalFragment
{
    [JsonPropertyName("profiles")]
    public List<TerminalProfile> Profiles { get; } = new();

    [JsonPropertyName("schemes")]
    public List<TerminalScheme> Schemes { get; } = new();
}

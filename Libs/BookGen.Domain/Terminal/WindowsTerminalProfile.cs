//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace BookGen.Domain.Terminal;

public class WindowsTerminalProfile
{
    [JsonPropertyName("name")]
    public string Name { get; init; }

    [JsonPropertyName("commandline")]
    public string CommandLine { get; init; }

    [JsonPropertyName("startingDirectory")]
    public string StartingDirectory { get; init; }

    [JsonPropertyName("icon")]
    public string Icon { get; init; }

    [JsonPropertyName("tabTitle")]
    public string TabTitle { get; init; }

    [JsonPropertyName("hidden")]
    public bool Hidden { get; init; }

    public WindowsTerminalProfile()
    {
        Name = string.Empty;
        CommandLine = string.Empty;
        StartingDirectory = string.Empty;
        Icon = string.Empty;
        TabTitle = string.Empty;
    }
}

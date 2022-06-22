//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace BookGen.Domain.Terminal;

public class TerminalFragment
{
    [JsonPropertyName("profiles")]
    public List<WindowsTerminalProfile> Profiles { get; set; } = new();
}

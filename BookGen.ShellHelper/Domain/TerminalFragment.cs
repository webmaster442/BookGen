//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookGen.ShellHelper.Domain
{
    public class TerminalFragment
    {
        [JsonPropertyName("profiles")]
        public List<WindowsTerminalProfile> Profiles { get; set; } = new();
    }
}

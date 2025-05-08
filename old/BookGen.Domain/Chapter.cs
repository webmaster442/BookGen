//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Domain
{
    [DebuggerDisplay("{Title}")]
    public sealed class Chapter
    {
        public string Title { get; set; }
        public List<string> Files { get; set; }

        public Chapter()
        {
            Title = string.Empty;
            Files = new List<string>();
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace BookGen.Domain.ArgumentParsing
{
    internal enum ChaptersAction
    {
        [Description("Scan *.md files and create chapters document")]
        Scan,
        [Description("Converts chapters document to summary")]
        GenSummary
    }
}

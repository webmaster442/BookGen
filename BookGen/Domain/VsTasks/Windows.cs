//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Domain.VsTasks
{
    public class Windows
    {
#pragma warning disable IDE1006 // Naming Styles
        public string? command { get; set; }
        public List<Arg>? args { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}

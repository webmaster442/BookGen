//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Contracts
{
    internal interface IModuleCollection
    {
        IEnumerable<BaseModule>? Modules { get; set; }
    }
}

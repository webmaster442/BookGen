//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using System.Collections.Generic;

namespace BookGen.Contracts
{
    internal interface IModuleCollection
    {
        IEnumerable<ModuleBase>? Modules { get; set; }
    }
}

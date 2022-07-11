//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;

namespace BookGen.Infrastructure
{
    internal interface IModuleCollection
    {
        IEnumerable<ModuleBase>? Modules { get; set; }
    }
}

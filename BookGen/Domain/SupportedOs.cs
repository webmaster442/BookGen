//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Domain
{
    [Flags]
    internal enum SupportedOs
    {
        None = 0,
        Windows = 1,
        Linux = 2,
        OsX = 4,
    }
}

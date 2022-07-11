//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    [Flags]
    public enum SupportedOs
    {
        None = 0,
        Windows = 1,
        Linux = 2,
        OsX = 4,
    }
}

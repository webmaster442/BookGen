//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    internal enum ExitCode
    {
        Succes = 0,
        Exception = -1,
        UnknownCommand = 1,
        BadParameters = 2,
        BadConfiguration = 3,
    }
}

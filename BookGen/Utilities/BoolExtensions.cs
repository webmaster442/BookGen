//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;

namespace BookGen.Utilities
{
    internal static class BoolExtensions
    {
        public static ModuleRunResult ToSuccesOrError(this bool boolean)
        {
            if (boolean) return ModuleRunResult.Succes;
            else return ModuleRunResult.GeneralError;
        }
    }
}

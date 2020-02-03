//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Contracts
{
    internal interface IAppSetting
    {
        int NodeJsTimeout { get; }
        string NodeJsPath { get; }
    }
}

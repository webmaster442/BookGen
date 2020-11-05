//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Contracts
{
    public interface IAppSetting
    {
        int NodeJsTimeout { get; }
        string NodeJsPath { get; }
        string PhpPath { get; }
        int PhpTimeout { get; }
        string PythonPath { get; }
        int PythonTimeout { get; }
        string AppDataPath { get; }
    }
}

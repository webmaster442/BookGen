//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Contracts
{
    internal interface IMoudleApi
    {
        GeneratorRunner CreateRunner(bool verbose, string workDir);
        void ExecuteModule(string module, string[] arguments);
    }
}

//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Infrastructure
{
    internal interface IModuleApi
    {
        GeneratorRunner CreateRunner(bool verbose, string workDir);
        void ExecuteModule(string module, string[] arguments);
        void Wait(string exitString);
        IEnumerable<string> GetCommandNames();
        string[] GetAutoCompleteItems(string commandName);
    }
}

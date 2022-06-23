//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;

namespace BookGen.Infrastructure
{
    internal interface IModuleApi
    {
        GeneratorRunner CreateRunner(bool verbose, string workDir)
        {
            Program.CurrentState.Log.LogLevel = verbose ? LogLevel.Detail : LogLevel.Info;
            return new GeneratorRunner(Program.CurrentState.Log, Program.CurrentState.ServerLog, workDir);
        }
        
        void ExecuteModule(string module, string[] arguments)
        {
            Program.RunModule(module, arguments, skipLockCheck: true);
        }
    }

    internal class ModuleApi : IModuleApi { }
}

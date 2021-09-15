//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;

namespace BookGen
{
    internal class ModuleApi : IMoudleApi
    {
        public GeneratorRunner CreateRunner(bool verbose, string workDir)
        {
            Program.CurrentState.Log.LogLevel = verbose ? LogLevel.Detail : LogLevel.Info;
            return new GeneratorRunner(Program.CurrentState.Log, Program.CurrentState.ServerLog, workDir);
        }
    }
}

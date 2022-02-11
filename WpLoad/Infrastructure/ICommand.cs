//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using WpLoad.Domain;

namespace WpLoad.Infrastructure
{
    internal interface ICommand : ICommandBase
    {
        /// <summary>
        /// Command code entry point
        /// </summary>
        /// <param name="log">current output log</param>
        /// <param name="arguments">arguments passed to program without subcommand</param>
        /// <returns>Exit code of command</returns>
        ExitCode Execute(ILog log, IReadOnlyList<string> arguments);
    }
}

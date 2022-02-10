using WpLoad.Domain;

namespace WpLoad.Infrastructure
{
    internal interface IAsyncCommand : ICommandBase
    {
        /// <summary>
        /// Command code entry point
        /// </summary>
        /// <param name="log">current output log</param>
        /// <param name="arguments">arguments passed to program without subcommand</param>
        /// <returns>Exit code of command</returns>
        Task<ExitCode> Execute(ILog log, IReadOnlyList<string> arguments);
    }
}

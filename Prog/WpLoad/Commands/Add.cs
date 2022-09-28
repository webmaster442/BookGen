//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using WpLoad.Domain;
using WpLoad.Infrastructure;
using WpLoad.Services;

namespace WpLoad.Commands
{
    internal sealed class Add : IAsyncCommand
    {
        public string CommandName => nameof(Add);

        public async Task<ExitCode> Execute(ILog log, IReadOnlyList<string> arguments)
        {
            if (arguments.TryGetArgument(0, out string? name))
            {
                log.Info("Creating new profile & opening editor...");
                SiteServices.WriteDefault(name);

                log.Info("Waiting for editor to close...");
                await SiteServices.OpenEditorAndWaitClose(name);

                return ExitCode.Success;
            }
            return ExitCode.BadParameters;
        }
    }
}

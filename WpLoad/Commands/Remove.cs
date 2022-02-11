//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using WpLoad.Domain;
using WpLoad.Infrastructure;
using WpLoad.Services;

namespace WpLoad.Commands
{
    internal class Remove : ICommand
    {
        public string CommandName => nameof(Remove);

        public ExitCode Execute(ILog log, IReadOnlyList<string> arguments)
        {
            if (arguments.TryGetArgument(0, out string? name))
            {
                if (SiteServices.TryRemove(name))
                {
                    log.Info($"Removed profile: {name}");
                    return ExitCode.Success;
                }
                else
                {
                    log.Error($"Profile doen't exist: {name}");
                    return ExitCode.Fail;
                }
            }
            return ExitCode.BadParameters;
        }
    }
}

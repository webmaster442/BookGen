//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using WpLoad.Domain;
using WpLoad.Infrastructure;

namespace WpLoad.Commands
{
    internal sealed class Help : ICommand
    {
        public string CommandName => nameof(Help);

        public ExitCode Execute(ILog log, IReadOnlyList<string> arguments)
        {
            string? helpText = Properties.Resources.Help;
            log.Info(helpText);
            return ExitCode.Success;
        }
    }
}

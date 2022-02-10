using WpLoad.Domain;
using WpLoad.Infrastructure;

namespace WpLoad.Commands
{
    internal class Help : ICommand
    {
        public string CommandName => nameof(Help);

        public ExitCode Execute(ILog log, IReadOnlyList<string> arguments)
        {
            var helpText = Properties.Resources.Help;
            log.Info(helpText);
            return ExitCode.Success;
        }
    }
}

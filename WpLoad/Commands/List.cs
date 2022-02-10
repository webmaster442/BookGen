using WpLoad.Domain;
using WpLoad.Infrastructure;
using WpLoad.Services;

namespace WpLoad.Commands
{
    internal class List : ICommand
    {
        public string CommandName => nameof(List);

        public ExitCode Execute(ILog log, IReadOnlyList<string> arguments)
        {
            string[] profiles = SiteServices.ListProfiles();
            if (profiles.Length < 1)
            {
                log.Info("No profiles are availabe");
                return ExitCode.Warning;
            }

            log.Info("Available profiles:\n");
            foreach (var profile in profiles)
            {
                log.Info($"  {profile}");
            }
            return ExitCode.Success;
        }
    }
}

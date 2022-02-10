using WpLoad.Domain;
using WpLoad.Infrastructure;
using WpLoad.Services;

namespace WpLoad.Commands
{
    internal class Add : IAsyncCommand
    {
        public string CommandName => nameof(Add);

        public async Task<ExitCode> Execute(ILog log, IReadOnlyList<string> arguments)
        {
            if (arguments.TryGetArgument(0, out string? name))
            {
                log.Info("Creating new profile & opening editor...");
                string newTempFile = SiteServices.CreateRandomName();
                SiteServices.WriteDefault(newTempFile);

                log.Info("Waiting for editor to close...");
                await EditorService.OpenAndWaitClose(newTempFile);

                log.Info("Encrypting...");
                SiteServices.EncryptAndDeleteTemp(newTempFile, name);
                return ExitCode.Success;
            }
            return ExitCode.BadParameters;
        }
    }
}

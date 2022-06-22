//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using WordPressPCL;
using WpLoad.Domain;
using WpLoad.Services;

namespace WpLoad.Infrastructure
{
    internal abstract class LoadCommandBase : IAsyncCommand
    {
        public abstract string CommandName { get; }
        public abstract Task<ExitCode> Execute(ILog log, IReadOnlyList<string> arguments);

        protected static bool TryConfigureFolderAndClient(ILog log,
                                                          UpArguments arguments,
                                                          [NotNullWhen(true)] out WordPressClient? client)
        {
            if (!Directory.Exists(arguments.Path))
            {
                log.Error($"{arguments.Path} doesn't exist");
                client = null;
                return false;
            }

            log.Info("Configuring connection...");
            if (!ClientService.TryConfifgureConnection(arguments.Site, out client))
            {
                log.Error($"{arguments.Site} profile doesn't exist");
                return false;
            }
            return true;
        }
    }
}

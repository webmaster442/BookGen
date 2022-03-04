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


        protected bool ConfigureFolder(ILog log, IReadOnlyList<string> arguments, out string folder)
        {
            if (arguments.TryGetArgument(1, out string? argument))
            {
                folder = argument;
            }
            else
            {
                folder = Environment.CurrentDirectory;
            }

            if (!Directory.Exists(folder))
            {
                log.Error($"{folder} doesn't exist");
                return false;
            }
            return true;
        }


        protected bool ConfigureClient(ILog log, string site, [NotNullWhen(true)] out WordPressClient? client)
        {
            log.Info("Configuring connection...");
            if (!ClientService.TryConfifgureConnection(site, out client))
            {
                log.Error($"{site} profile doesn't exist");
                return false;
            }
            return true;
        }

    }
}

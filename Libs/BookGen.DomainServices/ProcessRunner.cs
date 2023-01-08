//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Medallion.Shell;

namespace BookGen.DomainServices
{
    public static class ProcessRunner
    {
        public static (int exitcode, string output) RunProcess(string programPath,
                                                              string argument,
                                                              int timeOutSeconds,
                                                              string? workdir = null)
        {
            return RunProcess(programPath, new string[] { argument }, timeOutSeconds, workdir);
        }

        public static (int exitcode, string output) RunProcess(string programPath,
                                                               string[] arguments,
                                                               int timeOutSeconds,
                                                               string? workdir = null)
        {
            using var command = Command.Run(programPath, arguments, (options) =>
            {
                options.Timeout(TimeSpan.FromSeconds(timeOutSeconds));

                if (!string.IsNullOrEmpty(workdir))
                    options.WorkingDirectory(workdir);
            });

            command.Wait();

            return (command.Result.ExitCode, command.Result.StandardOutput);
        }
    }
}

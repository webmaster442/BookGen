//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using System.Diagnostics;

namespace BookGen.DomainServices
{
    public static class ProcessRunner
    {
        public static (int exitcode, string output) RunProcess(string command, string arguments, int timeout, string workdir = "")
        {
            (int exitcode, string output) result = (-1, string.Empty);

            using (var process = new Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                if (!string.IsNullOrEmpty(workdir))
                    process.StartInfo.WorkingDirectory = workdir;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();

                if (process.WaitForExit(timeout * 1000))
                {
                    result.output = output;
                    result.exitcode = process.ExitCode;
                }
                else
                {
                    result.exitcode = -1;
                    result.output = err;
                    process.Kill();
                }
            }
            return result;
        }
    }
}

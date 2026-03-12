//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Shell.Shared;

public static class ProcessRunner
{
    public static void OpenUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            throw new ArgumentException("invalid url", nameof(url));
           
        using (var process = new Process())
        {
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = url;
            process.Start();
        }
    }

    public static (int exitcode, string output, string error) RunProcess(string programPath,
                                                                         string argument,
                                                                         int timeOutSeconds,
                                                                         string? workdir = null)
        => RunProcess(programPath, [argument], timeOutSeconds, workdir);

    public static (int exitcode, string output, string error) RunProcess(string programPath,
                                                                         string[] arguments,
                                                                         int timeOutSeconds,
                                                                         string? workdir = null)
    {

        using (var process = new Process())
        {
            process.StartInfo.FileName = programPath;
            SetArguments(process.StartInfo, arguments);
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            if (workdir != null)
                process.StartInfo.WorkingDirectory = workdir;

            Task timeout = Task.Delay(timeOutSeconds * 1000);
            process.Start();
            Task<string> read = process.StandardOutput.ReadToEndAsync();
            Task<string> errorRead = process.StandardError.ReadToEndAsync();
            if (Task.WaitAny(timeout, read, errorRead) == 0)
            {
                return (-1, string.Empty, "Timeout");
            }
            else
            {
                return (process.ExitCode, read.Result, errorRead.Result);
            }
        }
    }

    private static void SetArguments(ProcessStartInfo startInfo, string[] arguments)
    {
        foreach (var arg in arguments)
            startInfo.ArgumentList.Add(arg);
    }
}

//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

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

        private static Task RunShell(string shell, string arguments)
        {
            using var process = new Process();
            process.StartInfo.FileName = shell;
            process.StartInfo.Arguments = arguments;
            process.Start();
            return process.WaitForExitAsync();
        }

        public static Task RunCmdScript(string shellScript)
            => RunShell("cmd.exe", $"\"{shellScript}\"");

        public static Task RunPowershellScript(string shellScript)
        {
            var installStatus = InstallDetector.GetInstallStatus();
            if (installStatus.IsPsCoreInstalled)
                return RunShell(InstallDetector.PowershellCoreExe, $"-ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"");
            else
                return RunShell("powershell.exe", $"-ExecutionPolicy Bypass -NoExit -File \"{shellScript}\"");
        }


        public static (int exitcode, string output) RunProcess(string programPath,
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
                process.StartInfo.CreateNoWindow = true;
                Task timeout = Task.Delay(timeOutSeconds * 1000);
                process.Start();
                Task<string> read = process.StandardOutput.ReadToEndAsync();
                if (Task.WaitAny(timeout, read) == 0)
                {
                    return (-1, "Timeout");
                }
                else
                {
                    return (process.ExitCode, read.Result);
                }
            }
        }

        public static string GetCommandOutput(string program, string[] arguments, string stdIn, int timeoutSeconds)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = program;
                SetArguments(process.StartInfo, arguments);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.CreateNoWindow = true;

                Task timeout = Task.Delay(timeoutSeconds * 1000);
                process.Start();
                process.StandardInput.Write(stdIn);
                process.StandardInput.Close();
                Task<string> read = process.StandardOutput.ReadToEndAsync();
                if (Task.WaitAny(timeout, read) == 0)
                {
                    return "timeout";
                }
                return read.Result;
            }
        }

        private static void SetArguments(ProcessStartInfo startInfo, string[] arguments)
        {
            foreach (var arg in arguments)
                startInfo.ArgumentList.Add(arg);
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Text;

using BookGen.Api;

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

        private static void RunShell(string shell, string arguments, ILog log)
        {
            try
            {
                using var process = new Process();
                process.StartInfo.FileName = shell;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e) 
            {
                log.Warning(e);
            }
        }

        public static void RunCmdScript(string shellScript, ILog log)
            => RunShell("cmd.exe", $"\"{shellScript}\"", log);

        public static void RunPowershellScript(string shellScript, ILog log)
        {
            var installStatus = InstallDetector.GetInstallStatus();
            if (installStatus.IsPsCoreInstalled)
                RunShell(InstallDetector.PowershellCoreExe, $"-ExecutionPolicy Bypass -File \"{shellScript}\"", log);
            else
                RunShell("powershell.exe", $"-ExecutionPolicy Bypass -File \"{shellScript}\"", log);
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
                
                if (workdir != null)
                    process.StartInfo.WorkingDirectory = workdir;

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

        public static (string stdOut, string stdErr) GetCommandOutput(string program, string[] arguments, string stdIn, int timeoutSeconds)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = program;
                SetArguments(process.StartInfo, arguments);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.StandardInputEncoding = Encoding.UTF8;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;

                Task timeout = Task.Delay(timeoutSeconds * 1000);
                process.Start();
                process.StandardInput.Write(stdIn);
                process.StandardInput.Close();
                Task<string> @out = process.StandardOutput.ReadToEndAsync();
                Task<string> err = process.StandardError.ReadToEndAsync();
                if (Task.WaitAny(timeout, @out, err) == 0)
                {
                    return ("", "timeout");
                }
                return (@out.Result, err.Result);
            }
        }

        private static void SetArguments(ProcessStartInfo startInfo, string[] arguments)
        {
            foreach (var arg in arguments)
                startInfo.ArgumentList.Add(arg);
        }
    }
}

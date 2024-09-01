//-----------------------------------------------------------------------------
// (c) 2020-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Text;

using Microsoft.Extensions.Logging;

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

        private static void RunShell(string shell, string arguments, ILogger log)
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
                log.LogWarning(e, e.Message);
            }
        }

        public static void RunCmdScript(string shellScript, ILogger log)
            => RunShell("cmd.exe", $"\"{shellScript}\"", log);

        public static void RunPowershellScript(string shellScript, ILogger log)
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

        private static IProgress<string>? _reporter;

        public static int RunProcess(string programPath,
                                     string[] arguments,
                                     string? workdir,
                                     IProgress<string> progress)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = programPath;
                SetArguments(process.StartInfo, arguments);
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.StartInfo.StandardErrorEncoding = Encoding.UTF8;
                process.OutputDataReceived += OnOutput;
                process.ErrorDataReceived += OnOutput;
                _reporter = progress;

                if (workdir != null)
                    process.StartInfo.WorkingDirectory = workdir;

                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();

                process.OutputDataReceived -= OnOutput;
                process.ErrorDataReceived -= OnOutput;
                _reporter = null;

                return process.ExitCode;
            }
        }

        private static void OnOutput(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                _reporter?.Report(e.Data);
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

        public static void RunProcess(string v, object value, string workdir, object onReportProgress)
        {
            throw new NotImplementedException();
        }
    }
}

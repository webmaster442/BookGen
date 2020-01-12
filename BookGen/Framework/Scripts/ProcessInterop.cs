//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Framework.Scripts
{
    internal static class ProcessInterop
    {
        private static Task<bool> WaitForExitAsync(Process process, string stdin, int timeout)
        {
            return Task.Run(() =>
            {
                process.StandardInput.Write(stdin);
                return process.WaitForExit(timeout);
            });
        }

        public static async Task<(int exitcode, string output)> RunProcess(string command, string arguments, string stdin, int timeout)
        {
            (int exitcode, string output) result = (-1, string.Empty);

            using (var process = new Process())
            {
                // If you run bash-script on Linux it is possible that ExitCode can be 255.
                // To fix it you can try to add '#!/bin/bash' header to the script.

                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                var outputBuilder = new StringBuilder();
                var outputCloseEvent = new TaskCompletionSource<bool>();

                process.OutputDataReceived += (s, e) =>
                {
                    // The output stream has been closed i.e. the process has terminated
                    if (e.Data == null)
                        outputCloseEvent.SetResult(true);
                    else
                        outputBuilder.AppendLine(e.Data);
                };

                var errorBuilder = new StringBuilder();
                var errorCloseEvent = new TaskCompletionSource<bool>();

                process.ErrorDataReceived += (s, e) =>
                {
                    // The error stream has been closed i.e. the process has terminated
                    if (e.Data == null)
                        errorCloseEvent.SetResult(true);
                    else
                        errorBuilder.AppendLine(e.Data);
                };

                bool isStarted;

                try
                {
                    isStarted = process.Start();
                }
                catch (Exception error)
                {
                    // Usually it occurs when an executable file is not found or is not executable

                    result.exitcode = -1;
                    result.output = error.Message;
                    isStarted = false;
                }

                if (!isStarted)
                    return result;

                // Reads the output stream first and then waits because deadlocks are possible
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Creates task to wait for process exit using timeout
                var waitForExit = WaitForExitAsync(process, stdin, timeout);

                // Create task to wait for process exit and closing all output streams
                var processTask = Task.WhenAll(waitForExit, outputCloseEvent.Task, errorCloseEvent.Task);

                // Waits process completion and then checks it was not completed by timeout
                if (await Task.WhenAny(Task.Delay(timeout), processTask) == processTask && waitForExit.Result)
                {
                    result.exitcode = process.ExitCode;

                    // Adds process output if it was completed with error
                    if (process.ExitCode != 0)
                    {
                        result.output = $"{outputBuilder}{errorBuilder}";
                    }

                    return result;
                }

                try
                {
                    // Kill hung process
                    process.Kill();
                }
                catch
                {
                    //Intentionally left empty
                }
            }
            return result;
        }

        public static string? ResolveProgramFullPath(string programName)
        {
            string? pathVar = Environment.GetEnvironmentVariable("path");

            if (pathVar == null)
                return null;

            string[] searchFolders = pathVar.Split(';');

            foreach (string folder in searchFolders)
            {
                string programFile = Path.Combine(folder, programName);

                if (File.Exists(programFile))
                {
                    return programFile;
                }
            }

            return null;
        }

        internal static string AppendExecutableExtension(string file)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return file;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return $"{file}.exe";
            }
            else
            {
                throw new InvalidOperationException("Unknown host operating system");
            }
        }
    }
}

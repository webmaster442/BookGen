//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Framework.Scripts
{
    internal static class ProcessInterop
    {
        public static (int exitcode, string output) RunProcess(string command, string arguments, int timeout)
        {
            (int exitcode, string output) result = (-1, string.Empty);

            using (var process = new Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string err = process.StandardError.ReadToEnd();

                if (process.WaitForExit(timeout))
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

        public static string? ResolveProgramFullPath(string programName)
        {
            string? pathVar = Environment.GetEnvironmentVariable("path");

            if (pathVar == null)
                return null;

            var searchFolders = new List<string>(pathVar.Split(';'));

            if (AppDomain.CurrentDomain.BaseDirectory != null)
                searchFolders.Add(AppDomain.CurrentDomain.BaseDirectory);

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

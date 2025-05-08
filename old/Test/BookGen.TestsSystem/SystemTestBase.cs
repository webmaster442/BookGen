//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace BookGen.TestsSystem
{
    public abstract class SystemTestBase
    {
        private readonly string _workDir;

        protected string LastLog { get; private set; }

        protected TestEnvironment Environment { get; }

        protected SystemTestBase(string workDir)
        {
            _workDir = Path.Combine(AppContext.BaseDirectory, workDir);
            Environment = new TestEnvironment(_workDir);
            LastLog = string.Empty;
        }

        protected void EnsureRunWithoutException(int expectedExitCode, string commandLine)
        {
            var process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = GetFileName();
            process.StartInfo.Arguments = $"{commandLine} -js";
            process.StartInfo.WorkingDirectory = _workDir;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.Start();
            string st = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            LastLog = st;
            if ((int)expectedExitCode != process.ExitCode)
            {
                string? logmsg = GetLastLogMsg();
                Assert.Fail(logmsg+$"\r\nExit code: {process.ExitCode}");
            }
        }

        private string GetLastLogMsg()
        {
            if (LastLog?.Any() != true)
                return "No log can be shown. Log was empty";

            var lines = LastLog.Split('\n');

            return lines.LastOrDefault() ?? "No log can be shown. Log was empty";
        }

        protected abstract void CleanFiles();

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            CleanFiles();
        }

        private static string GetFileName()
        {
            return Path.Combine(AppContext.BaseDirectory, "BookGen.exe");
        }
    }
}

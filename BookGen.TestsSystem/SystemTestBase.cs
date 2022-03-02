//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace BookGen.TestsSystem
{
    public abstract class SystemTestBase
    {
        private readonly string _workDir;

        public List<LogEntry> LastLog { get; private set; }

        public TestEnvironment Environment { get; }

        protected SystemTestBase(string workDir)
        {
            _workDir = Path.Combine(AppContext.BaseDirectory, workDir);
            Environment = new TestEnvironment(_workDir);
            LastLog = new List<LogEntry>();
        }

        public void EnsureRunWithoutException(ExitCode expectedExitCode, string commandLine)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName =  GetFileName();
            process.StartInfo.Arguments = $"{commandLine} -js";
            process.StartInfo.WorkingDirectory = _workDir;
            process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            process.Start();
            string st = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            LastLog = GetLog(st);
            if ((int)expectedExitCode != process.ExitCode)
            {
                var logmsg = GetLastLogMsg();
                Assert.Fail(logmsg);
            }
        }

        private static List<LogEntry> GetLog(string output)
        {
            try
            {
                var list = JsonSerializer.Deserialize<List<LogEntry>>(output);
                return list ?? new List<LogEntry>();
            }
            catch (Exception)
            {
                return new List<LogEntry>();
            }
        }

        private string GetLastLogMsg()
        {
            if (LastLog == null 
                || !LastLog.Any())
                return "No log can be shown. Log was empty";

            var entry = LastLog.OrderByDescending(l => l.TimeStamp).First();
            return entry.Message;
        }

        public abstract void CleanFiles();

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

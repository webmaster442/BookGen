using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;

namespace BookGen.Tests.EndTest
{
    [TestFixture]
    [SingleThreaded]
    public class EndTests
    {
        private string _tempDir;
        private const int OneMinute = 60 * 1000;

        [OneTimeSetUp]
        public void Setup()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "BookGenTests");
            if (!Directory.Exists(_tempDir))
                Directory.CreateDirectory(_tempDir);

            var testFiles = Directory.GetFiles(Environment.TestEnvironment.GetBookStub(), "*.*");
            foreach(var testfile in testFiles)
            {
                var fname = Path.GetFileName(testfile);
                File.Copy(testfile, Path.Combine(_tempDir, fname));
            }
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            if (Directory.Exists(_tempDir))
                Directory.Delete(_tempDir, true);
        }

        private int RunBookGen(string action, out double runtimeMs, out string output, int timeout = OneMinute)
        {
            Process process = new Process();
            process.StartInfo.FileName = Environment.TestEnvironment.GetBookGenPath();
            process.StartInfo.WorkingDirectory = _tempDir;
            process.StartInfo.Arguments = $"-a {action} -n";
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            DateTime start = DateTime.Now;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit(timeout);
            runtimeMs = (DateTime.Now - start).TotalMilliseconds;
            return process.ExitCode;
        }


        [Test]
        [Order(1)]
        [Timeout(OneMinute)]
        public void EnsureThat_BookGen_Creates_PrintableHTML()
        {
            int exitCode = RunBookGen("BuildPrint", out double runtime, out string output);
            if (exitCode != 0)
                Assert.Fail(output);
            else
                Assert.Pass();
        }
    }
}

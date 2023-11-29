//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NUnit.Framework;

using System.IO;

namespace BookGen.TestsSystem
{
    public class TestEnvironment
    {
        private readonly string _workDir;

        public void AssertFileExistsAndHasContents(params string[] file)
        {
            var filePath = Path.Combine(file);
            var f = new FileInfo(Path.Combine(_workDir, filePath));
            Assert.Multiple(() =>
            {
                Assert.That(f.Exists, Is.True);
                Assert.That(f.Length, Is.GreaterThan(0));
            });
        }

        public string ReadFileContents(string file)
        {
            return File.ReadAllText(Path.Combine(_workDir, file));
        }

        internal TestEnvironment(string workDir)
        {
            _workDir = workDir;
        }

        internal void DeleteDir(string directory)
        {
            string path = Path.Combine(_workDir, directory);
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        internal void DeleteFile(string file)
        {
            string path = Path.Combine(_workDir, file);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}

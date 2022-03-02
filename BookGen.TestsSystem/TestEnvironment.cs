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

        public void AssertFileExistsAndHasContents(string  file)
        {
            FileInfo f = new FileInfo(Path.Combine(_workDir, file));
            Assert.IsTrue(f.Exists);
            Assert.IsTrue(f.Length > 0);
        }

        public string ReadFileContents(string file)
        {
            return File.ReadAllText(Path.Combine(_workDir, file));
        }

        internal TestEnvironment(string workDir)
        {
            _workDir = workDir;
        }

        internal void DeleteFile(string file)
        {
            File.Delete(Path.Combine(_workDir, file));
        }
    }
}

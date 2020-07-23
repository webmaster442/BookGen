//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Tests.Environment;
using NUnit.Framework;
using System.IO;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_FsPathExtensions
    {
        private string _testDir;

        [OneTimeSetUp]
        public void Setup()
        {
            _testDir = Path.Combine(Path.GetTempPath(), "fspath");
            if (!Directory.Exists(_testDir))
                Directory.CreateDirectory(_testDir);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            if (Directory.Exists(_testDir))
                Directory.Delete(_testDir, true);
        }

        [Test]
        public void EnsureThat_FsUtils_CreateDir_CreatesDirectory()
        {
            var dir = new FsPath(_testDir, "testDirectory");

            Assert.IsFalse(dir.IsExisting);
            dir.CreateDir(TestEnvironment.GetMockedLog());
            Assert.IsTrue(dir.IsExisting);
        }

        [Test]
        public void EnsureThat_FsUtils_CreateDir_DoesntCreateEmpty()
        {
            var dir = new FsPath("");

            Assert.IsFalse(dir.IsExisting);
            dir.CreateDir(TestEnvironment.GetMockedLog());
            Assert.IsFalse(dir.IsExisting);
        }

        [Test]
        public void EnsureThat_FsUtils_WriteFile_CreatesFile()
        {
            var file = new FsPath(_testDir, "test.txt");

            file.WriteFile(TestEnvironment.GetMockedLog(), "test");

            Assert.IsTrue(file.IsExisting);

            var content = File.ReadAllText(Path.Combine(_testDir, "test.txt"));

            Assert.AreEqual("test", content);
        }

        [TestCase("")]
        [TestCase("c:\\")]
        public void EnsureThat_FsUtils_WriteFile_Fails_PathNotCorrect(string path)
        {
            var file = new FsPath(path);

            bool result = file.WriteFile(TestEnvironment.GetMockedLog(), "hello");

            Assert.IsFalse(result);
        }

        [Test]
        public void EnsureThat_FsUtils_CopyDirectory_Works()
        {
            var source = new FsPath(TestEnvironment.GetTestFolder());
            var target = new FsPath(_testDir, "copydir");

            var result = source.CopyDirectory(target, TestEnvironment.GetMockedLog());

            Assert.IsTrue(result);

            var files = Directory.GetFiles(Path.Combine(_testDir, "copydir"));

            Assert.AreEqual(6, files.Length);
        }

        [TestCase("", "")]
        [TestCase("c:\\", "")]
        [TestCase("", "c:\\")]
        public void EnsureThat_FsUtils_CopyDirectory_Fails_InvalidPath(string d1, string d2)
        {
            var source = new FsPath(d1);
            var target = new FsPath(d2);

            var result = source.CopyDirectory(target, TestEnvironment.GetMockedLog());

            Assert.IsFalse(result);
        }

        [Test]
        public void EnsureThat_FsUtils_Copy_Works()
        {
            var source = new FsPath(TestEnvironment.GetFile("TestFile.txt"));
            var target = new FsPath(_testDir, "copyfile");

            var result = source.Copy(target, TestEnvironment.GetMockedLog());

            Assert.IsTrue(result);
            Assert.IsTrue(target.IsExisting);
        }

        [TestCase("", "")]
        [TestCase("c:\\asd.txt", "")]
        [TestCase("", "c:\\asd.txt")]
        public void EnsureThat_FsUtils_Copy_Fails_InvalidPath(string d1, string d2)
        {
            var source = new FsPath(d1);
            var target = new FsPath(d2);

            var result = source.Copy(target, TestEnvironment.GetMockedLog());

            Assert.IsFalse(result);
        }

        [TestCase("TestFile.txt", "Test")]
        [TestCase("foo.txt", "")]
        [TestCase("", "")]
        public void EnsureThat_FsUtils_ReadFile_Works(string file, string expected)
        {
            var source = new FsPath(TestEnvironment.GetFile(file));

            string actual = source.ReadFile(TestEnvironment.GetMockedLog());

            Assert.AreEqual(expected, actual);
        }

        [TestCase("", "", "")]
        [TestCase("test", @"c:\", @"c:\test")]
        [TestCase(@"..\asd.png", @"c:\foo\bar", @"c:\foo\asd.png")]
        [TestCase(@"..\..\asd.png", @"c:\foo\bar", @"c:\asd.png")]
        public void EnsureThat_FsUtils_GetAbsolutePathRelativeTo_Works(string file, string relative, string expected)
        {
            var source = new FsPath(file);
            var relativeTo = new FsPath(relative);

            var result = source.GetAbsolutePathRelativeTo(relativeTo);

            Assert.AreEqual(expected, result.ToString());
        }

        [TestCase("", "", "")]
        [TestCase(@"c:\test", @"c:\", "test")]
        [TestCase(@"c:\foo\asd.png", @"c:\foo\bar", @"..\asd.png" )]
        [TestCase(@"c:\asd.png", @"c:\foo\bar", @"..\..\asd.png")]
        public void EnsureThat_FsUtils_GetRelativePathRelativeTo_Works(string absolute, string relative, string expected)
        {
            var source = new FsPath(absolute);
            var relativeTo = new FsPath(relative);

            var result = source.GetRelativePathRelativeTo(relativeTo);

            Assert.AreEqual(expected, result.ToString());
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

            Assert.That(dir.IsExisting, Is.False);
            dir.CreateDir(TestEnvironment.GetMockedLog());
            Assert.That(dir.IsExisting, Is.True);
        }

        [Test]
        public void EnsureThat_FsUtils_CreateDir_DoesntCreateEmpty()
        {
            var dir = new FsPath("");

            Assert.That(dir.IsExisting, Is.False);
            dir.CreateDir(TestEnvironment.GetMockedLog());
            Assert.That(dir.IsExisting, Is.False);
        }

        [Test]
        public void EnsureThat_FsUtils_WriteFile_CreatesFile()
        {
            var file = new FsPath(_testDir, "test.txt");

            file.WriteFile(TestEnvironment.GetMockedLog(), "test");

            Assert.That(file.IsExisting, Is.True);

            string content = File.ReadAllText(Path.Combine(_testDir, "test.txt"));

            Assert.That(content, Is.EqualTo("test"));
        }

        [TestCase("")]
        [TestCase("c:\\")]
        public void EnsureThat_FsUtils_WriteFile_Fails_PathNotCorrect(string path)
        {
            var file = new FsPath(path);

            bool result = file.WriteFile(TestEnvironment.GetMockedLog(), "hello");

            Assert.That(result, Is.False);
        }

        [Test]
        public void EnsureThat_FsUtils_CopyDirectory_Works()
        {
            var source = new FsPath(TestEnvironment.GetTestFolder());
            var target = new FsPath(_testDir, "copydir");

            int expectedCount = source.GetAllFiles(false).Count();

            bool result = source.CopyDirectory(target, TestEnvironment.GetMockedLog());

            Assert.That(result, Is.True);

            string[] files = Directory.GetFiles(Path.Combine(_testDir, "copydir"));

            Assert.That(files, Has.Length.EqualTo(expectedCount));
        }

        [TestCase("", "")]
        [TestCase("c:\\", "")]
        [TestCase("", "c:\\")]
        public void EnsureThat_FsUtils_CopyDirectory_Fails_InvalidPath(string d1, string d2)
        {
            var source = new FsPath(d1);
            var target = new FsPath(d2);

            bool result = source.CopyDirectory(target, TestEnvironment.GetMockedLog());

            Assert.That(result, Is.False);
        }

        [Test]
        public void EnsureThat_FsUtils_Copy_Works()
        {
            var source = new FsPath(TestEnvironment.GetFile("TestFile.txt"));
            var target = new FsPath(_testDir, "copyfile");

            bool result = source.Copy(target, TestEnvironment.GetMockedLog());
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(target.IsExisting, Is.True);
            });
        }

        [TestCase("", "")]
        [TestCase("c:\\asd.txt", "")]
        [TestCase("", "c:\\asd.txt")]
        public void EnsureThat_FsUtils_Copy_Fails_InvalidPath(string d1, string d2)
        {
            var source = new FsPath(d1);
            var target = new FsPath(d2);

            bool result = source.Copy(target, TestEnvironment.GetMockedLog());

            Assert.That(result, Is.False);
        }

        [TestCase("TestFile.txt", "Test")]
        [TestCase("foo.txt", "")]
        [TestCase("", "")]
        public void EnsureThat_FsUtils_ReadFile_Works(string file, string expected)
        {
            var source = new FsPath(TestEnvironment.GetFile(file));

            string actual = source.ReadFile(TestEnvironment.GetMockedLog());

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("", "", "")]
        [TestCase("test", @"c:\", @"c:\test")]
        [TestCase(@"..\asd.png", @"c:\foo\bar", @"c:\foo\asd.png")]
        [TestCase(@"..\..\asd.png", @"c:\foo\bar", @"c:\asd.png")]
        public void EnsureThat_FsUtils_GetAbsolutePathRelativeTo_Works(string file, string relative, string expected)
        {
            var source = new FsPath(file);
            var relativeTo = new FsPath(relative);

            FsPath result = source.GetAbsolutePathRelativeTo(relativeTo);

            Assert.That(result.ToString(), Is.EqualTo(expected));
        }

        [TestCase("", "", "")]
        [TestCase(@"c:\test", @"c:\", "test")]
        [TestCase(@"c:\foo\asd.png", @"c:\foo\bar", @"..\asd.png")]
        [TestCase(@"c:\asd.png", @"c:\foo\bar", @"..\..\asd.png")]
        public void EnsureThat_FsUtils_GetRelativePathRelativeTo_Works(string absolute, string relative, string expected)
        {
            var source = new FsPath(absolute);
            var relativeTo = new FsPath(relative);

            FsPath result = source.GetRelativePathRelativeTo(relativeTo);

            Assert.That(result.ToString(), Is.EqualTo(expected));
        }

        [TestCase(@"c:\foo.bar", ".txt", @"c:\foo.txt")]
        [TestCase(@"c:\foo.bar", "", @"c:\foo.")]
        [TestCase(@"c:\foo.bar", "txt", @"c:\foo.txt")]
        public void EnsureThat_FsUtils_ChangeExtension_Works(string input, string ext, string expected)
        {
            FsPath result = new FsPath(input).ChangeExtension(ext);
            Assert.That(result.ToString(), Is.EqualTo(expected));
        }

        [TestCase("", false)]
        [TestCase(@"c:\foo.bar", false)]
        [TestCase(@"c:\foo.*", true)]
        [TestCase(@"c:\*.bar", true)]
        [TestCase(@"c:\*.*", true)]
        public void EnsureThat_FsUtils_IsWildCard_Works(string input, bool expected)
        {
            bool result = new FsPath(input).IsWildCard();
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}

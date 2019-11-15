//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Tests.Environment;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_FsPath
    {
        [TestCase(true, "TestFile.txt")]
        [TestCase(false, "asd.txt")]
        public void EnsureThat_FsPath_IsExisting_ReturnsExpected(bool expected, string file)
        {
            var sut = new FsPath(TestEnvironment.GetFile(file));

            Assert.AreEqual(expected, sut.IsExisting);
        }

        [Test]
        public void EnsureThat_FsPath_IsExisting_Returns_False_ForEmpty()
        {
            var sut = new FsPath("");

            Assert.IsFalse(sut.IsExisting);
        }


        [TestCase(".txt", "test.txt")]
        [TestCase(".bar", "foo.bar")]
        [TestCase("", "")]
        public void EnsureThat_FsPath_Extension_ReturnsExpected(string expected, string file)
        {
            var sut = new FsPath(file);

            Assert.AreEqual(expected, sut.Extension);
        }

        [TestCase("test.txt", @"t:\test.txt")]
        [TestCase("foo.bar", @"t:\foo.bar")]
        [TestCase("", "")]
        public void EnsureThat_FsPath_Filename_ReturnsExpected(string expected, string file)
        {
            var sut = new FsPath(file);

            Assert.AreEqual(expected, sut.Filename);
        }

        [TestCase(true, "foo.bar", "foo.bar")]
        [TestCase(true, "", "")]
        [TestCase(false, "foo.asd", "foo.bar")]
        public void EnsureThat_FsPath_Equals_ReturnsExpected(bool expected, string i1, string i2)
        {
            var sut1 = new FsPath(i1);
            var sut2 = new FsPath(i2);

            Assert.IsFalse(object.ReferenceEquals(sut1, sut2));
            Assert.AreEqual(expected, sut1.Equals(sut2));
            Assert.AreEqual(expected, sut2.Equals(sut1));
        }


        [TestCase(true, "foo.bar", "foo.bar")]
        [TestCase(true, "", "")]
        [TestCase(false, "foo.asd", "foo.bar")]
        public void EnsureThat_FsPath_EqualsOperator_ReturnsExpected(bool expected, string i1, string i2)
        {
            var sut1 = new FsPath(i1);
            var sut2 = new FsPath(i2);

            Assert.IsFalse(object.ReferenceEquals(sut1, sut2));
            Assert.AreEqual(expected, sut1 == sut2);
            Assert.AreEqual(expected, sut2 == sut1);
        }
    }
}

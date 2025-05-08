//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

            Assert.That(sut.IsExisting, Is.EqualTo(expected));
        }

        [Test]
        public void EnsureThat_FsPath_IsExisting_Returns_False_ForEmpty()
        {
            var sut = new FsPath("");

            Assert.That(sut.IsExisting, Is.False);
        }

        [TestCase(".txt", "test.txt")]
        [TestCase(".bar", "foo.bar")]
        [TestCase("", "")]
        public void EnsureThat_FsPath_Extension_ReturnsExpected(string expected, string file)
        {
            var sut = new FsPath(file);

            Assert.That(sut.Extension, Is.EqualTo(expected));
        }

        [TestCase("test.txt", @"t:\test.txt")]
        [TestCase("foo.bar", @"t:\foo.bar")]
        [TestCase("", "")]
        public void EnsureThat_FsPath_Filename_ReturnsExpected(string expected, string file)
        {
            var sut = new FsPath(file);

            Assert.That(sut.Filename, Is.EqualTo(expected));
        }

        [TestCase(true, "foo.bar", "foo.bar")]
        [TestCase(true, "", "")]
        [TestCase(false, "foo.asd", "foo.bar")]
        public void EnsureThat_FsPath_Equals_ReturnsExpected(bool expected, string i1, string i2)
        {
            var sut1 = new FsPath(i1);
            var sut2 = new FsPath(i2);
            Assert.Multiple(() =>
            {
                Assert.That(object.ReferenceEquals(sut1, sut2), Is.False);
                Assert.That(sut1.Equals(sut2), Is.EqualTo(expected));
                Assert.That(sut2.Equals(sut1), Is.EqualTo(expected));
            });
        }

        [TestCase(true, "foo.bar", "foo.bar")]
        [TestCase(true, "", "")]
        [TestCase(false, "foo.asd", "foo.bar")]
        public void EnsureThat_FsPath_EqualsOperator_ReturnsExpected(bool expected, string i1, string i2)
        {
            var sut1 = new FsPath(i1);
            var sut2 = new FsPath(i2);
            Assert.Multiple(() =>
            {
                Assert.That(object.ReferenceEquals(sut1, sut2), Is.False);
                Assert.That(sut1 == sut2, Is.EqualTo(expected));
                Assert.That(sut2 == sut1, Is.EqualTo(expected));
            });
        }
    }
}

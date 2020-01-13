//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework.Shortcodes;
using BookGen.Tests.Environment;
using NUnit.Framework;
using System.Collections.Generic;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_SriDependency
    {
        private SriDependency _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new SriDependency(TestEnvironment.GetMockedLog(), TestEnvironment.GetMockedSettings());
        }

        [TearDown]
        public void TearDown()
        {
            _sut = null;
        }

        [Test]
        public void EnsureThat_SriDependency_Generate_WorksForJSFile()
        {
            var args = new Dictionary<string, string>
            {
                { "file", "Test.js" }
            };

            const string expected = "<script src=\"http://test.com/Test.js\" integrity=\"sha384-ZIiaaYu+MewKtrhJpP8K5vAKFUJ2wHaxNkltrjfIdh4opRm4o8xc9Tki1F9z2swu\" crossorigin=\"anonymous\"></script>";

            var result = _sut.Generate(args);

            Assert.AreEqual(expected, result);
        }


        [Test]
        public void EnsureThat_SriDependency_Generate_WorksForCSSFile()
        {
            var args = new Dictionary<string, string>
            {
                { "file", "Test.css" }
            };

            const string expected = "<link rel=\"stylesheet\" href=\"http://test.com/Test.css\" integrity=\"sha384-J8/g2z9Vs8+kXGVMf08+mwZ4yYQ9cRJOPruNGnoj6Tn6+L9cjqFwOHsCGk+yUpfa\" crossorigin=\"anonymous\"/>";

            var result = _sut.Generate(args);

            Assert.AreEqual(expected, result);
        }
    }
}

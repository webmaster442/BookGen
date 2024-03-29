﻿//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_JavaScriptInterop
    {
        private JavaScriptInterop _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new JavaScriptInterop();
        }

        [TearDown]
        public void Teardown()
        {
            _sut.Dispose();
            _sut = null;
        }

        [Test]
        public void TestSyntaxHighlight()
        {
            const string input = "#include <stdio.h>\r\n"
                + "int main() { printf(\"Hello World!\n\"); return 0; }";

            string result = _sut.PrismSyntaxHighlight(input, "c");
            Assert.That(result, Is.Not.Null);
        }
    }
}

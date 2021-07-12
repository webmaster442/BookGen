﻿//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using NUnit.Framework;

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

            var result = _sut.SyntaxHighlight(input, "c");
            Assert.IsNotNull(result);
        }
    }
}
﻿//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Utilities;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_MarkdownUtils
    {
        [TestCase("# C# Test", "C# Test")]
        [TestCase("## C# Test", "C# Test")]
        [TestCase("### C# Test", "C# Test")]
        public void EnsureThat_MarkdownUtils_GetTitleWorksCorrectly(string input, string expected)
        {
            var result = MarkdownUtils.GetTitle(input);
            Assert.AreEqual(expected, result);
        }

    }
}

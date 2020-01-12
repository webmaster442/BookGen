//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework.Scripts;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ProcessInterop
    {
        [TestCase("explorer.exe", true)]
        [TestCase("explorer", false)]
        public void EnsureThat_ProcessInterop_ResolveProgramFullPath_ReturnsCorrect(string program, bool expected)
        {
            var result = ProcessInterop.ResolveProgramFullPath(program);

            if (expected)
                Assert.IsNotNull(result);
            else
                Assert.IsNull(result);


        }

    }
}

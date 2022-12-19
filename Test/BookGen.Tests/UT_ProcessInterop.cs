//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework.Scripts;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ProcessInterop
    {
        [TestCase("explorer.exe", true)]
        [TestCase("explorer", false)]
        public void EnsureThat_ProcessInterop_ResolveProgramFullPath_ReturnsCorrect(string program, bool expected)
        {
            string result = ProcessInterop.ResolveProgramFullPath(program);

            if (expected)
                Assert.That(result, Is.Not.Null);
            else
                Assert.That(result, Is.Null);


        }

    }
}

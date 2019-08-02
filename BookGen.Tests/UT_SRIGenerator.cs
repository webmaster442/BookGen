//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//----------------------------------------------------------------------------

using BookGen.Core;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_SriGenerator
    {
        //Generate test case with:
        //echo -n "α" | openssl dgst -sha384 -binary | openssl base64 -A

        [TestCase("", "sha384-OLBgp1GsljhM2TJ+sbHjaiH9txEUvgdDTAzHv2P24donTt6/529l+9Ua0vFImLlb")]
        [TestCase("TestString", "sha384-wKWeztSCLwZXAexavFFTHJSIZK6EOR7GjoDBNdLz/lCSNEXptDbfoq/ap876g2e7")]
        [TestCase("α", "sha384-EBA5tvntdq0xYImnmBBvQJo3QePIG39H2dB0VqUWhS6DzcEjshHAascSJz3qzaZH")]
        public void EnsureThat_SriGenerator_GetSRI_ReturnsCorrectValue(string input, string expected)
        {
            var result = SriGenerator.GetSRI(input);
            Assert.AreEqual(expected, result);
        }

    }
}

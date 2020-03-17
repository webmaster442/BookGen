//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Shell;
using NUnit.Framework;
using System.Linq;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_Shell
    {
        [Test]
        public void EnsureThat_ShellProgram_DoComplete_WithoutArgs_Outputs_CmdList()
        {
            var results = ShellProgram.DoComplete(new string[] { });
            Assert.IsTrue(results.Any());
        }

        [TestCase("AssemblyDocument", "BookGen ass")]
        [TestCase("--assembly", "BookGen AssemblyDocument --ass")]
        public void EnsureThat_ShellProgram_DoComplete_Completes(string expected, params string[] input)
        {
            var results = ShellProgram.DoComplete(input);
            Assert.AreEqual(expected, results.FirstOrDefault());
        }

    }
}

//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Modules.Special;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_Shell
    {
        private ShellModule _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new ShellModule();
            _sut.Modules = Program.CreateModules();
        }

        [Test]
        public void EnsureThat_ShellProgram_DoComplete_WithoutArgs_Outputs_CmdList()
        {
            var results = _sut.DoComplete(new string[] { });
            Assert.IsTrue(results.Any());
        }

        [TestCase("BookGen", "bookGen")]
        [TestCase("BookGen", "Bookgen")]
        [TestCase("BookGen", "bookgen")]
        [TestCase("AssemblyDocument", "BookGen ass")]
        [TestCase("--assembly", "BookGen AssemblyDocument --ass")]
        [TestCase("AssemblyDocument", "bookgen ass")]
        [TestCase("--assembly", "bookGen AssemblyDocument --ass")]
        public void EnsureThat_ShellProgram_DoComplete_Completes(string expected, params string[] input)
        {
            var results = _sut.DoComplete(input);
            Assert.AreEqual(expected, results.FirstOrDefault());
        }

    }
}

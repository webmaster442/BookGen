//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
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
            _sut = new ShellModule
            {
                Modules = Program.CreateModules()
            };
        }

        [Test]
        public void EnsureThat_ShellProgram_DoComplete_WithoutArgs_Outputs_CmdList()
        {
            IEnumerable<string> results = _sut.DoComplete(new string[] { });
            Assert.That(results.Any(), Is.True);
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
            IEnumerable<string> results = _sut.DoComplete(input);
            Assert.That(results.FirstOrDefault(), Is.EqualTo(expected));
        }
    }
}

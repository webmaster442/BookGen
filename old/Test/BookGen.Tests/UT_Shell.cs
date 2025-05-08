//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Commands;
using BookGen.Infrastructure;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_Shell
    {
        private ShellCommand _sut;
        private IModuleApi _moduleApiMock;

        [SetUp]
        public void Setup()
        {
            _moduleApiMock = Substitute.For<IModuleApi>();
            _moduleApiMock.GetCommandNames().Returns(new[] { "bookgen", "assemblydocument" });
            _moduleApiMock.GetAutoCompleteItems("assemblydocument").Returns(new string[] { "--assembly" });
            _sut = new ShellCommand(_moduleApiMock);
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
        [TestCase("assemblydocument", "BookGen ass")]
        [TestCase("--assembly", "BookGen AssemblyDocument --ass")]
        [TestCase("assemblydocument", "bookgen ass")]
        [TestCase("--assembly", "bookGen AssemblyDocument --ass")]
        public void EnsureThat_ShellProgram_DoComplete_Completes(string expected, params string[] input)
        {
            IEnumerable<string> results = _sut.DoComplete(input);
            Assert.That(results.FirstOrDefault(), Is.EqualTo(expected));
        }
    }
}

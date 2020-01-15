//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Framework.Scripts;
using Moq;
using NUnit.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_Compiler
    {
        private const string source = @"namespace Test
{
    public class TestCode
    {
        public int Test(int a)
        {
            return 2 * a;
        }
    }
}";

        private Compiler _sut;
        private Mock<ILog> _logMock;

        [SetUp]
        public void Setup()
        {
            _logMock = new Mock<ILog>();
            _sut = new Compiler(_logMock.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _sut = null;
        }

        [Test]
        public void EnsureThat_Compiler_CompileToAssembly_ReturnsAssemblyValidSource()
        {
            var tree = _sut.ParseToSyntaxTree(source);
            var result = _sut.CompileToAssembly(tree);
            Assert.IsNotNull(result);
        }


        [Test]
        public void EnsureThat_Compiler_CompileToAssembly_ReturnsNullInValidSource()
        {
            const string invalidsource = source + "asdd";

            var tree = _sut.ParseToSyntaxTree(invalidsource);
            var result = _sut.CompileToAssembly(tree);
            Assert.IsNull(result);
        }
    }
}

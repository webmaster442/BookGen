//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Framework.Scripts;

namespace BookGen.Tests
{
    [TestFixture, SingleThreaded]
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

        [Test]
        public void EnsureThat_Compiler_CompileToAssembly_ReturnsAssemblyValidSource()
        {
            var _logMock = new Mock<ILog>();
            var _sut = new Compiler(_logMock.Object);

            var tree = _sut.ParseToSyntaxTree(source);
            var result = _sut.CompileToAssembly(tree);
            Assert.IsNotNull(result);
        }


        [Test]
        public void EnsureThat_Compiler_CompileToAssembly_ReturnsNullInValidSource()
        {
            var _logMock = new Mock<ILog>();
            var _sut = new Compiler(_logMock.Object);

            const string invalidsource = source + "asdd";

            var tree = _sut.ParseToSyntaxTree(invalidsource);
            var result = _sut.CompileToAssembly(tree);
            Assert.IsNull(result);
        }
    }
}

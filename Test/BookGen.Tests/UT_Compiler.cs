//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
            var _logMock = Substitute.For<ILog>();
            var _sut = new Compiler(_logMock);

            IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> tree = _sut.ParseToSyntaxTree(source);
            System.Reflection.Assembly result = _sut.CompileToAssembly(tree);
            Assert.IsNotNull(result);
        }


        [Test]
        public void EnsureThat_Compiler_CompileToAssembly_ReturnsNullInValidSource()
        {
            var _logMock = Substitute.For<ILog>();
            var _sut = new Compiler(_logMock);

            const string invalidsource = source + "asdd";

            IEnumerable<Microsoft.CodeAnalysis.SyntaxTree> tree = _sut.ParseToSyntaxTree(invalidsource);
            System.Reflection.Assembly result = _sut.CompileToAssembly(tree);
            Assert.IsNull(result);
        }
    }
}

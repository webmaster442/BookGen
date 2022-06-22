//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;

namespace BookGen.Tests
{
    [TestFixture]
    public class UT_ShortCodeArgumentTokenizer
    {
        public static IEnumerable<TestCaseData> NoSkipTests
        {
            get
            {
                yield return new TestCaseData("", new string[] { });
                yield return new TestCaseData(" abc ", new string[] { "abc" });
                yield return new TestCaseData("a b", new string[] { "a", "b" });
                yield return new TestCaseData("a b \"cdef\"", new string[] { "a", "b", "cdef" });
                yield return new TestCaseData("something \"qouted long\"", new string[] { "something", "qouted long" });
            }
        }


        [TestCaseSource(nameof(NoSkipTests))]
        public void EnsureThat_ShortCodeArgumentTokenizer_Split_Works(string input, string[] result)
        {
            var tokens = ShortCodeArgumentTokenizer.Split(input).ToArray();
            Assert.AreEqual(result, tokens);
        }

    }
}

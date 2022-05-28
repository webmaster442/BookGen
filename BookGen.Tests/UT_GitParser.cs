using BookGen.ShellHelper.Code;
using BookGen.ShellHelper.Domain;

namespace BookGen.Tests
{
    [TestFixture]
    internal class UT_GitParser
    {
        public const string TestInput = "# branch.oid 6c84b1f529990058bc913b113fb011400ed7d744\r\n" +
                                        "# branch.head next\r\n" +
                                        "# branch.upstream origin/next\r\n" +
                                        "# branch.ab +5 -8\r\n" +
                                        "? BookGen.Shell / ";

        [Test]
        public void TestParseStatus()
        {
            GitStatus result = GitParser.ParseStatus(TestInput);
            Assert.AreEqual("next", result.BranchName);
            Assert.AreEqual("6c84b1f529990058bc913b113fb011400ed7d744", result.LastCommitId);
            Assert.AreEqual(8, result.IncommingCommits);
            Assert.AreEqual(5, result.OutGoingCommits);
        }
    }
}

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

        public const string TestModified =
            "# branch.oid 1dd7c10acf69a077cc13e7045e700ac45451314e\r\n" +
            "# branch.head crypto\r\n" +
            "# branch.upstream origin/crypto\r\n" +
            "# branch.ab +0 -0\r\n" +
            "? \"crypto/K\\303\\263d/Tests/\"\r\n" +
            "? test.txt";

        [Test]
        public void TestParseStatus()
        {
            GitStatus result = GitParser.ParseStatus(TestInput);
            Assert.AreEqual("next", result.BranchName);
            Assert.AreEqual("6c84b1f529990058bc913b113fb011400ed7d744", result.LastCommitId);
            Assert.AreEqual(8, result.IncommingCommits);
            Assert.AreEqual(5, result.OutGoingCommits);
        }

        [Test]
        public void TestParseStatusModified()
        {
            GitStatus result = GitParser.ParseStatus(TestModified);
            Assert.AreEqual("crypto", result.BranchName);
            Assert.AreEqual("1dd7c10acf69a077cc13e7045e700ac45451314e", result.LastCommitId);
            Assert.AreEqual(0, result.IncommingCommits);
            Assert.AreEqual(0, result.OutGoingCommits);
            Assert.AreEqual(2, result.NotCommitedChanges);
        }
    }
}

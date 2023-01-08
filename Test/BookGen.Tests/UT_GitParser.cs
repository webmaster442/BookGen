//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Terminal;

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
            Assert.Multiple(() =>
            {
                Assert.That(result.BranchName, Is.EqualTo("next"));
                Assert.That(result.LastCommitId, Is.EqualTo("6c84b1f529990058bc913b113fb011400ed7d744"));
                Assert.That(result.IncommingCommits, Is.EqualTo(8));
                Assert.That(result.OutGoingCommits, Is.EqualTo(5));
            });
        }

        [Test]
        public void TestParseStatusModified()
        {
            GitStatus result = GitParser.ParseStatus(TestModified);
            Assert.Multiple(() =>
            {
                Assert.That(result.BranchName, Is.EqualTo("crypto"));
                Assert.That(result.LastCommitId, Is.EqualTo("1dd7c10acf69a077cc13e7045e700ac45451314e"));
                Assert.That(result.IncommingCommits, Is.EqualTo(0));
                Assert.That(result.OutGoingCommits, Is.EqualTo(0));
                Assert.That(result.NotCommitedChanges, Is.EqualTo(2));
            });
        }
    }
}

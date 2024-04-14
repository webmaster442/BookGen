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
        public const string TestInput = """
            # branch.oid 6c84b1f529990058bc913b113fb011400ed7d744
            # branch.head next
            # branch.upstream origin/next
            # branch.ab +5 -8
            ? BookGen.Shell
            """;

        public const string TestModified =
            """
            # branch.oid 1dd7c10acf69a077cc13e7045e700ac45451314e
            # branch.head crypto
            # branch.upstream origin/crypto
            # branch.ab +0 -0
            ? \"crypto/K\\303\\263d/Tests/
            ? test.txt
            """;

        public const string TestInitial =
            """
            # branch.oid (initial)
            # branch.head master
            ? test.txt
            """;

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

        [Test]
        public void TestParseStatusInitial()
        {
            GitStatus result = GitParser.ParseStatus(TestInitial);
            Assert.Multiple(() =>
            {
                Assert.That(result.BranchName, Is.EqualTo("master"));
                Assert.That(result.LastCommitId, Is.EqualTo("(initial)"));
                Assert.That(result.IncommingCommits, Is.EqualTo(0));
                Assert.That(result.OutGoingCommits, Is.EqualTo(0));
                Assert.That(result.NotCommitedChanges, Is.EqualTo(1));
            });
        }
    }
}

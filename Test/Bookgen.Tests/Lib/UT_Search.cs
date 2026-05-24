using Bookgen.Lib.Markdown;

namespace Bookgen.Tests.Lib;

internal class UT_Search
{
    [TestCase("helo", "hello", 1)]
    [TestCase("kelo", "hello", 2)]
    [TestCase("kel", "hello", 3)]
    [TestCase("kelm", "hello", 3)]
    [TestCase("setting", "sittmg", 3)]
    public void EnsureThat_LevenshteinDistance_ReturnsExpected(string t1, string t2, int expected)
    {
        int result = Search.LevenshteinDistance(t1, t2);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public static void EnsureThat_Contains_ReturnsExpected()
    {
        const string document = "This is a test document.\r\nIt contains multiple lines.\r\nSome lines are similar to the search term.";
        const string searchTerm = "test document";
        const float similarityThreshold = 0.8f;

        bool result = Search.Contains(document, searchTerm, similarityThreshold, out string? context);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.True);
            Assert.That(context, Is.EqualTo("This is a test document"));
        }
    }
}

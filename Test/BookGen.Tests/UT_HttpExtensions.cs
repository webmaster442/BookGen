using Webmaster442.HttpServerFramework.Internal;

namespace BookGen.Tests;

[TestFixture]
public class UT_HttpExtensions
{
    [Test]
    public void EnsureThat_Extensions_ToLastModifiedHeaderFormat_ReturnsExpected()
    {
        string result = new DateTime(2015, 10, 21, 7, 28, 0, DateTimeKind.Utc).ToLastModifiedHeaderFormat();

        Assert.That(result, Is.EqualTo("Wed, 21 Oct 2015 07:28:00 GMT"));
    }
}

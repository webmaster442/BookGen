namespace Bookgen.Experiments.Tests;

[TestFixture]
internal class DotenvParserTests
{
    //https://github.com/env-lang/env/blob/main/env.md

    [Test]
    public void EnsureThat_Comments_And_EmptyLines_AreIgnored()
    {
        string dotenv = """
            ; This is a semicolon comment
            // This is a double-slash comment

            # This is a hash comment

            """;

        using var reader = new StringReader(dotenv);

        var env = DotEnvParser.Parse(reader, StringComparer.Ordinal);

        Assert.That(env.Keys, Is.Empty, "Expected no keys to be parsed from comments.");
    }

    [TestCase("SECRET=\"password#123\"  # The '#' is part of the value", "SECRET", "password#123")]
    [TestCase("MESSAGE=\"Hello # World\" # The '#' after Hello is part of the value", "MESSAGE", "Hello # World")]
    [TestCase("URL=https://example.com/path?foo=bar&baz=qux", "URL", "https://example.com/path?foo=bar&baz=qux")]
    [TestCase("KEY=\"VAL//UE\" //comment", "KEY", "VAL//UE")]
    [TestCase("KEY=\"VAL;UE\" ; comment", "KEY", "VAL;UE")]
    [TestCase("FOO=value", "FOO", "value")]
    [TestCase("foo=value", "foo", "value")]
    [TestCase("FOO_BAR=value", "FOO_BAR", "value")]
    [TestCase("_FOO=value", "_FOO", "value")]
    [TestCase("FOO=bar", "FOO", "bar")]
    [TestCase("FOO= bar", "FOO", " bar")]
    [TestCase("FOO=bar baz", "FOO", "bar baz")]
    [TestCase("FOO=\"BAR\"", "FOO", "BAR")]
    [TestCase("FOO=\" bar \"", "FOO", " bar ")]
    [TestCase("EMPTY=", "EMPTY", "")]
    [TestCase("EMPTY=\"\"", "EMPTY", "")]
    [TestCase("UNQUOTED=value with spaces", "UNQUOTED", "value with spaces")]
    [TestCase("PATH=/usr/local/bin:/usr/bin:/bin", "PATH", "/usr/local/bin:/usr/bin:/bin")]
    [TestCase("MESSAGE='Hello World'", "MESSAGE", "Hello World")]
    [TestCase("PATH=\"C:\\Program Files\\App\"", "PATH", "C:\\Program Files\\App")]
    [TestCase("HASH=\"my#password\"", "HASH", "my#password")]
    public void EnsureThat_Keys_And_Values_ParsedCorrectly(string input, string expectedKey, string expectedValue)
    {
        using var reader = new StringReader(input);

        var env = DotEnvParser.Parse(reader, StringComparer.Ordinal);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(env.Keys, Contains.Key(expectedKey));
            Assert.That(env.GetValueOrDefault(expectedKey, string.Empty), Is.EqualTo(expectedValue));
        }
    }

    [TestCase("123FOO=value")]
    [TestCase("FOO-BAR=value")]
    [TestCase(".FOO=value")]
    [TestCase("FOO\\")]
    [TestCase("BAR=value ")]
    [TestCase("foo\nbar\nbaz=qux")]
    public void EnsureThat_InvalidKeys_ThrowException(string input)
    {
        using var reader = new StringReader(input);
        Assert.Throws<EnvironmentException>(() => DotEnvParser.Parse(reader, StringComparer.Ordinal));
    }

    [Test]
    public void EnsureThat_Quoted_MultiLine_Works()
    {
        string dotenv = """
            PRIVATE_KEY="-----BEGIN RSA PRIVATE KEY-----
            MIIBOgIBAAJBAOsfi5AGYhdRs/x6q5H7kScxA0Kzrw
            ...
            -----END RSA PRIVATE KEY-----"
            """;
        using var reader = new StringReader(dotenv);

        var env = DotEnvParser.Parse(reader, StringComparer.Ordinal);

        string expectedValue = """
            -----BEGIN RSA PRIVATE KEY-----
            MIIBOgIBAAJBAOsfi5AGYhdRs/x6q5H7kScxA0Kzrw
            ...
            -----END RSA PRIVATE KEY-----
            """;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(env.Keys, Contains.Key("PRIVATE_KEY"));
            Assert.That(env.GetValueOrDefault("PRIVATE_KEY", string.Empty), Is.EqualTo(expectedValue).IgnoreLineEndingFormat);
        }
    }

    [Test]
    public void EnsureThat_BackSlashes_Work()
    {
        string dotenv = """
            LONG_MESSAGE=first line \
            second line \
            third line
            """;
        using var reader = new StringReader(dotenv);

        var env = DotEnvParser.Parse(reader, StringComparer.Ordinal);

        string expectedValue = """
            first line 
            second line 
            third line
            """;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(env.Keys, Contains.Key("LONG_MESSAGE"));
            Assert.That(env.GetValueOrDefault("LONG_MESSAGE", string.Empty), Is.EqualTo(expectedValue).IgnoreLineEndingFormat);
        }
    }

    [TestCase("foo\nbar\nbaz=qux", "ENV001: Invalid Line Format")]
    [TestCase("foo=bar\r\nfoo=bar", "ENV002: Duplicate Key")]
    [TestCase("123FOO=value", "ENV003: Invalid Key Format")]
    [TestCase("FOO-BAR=value", "ENV003: Invalid Key Format")]
    [TestCase(".FOO=value", "ENV003: Invalid Key Format")]
    [TestCase("Foo=\"bar", "ENV004: Unclosed Quote")]
    [TestCase("SECRET=password\\ # comment\r\nNEXT=value", "ENV005: Invalid Line Continuation")]
    [TestCase("MULTI\\\r\nLINE_KEY=value", "ENV006: Multi-line Key")]
    public void EnsureThat_Error_Handling_Works_AccordingToSpec(string input, string expectedMessage)
    {
        using var reader = new StringReader(input);

        EnvironmentException? ex = Assert.Throws<EnvironmentException>(() => DotEnvParser.Parse(reader, StringComparer.Ordinal));

        Assert.That(ex.Message, Does.StartWith(expectedMessage));
    }
}

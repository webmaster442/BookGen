//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Internals;

namespace Bookgen.Tests.Lib;

[TestFixture]
public class UT_StringExtensions
{
    [TestCase("Hello World!", "hello-worldexclamation")]
    [TestCase("C# & .NET", "csharp-and-dotnet")]
    [TestCase("100%", "100percent")]
    [TestCase("foo/bar", "fooslashbar")]
    [TestCase("A+B", "aplusb")]
    [TestCase("question?", "questionquestion")]
    [TestCase("ümlaut", "umlaut", ExcludePlatform = "Linux")]
    [TestCase("", "n-a")]
    [TestCase("   ", "---")]
    [TestCase("a|b", "apipeb")]
    public void EnsureThat_ToUrlNiceName_ReturnExpected(string input, string expected)
    {
        var result = input.ToUrlNiceName();
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("<br>", "<br/>")]
    [TestCase("<img src='foo.png'>", "<img src='foo.png'/>")]
    [TestCase("<input type='text'>", "<input type='text'/>")]
    [TestCase("<div><br></div>", "<div><br/></div>")]
    [TestCase("<hr>", "<hr/>")]
    [TestCase("<meta charset='utf-8'>", "<meta charset='utf-8'/>")]
    [TestCase("<wbr>", "<wbr/>")]
    [TestCase("<custom>", "<custom>")]
    public void EnsureThat_MakeSelfClosingTagsXmlCompatible_ReturnsExpected(string html, string expected)
    {
        var result = html.MakeSelfClosingTagsXmlCompatible();
        Assert.That(expected, Is.EqualTo(result));
    }
}

using Bookgen.Lib.Templates;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using Moq;

namespace Bookgen.Tests.Lib;

[TestFixture]
internal class UT_TemplateEngine
{
    private TemplateEngine _sut;
    private TestLogger _logger;
    private Mock<IAssetSource> _assetSourceMock;

    [SetUp]
    public void Setup()
    {
        _assetSourceMock = new Mock<IAssetSource>(MockBehavior.Strict);
        _logger = new TestLogger();
        _sut = new TemplateEngine(_logger, _assetSourceMock.Object);
        _sut.RegisterFunction("MyFunction", (arg) => string.Join(',', arg));
    }

    [TestCase("", "")]
    [TestCase("<p>No template tags</p>", "<p>No template tags</p>\r\n")]
    [TestCase("<h1>{{Title}}</h1><p>{{Content}}</p>", "<h1>title</h1><p>This is content</p>\r\n")]
    [TestCase("<h1>{{Title}}</h1><p>{{MyFunction(\"Foo\")}}</p>", "<h1>title</h1><p>Foo</p>\r\n")]
    [TestCase("{{MyFunction(\"Foo\")}}", "Foo\r\n")]
    [TestCase("{{MyFunction(\"Foo\", \"Bar\")}}", "Foo,Bar\r\n")]
    [TestCase("{{MyFunction(\"Foo\", \"Bar\",\"Baz\")}}", "Foo,Bar,Baz\r\n")]
    [TestCase("{{MyFunction(\"Foo\", \"Bar\",\"Baz\", \"Banana\")}}", "Foo,Bar,Baz,Banana\r\n")]
    public void EnsureThat_Render_Works(string template, string expected)
    {
        var viewData = new ViewData { Content = "This is content", Title = "title", Host = string.Empty };

        string result = _sut.Render(template, viewData);

        IEqualityComparer<string?> comparer = new LineEndingIgnoreConverter();
        Assert.That(result, Is.EqualTo(expected).Using(comparer));
    }

    [Test]
    public void EnsureThat_Render_Warns_Unrecognized_Template_Parts()
    {
        var viewData = new ViewData { Content = "This is content", Title = "title", Host = string.Empty };
        string template = "<h1>{{Title}}</h1><p>{{UnrecognizedPart}}</p>";

        _sut.Render(template, viewData);

        Assert.That(_logger.Warnings, Is.EqualTo(1));
    }

    [Test]
    public void EnsureThat_Render_Warns_Unrecognized_Functions()
    {
        var viewData = new ViewData { Content = "This is content", Title = "title", Host = string.Empty };
        string template = "<h1>{{Title}}</h1><p>{{UnrecognizedFunction()}}</p>";
        _sut.Render(template, viewData);
        Assert.That(_logger.Warnings, Is.EqualTo(1));
    }
}

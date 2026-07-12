//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib;
using BookGen.Lib.Rendering.Templates;

using BookGen.Vfs;

namespace Bookgen.Tests.Lib;

[TestFixture]
internal class UT_TemplateEngine
{
    private TemplateEngine _sut;
    private TestLogger _logger;
    private TestEnvironment _environment;

    public class TestTimeProvider : TimeProvider
    {
        public override DateTimeOffset GetUtcNow()
        {
            return new DateTimeOffset(2026, 1, 1, 11, 12, 13, TimeSpan.Zero);
        }

        public override TimeZoneInfo LocalTimeZone
            => TimeZoneInfo.Utc;
    }


    [SetUp]
    public void Setup()
    {
        _environment = new TestEnvironment();
        _logger = new TestLogger();
        _sut = new TemplateEngine(_logger, _environment, new TemplateEngineOptions
        {
            TimeProvider = new TestTimeProvider()
        });
    }

    [TearDown]
    public void TearDown()
    {
        _environment.Dispose();
    }

    [TestCase("", "")]
    [TestCase("<p>No template tags</p>", "<p>No template tags</p>")]
    [TestCase("<h1>{{Title}}</h1><p>{{Content}}</p>", "<h1>title</h1><p>This is content</p>")]
    [TestCase("{{LastModified}}", "1987-10-11 00:00:00")]
    [TestCase("{{ToUpper(Title)}}", "TITLE")]
    [TestCase("{{ToLower(Title)}}", "title")]
    [TestCase("{{Substring(Content, 0, 4)}}", "This")]
    [TestCase("{{Trim('  Hello  ')}}", "Hello")]
    [TestCase("{{TrimStart('  Hello  ')}}", "Hello  ")]
    [TestCase("{{TrimEnd('  Hello  ')}}", "  Hello")]
    [TestCase("{{Replace(Content, 'content', 'Universe')}}", "This is Universe")]
    [TestCase("{{Concat(Content, '-','Bar', '-', '42')}}", "This is content-Bar-42")]
    [TestCase("{{Concat(Content, ' ', 42)}}", "This is content 42")]
    [TestCase("{{HtmlEncode('<div>')}}", "&lt;div&gt;")]
    [TestCase("{{UrlEncode('https://example.com')}}", "https%3A%2F%2Fexample.com")]
    [TestCase("{{UrlDecode('https%3A%2F%2Fexample.com')}}", "https://example.com")]
    [TestCase("{{CurrentDate()}}", "2026-01-01")]
    [TestCase("{{CurrentTime()}}", "11:12:13")]
    [TestCase("{{CurrentDateTime()}}", "2026-01-01 11:12:13")]
    [TestCase("{{RegexReplace(Content, 's', 'z')}}", "Thiz iz content")]
    public void EnsureThat_Render_Works(string template, string expected)
    {
        var viewData = new ViewData
        {
            Content = "This is content",
            Title = "title",
            Host = string.Empty,
            LastModified = new DateTime(1987, 10, 11)
        };

        string result = _sut.Render(template, viewData);

        IEqualityComparer<string?> comparer = new LineEndingIgnoreComparer();
        Assert.That(result, Is.EqualTo(expected).Using(comparer));
    }

    [Test]
    public void EnsureThat_Render_Errors_Unrecognized_Template_Parts()
    {
        var viewData = new ViewData
        {
            Content = "This is content",
            Title = "title",
            Host = string.Empty,
            LastModified = new DateTime(1987, 10, 11)
        };

        string template = "<h1>{{Title}}</h1><p>{{UnrecognizedPart}}</p>";

        _sut.Render(template, viewData);

        Assert.That(_logger.Errors, Is.EqualTo(1));
    }

    [Test]
    public void EnsureThat_Render_Errors_Unrecognized_Functions()
    {
        var viewData = new ViewData
        {
            Content = "This is content",
            Title = "title",
            Host = string.Empty,
            LastModified = new DateTime(1987, 10, 11)
        };

        string template = "<h1>{{Title}}</h1><p>{{UnrecognizedFunction()}}</p>";
        _sut.Render(template, viewData);

        Assert.That(_logger.Errors, Is.EqualTo(1));
    }

    [TestCase(BundledAssets.TemplateBlank)]
    [TestCase(BundledAssets.TemplatePrint)]
    [TestCase(BundledAssets.TemplateSinglePage)]
    [TestCase(BundledAssets.TemplateStaticWeb)]
    public void EnsureThat_BundledTemplates_Correct(string template)
    {
        IAssetSource assetSource = _environment;
        string TemplateContent = assetSource.GetAsset(template);

        var viewData = new ViewData
        {
            Content = "",
            Title = "",
            Host = string.Empty,
            LastModified = new DateTime(1987, 10, 11)
        };

        string result = _sut.Render(template, viewData);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Does.Not.Contain("{{Content}}"));
            Assert.That(result, Does.Not.Contain("{{Title}}"));
            Assert.That(_logger.Errors, Is.Zero);
        }
    }
}

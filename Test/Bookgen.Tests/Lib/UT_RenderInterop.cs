using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Markdown.RenderInterop;

namespace Bookgen.Tests.Lib;

[TestFixture]
internal class UT_RenderInterop
{
    private RenderInterop _sut;
    private ImageConfig _config;
    private TestEnvironment _environment;

    [SetUp]
    public void Setup()
    {
        _environment = new TestEnvironment();
        _config = new ImageConfig
        {
            SvgRecode = SvgRecodeOption.Passtrough,
        };
        _sut = new RenderInterop(_environment);
    }

    [TearDown]
    public void TearDown()
    {
        _sut.Dispose();
        _environment.Dispose();
    }

    [Test]
    public void EnsureThat_RenderLatex_ReturnsCorrectSvg()
    {
        ImageResult svg = _sut.RenderLatex("\\frac{1}{2} + \\sqrt{x}", _config);
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(svg.ImageType, Is.EqualTo(ImageType.Svg));
            Assert.That(svg.Data, Does.StartWith("<svg"));
            Assert.That(svg.Data, Does.EndWith("</svg>\n"));
        }
    }

    [Test]
    public void EnsureThat_Render_Nomnoml_ReturnsCorrectSvg()
    {
        ImageResult svg = _sut.RenderNomnoml("[<frame>Test]", _config);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(svg.ImageType, Is.EqualTo(ImageType.Svg));
            Assert.That(svg.Data, Does.StartWith("<svg"));
            Assert.That(svg.Data, Does.EndWith("</svg>"));
        }
    }

    [Test]
    public void EnsureThat_RenderQrCode_ReturnsCorrectSvg()
    {
        ImageResult svg = _sut.RenderQrCode("https://example.com", _config);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(svg.ImageType, Is.EqualTo(ImageType.Svg));
            Assert.That(svg.Data, Does.EndWith("</svg>"));
        }
    }

    [Test]
    public void EnsureThat_PrisimSyntaxHighlight_ReturnsCorrectHtml()
    {
        string html = _sut.PrismSyntaxHighlight("Console.WriteLine(\"Hello, World!\");", "csharp");
        string expected = "Console<span class=\"token punctuation\">.</span><span class=\"token function\">WriteLine</span><span class=\"token punctuation\">(</span><span class=\"token string\">\"Hello, World!\"</span><span class=\"token punctuation\">)</span><span class=\"token punctuation\">;</span>";
        Assert.That(html, Is.EqualTo(expected));
    }
}

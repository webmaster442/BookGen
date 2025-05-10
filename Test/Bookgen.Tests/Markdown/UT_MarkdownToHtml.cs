using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Markdown;

using Moq;

namespace Bookgen.Tests.Markdown;
internal class UT_MarkdownToHtml
{
    private Mock<IImgService> _imgServiceMock;
    private string _markdown;
    private string _soruceCode;

    [SetUp]
    public void Setup()
    {
        _imgServiceMock = new Mock<IImgService>(MockBehavior.Strict);
        _imgServiceMock.Setup(x => x.GetImageEmbedData("img.svg")).Returns(("<svg></svg>", ImageType.Svg));
        _imgServiceMock.Setup(x => x.GetImageEmbedData("img.png")).Returns(("base64", ImageType.Png));

        _soruceCode = """
            ```csharp
            public void Foo(Action<string> callback)
                => callback.Invoke("const");
            ```
            """;

        _markdown = """
            ---
            fontMatter: yes
            ---

            # First Headding

            ## Second heading

            This is a pragraph

            This is an autolink https://youtube.be

            This is a [link](https://youtube.be)

            ^^^
            ![this is an svg](img.svg)
            ^^^ Svg test

            ^^^
            ![this is png](img.png)
            ^^^ Png test
            """;
    }

    [Test]
    public void EnsureThat_Css_ClassesAreAplied()
    {
        var settings = new RenderSettings
        {
            CssClasses = new CssClasses
            {
                H1 = "h1 first",
                FigureCaption = "caption",
                Figure = "fig",
            },
            DeleteFirstH1 = false,
            HostUrl = null,
            PrismJsInterop = null,
        };

        using var sut = new MarkdownToHtml(_imgServiceMock.Object, settings);

        string expected = """
            <h1 id="first-headding" class="h1 first">First Headding</h1>
            <h2 id="second-heading">Second heading</h2>
            <p>This is a pragraph</p>
            <p>This is an autolink <a href="https://youtube.be">https://youtube.be</a></p>
            <p>This is a <a href="https://youtube.be">link</a></p>
            <figure class="fig">
            <p><svg></svg></p>
            <figcaption class="caption">Svg test</figcaption>
            </figure>
            <figure class="fig">
            <p><img src="data:image/png;base64,%20base64" alt="this is png" /></p>
            <figcaption class="caption">Png test</figcaption>
            </figure>

            """;

        string result = sut.RenderMarkdownToHtml(_markdown);

        Assert.That(result, Is.EqualTo(expected.Replace("\r\n", "\n")));
    }

    [Test]
    public void EnsureThat_DeleteFirstH1_HostUrlTargeting_Works()
    {
        var settings = new RenderSettings
        {
            CssClasses = new CssClasses(),
            DeleteFirstH1 = true,
            HostUrl = "https://my.domain",
            PrismJsInterop = null,
        };

        using var sut = new MarkdownToHtml(_imgServiceMock.Object, settings);

        string expected = """
            <h2 id="second-heading">Second heading</h2>
            <p>This is a pragraph</p>
            <p>This is an autolink <a href="https://youtube.be" target="_blank">https://youtube.be</a></p>
            <p>This is a <a href="https://youtube.be" target="_blank">link</a></p>
            <figure>
            <p><svg></svg></p>
            <figcaption>Svg test</figcaption>
            </figure>
            <figure>
            <p><img src="data:image/png;base64,%20base64" alt="this is png" /></p>
            <figcaption>Png test</figcaption>
            </figure>

            """;

        string result = sut.RenderMarkdownToHtml(_markdown);

        Assert.That(result, Is.EqualTo(expected.Replace("\r\n", "\n")));
    }

    [Test]
    public void EnsureThat_SourceCode_Works()
    {
        var settings = new RenderSettings
        {
            CssClasses = new CssClasses(),
            DeleteFirstH1 = false,
            HostUrl = "https://my.domain",
            PrismJsInterop = null,
        };

        using var sut = new MarkdownToHtml(_imgServiceMock.Object, settings);

        string expected = """
            <pre><code class="language-csharp">public void Foo(Action&lt;string&gt; callback)
                =&gt; callback.Invoke(&quot;const&quot;);
            </code></pre>
            
            """;

        string result = sut.RenderMarkdownToHtml(_soruceCode);

        Assert.That(result, Is.EqualTo(expected.Replace("\r\n", "\n")));
    }


    [Test]
    public void EnsureThat_SourceCode_PreRender_Works()
    {
        using var settings = new RenderSettings
        {
            CssClasses = new CssClasses(),
            DeleteFirstH1 = false,
            HostUrl = "https://my.domain",
            PrismJsInterop = new Lib.JsInterop.PrismJsInterop(new TestEnvironment()),
        };

        using var sut = new MarkdownToHtml(_imgServiceMock.Object, settings);

        string expected = """
            <pre><code class="language-csharp"><span class="token keyword">public</span> <span class="token return-type class-name"><span class="token keyword">void</span></span> <span class="token function">Foo</span><span class="token punctuation">(</span><span class="token class-name">Action<span class="token punctuation">&lt;</span><span class="token keyword">string</span><span class="token punctuation">></span></span> callback<span class="token punctuation">)</span>
                <span class="token operator">=></span> callback<span class="token punctuation">.</span><span class="token function">Invoke</span><span class="token punctuation">(</span><span class="token string">"const"</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
            </code></pre>
            
            """;

        string result = sut.RenderMarkdownToHtml(_soruceCode);

        Assert.That(result, Is.EqualTo(expected.Replace("\r\n", "\n")));
    }

}

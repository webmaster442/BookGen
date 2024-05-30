using BookGen.RenderEngine;

namespace BookGen.Tests;

[TestFixture]
public class UT_TemplateRenderer
{
    private TemplateRenderer _sut;
    private ILog _log;
    private TemplateParameters _templateParameters;
    private TimeProvider _timeProvider;
    private DateTimeOffset _expected;
    private IAppSetting _appSetting;

    [SetUp]
    public void Setup()
    {
        _log = Substitute.For<ILog>();
        _appSetting = Substitute.For<IAppSetting>();
        _expected = new DateTimeOffset(new DateTime(1, 1, 1, 11, 11, 11));
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.LocalTimeZone.Returns(TimeZoneInfo.Utc);
        _timeProvider.GetUtcNow().Returns(_expected);

        _sut = new TemplateRenderer(_log, _timeProvider, _appSetting);
        
        _templateParameters = new TemplateParameters
        {
            Content = "content",
            Host = "host",
            Metadata = "metadata",
            PrecompiledHeader = "pch",
            Title = "title",
            Toc = "toc",
        };
    }

    [TestCase("{{title}}", "title\r\n")]
    [TestCase("{{content}}", "content\r\n")]
    [TestCase("{{host}}", "host\r\n")]
    [TestCase("{{metadata}}", "metadata\r\n")]
    [TestCase("{{precompiledheader}}", "pch\r\n")]
    [TestCase("{{toc}}", "toc\r\n")]
    [TestCase("{{TITLE}}", "title\r\n")]
    [TestCase("{{coNtent}}", "content\r\n")]
    [TestCase("", "")]
    [TestCase("{{title}} - {{content}}", "title - content\r\n")]
    public void EnsureThat_TemplateRendererWorksForSingleLineExpected(string input, string expected)
    {
        var rendered = _sut.Render(input, _templateParameters);
        Assert.That(rendered, Is.EqualTo(expected));
    }


    [TestCase("{{BuildTime()}}", "01-01-01 11:11:11\r\n")]
    public void EnsureThat_TemplateRenderer_WorksForFunctionExpected(string input, string expected)
    {
        var rendered = _sut.Render(input, _templateParameters);
        Assert.That(rendered, Is.EqualTo(expected));
    }
}

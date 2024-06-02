using BookGen.Domain;
using BookGen.RenderEngine;

namespace BookGen.Tests;

[TestFixture]
public class UT_TemplateRenderer
{
    private TemplateRenderer _sut;
    private TemplateParameters _templateParameters;
    private TimeProvider _timeProvider;
    private DateTimeOffset _expected;

    [SetUp]
    public void Setup()
    {
        _expected = new DateTimeOffset(new DateTime(1, 1, 1, 11, 11, 11));
        _timeProvider = Substitute.For<TimeProvider>();
        _timeProvider.LocalTimeZone.Returns(TimeZoneInfo.Utc);
        _timeProvider.GetUtcNow().Returns(_expected);

        System.Environment.CurrentDirectory = TestEnvironment.GetTestFolder();

        _sut = new TemplateRenderer(new FunctionServices
        {
            Log = Substitute.For<ILog>(),
            TimeProvider = _timeProvider,
            AppSetting = Substitute.For<IAppSetting>(),
            RuntimeSettings = TestEnvironment.GetMockedRuntimeSettings(),
        });
        
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


    [TestCase("{{UnknownFunction()}}", "{{UnknownFunction()}}\r\n")]
    [TestCase("{{BuildTime()}}", "01-01-01 11:11:11\r\n")]
    [TestCase("{{SriDependency(file=Test.js)}}", "<script src=\"http://test.com/Test.js\" integrity=\"sha384-ZIiaaYu+MewKtrhJpP8K5vAKFUJ2wHaxNkltrjfIdh4opRm4o8xc9Tki1F9z2swu\" crossorigin=\"anonymous\"></script>\r\n")]
    [TestCase("{{SriDependency(file=Test.css)}}", "<link rel=\"stylesheet\" href=\"http://test.com/Test.css\" integrity=\"sha384-J8/g2z9Vs8+kXGVMf08+mwZ4yYQ9cRJOPruNGnoj6Tn6+L9cjqFwOHsCGk+yUpfa\" crossorigin=\"anonymous\"/>\r\n")]
    [TestCase("{{InlineFile(file=TestFile.txt)}}", "Test\r\n")]
    public void EnsureThat_TemplateRenderer_WorksForFunctionExpected(string input, string expected)
    {
        var rendered = _sut.Render(input, _templateParameters);
        Assert.That(rendered, Is.EqualTo(expected));
    }
}

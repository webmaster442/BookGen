namespace Bookgen.Experiments.Tests;

public class TemplateEngineTests
{
    private TemplateEngine<TestModel> _sut;
    private TestModel _model;

    [SetUp]
    public void Setup()
    {
        _sut = new TemplateEngine<TestModel>(emitNullString: true);
        _sut.RegisterBuiltinFunctions();
        _model = new TestModel
        {
            Text = "Hello, World!",
            Integer = 42,
            Double = 3.14,
            Boolean = true
        };
    }


    [TestCase(nameof(TestModel.Text), "Hello, World!")]
    [TestCase(nameof(TestModel.Integer), "42")]
    [TestCase(nameof(TestModel.Double), "3.14")]
    [TestCase(nameof(TestModel.Boolean), "True")]
    public void EnsureThat_RenderWorks_ForProperties(string property, string expected)
    {
        string result = _sut.Render("{{" + property + "}}", _model);
        Assert.That(result, Is.EqualTo(expected));
    }

    [TestCase("ToUpper(Text)", "HELLO, WORLD!")]
    [TestCase("ToLower(Text)", "hello, world!")]
    [TestCase("Substring(Text, 7, 5)", "World")]
    [TestCase("Trim('  Hello  ')", "Hello")]
    [TestCase("TrimStart('  Hello  ')", "Hello  ")]
    [TestCase("TrimEnd('  Hello  ')", "  Hello")]
    [TestCase("Replace(Text, 'World', 'Universe')", "Hello, Universe!")]
    [TestCase("Concat(Text, '-','Bar', '-', '42')", "Hello, World!-Bar-42")]
    [TestCase("Concat(Text, ' ', Integer)", "Hello, World! 42")]
    [TestCase("RegexReplace(Text, 'World', 'Universe')", "Hello, Universe!")]
    [TestCase("HtmlEncode('<div>')", "&lt;div&gt;")]
    [TestCase("UrlEncode('https://example.com')", "https%3A%2F%2Fexample.com")]
    [TestCase("UrlDecode('https%3A%2F%2Fexample.com')", "https://example.com")]
    public void EnsureThat_BuiltinFunctions_Work(string functionCall, string expected)
    {
        string result = _sut.Render("{{" + functionCall + "}}", _model);
        Assert.That(result, Is.EqualTo(expected));
    }
}

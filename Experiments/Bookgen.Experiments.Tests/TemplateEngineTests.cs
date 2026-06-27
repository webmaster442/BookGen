namespace Bookgen.Experiments.Tests;

public class TemplateEngineTests
{
    private TemplateEngine<TestModel> _sut;
    private TestModel _model;

    [SetUp]
    public void Setup()
    {
        _sut = new TemplateEngine<TestModel>();
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



}

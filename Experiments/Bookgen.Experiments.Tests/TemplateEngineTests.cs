using System.Globalization;

namespace Bookgen.Experiments.Tests;

public class TemplateEngineTests
{
    private TemplateEngine<TestModel> _sut;
    private TestModel _model;

    [SetUp]
    public void Setup()
    {
        _sut = new TemplateEngine<TestModel>(emitNullString: true);
        _sut.RegisterFunction("ToUpper", TestFunctions.ToUpper);
        _model = new TestModel
        {
            Text = "Hello, World!",
            Integer = 42,
            Double = 3.14,
            Boolean = true
        };
    }

    internal static class  TestFunctions
    {
        public static string ToUpper(object obj)
        {
            if (obj is IFormattable formattable)
                return formattable.ToString(null, CultureInfo.InvariantCulture).ToUpper();

            return obj?.ToString()?.ToUpper()
                ?? string.Empty;
        }
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

    [TestCase("\"constant\"", "CONSTANT")]
    [TestCase(nameof(TestModel.Text), "HELLO, WORLD!")]
    [TestCase(nameof(TestModel.Boolean), "TRUE")]
    [TestCase(nameof(TestModel.Integer), "42")]
    [TestCase(nameof(TestModel.Double), "3.14")]
    public void EnsureThat_Render_Works_ForParameterizedFunction(string functionArg, string expected)
    {
        string result = _sut.Render("{{ToUpper(" + functionArg + ")}}", _model);
        Assert.That(result, Is.EqualTo(expected));
    }

}

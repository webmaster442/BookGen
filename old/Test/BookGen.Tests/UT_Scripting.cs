using BookGen.DomainServices.Markdown.Scripting;

using Markdig;

namespace BookGen.Tests;

public class UT_Scripting
{
    private MarkdownPipeline _pipeline;

    [SetUp]
    public void Setup()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseScripting()
            .UseGenericAttributes()
            .Build();
    }

    [Test]
    public void TestScripting()
    {
        var script = """
            '''script
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"<p>Hello World {i}<p>");
            }
            '''
            """;

        var expected = """
            <p>Hello World 0<p>
            <p>Hello World 1<p>
            <p>Hello World 2<p>
            <p>Hello World 3<p>
            <p>Hello World 4<p>
            <p>Hello World 5<p>
            <p>Hello World 6<p>
            <p>Hello World 7<p>
            <p>Hello World 8<p>
            <p>Hello World 9<p>

            """;

        string result = Markdown.ToHtml(script, _pipeline);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void TestConsoleReadThrows()
    {
       var script = """
            '''script
            Console.ReadLine();
            '''
            """;

        string result = Markdown.ToHtml(script, _pipeline);
        Assert.That(result, Is.EqualTo("Console Inputs is not supported in scripting\r\n"));
    }
}
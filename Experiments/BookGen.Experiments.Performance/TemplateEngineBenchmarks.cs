using System.Text;

using BenchmarkDotNet.Attributes;

using Bookgen.Experiments;

namespace BookGen.Experiments.Performance;

[SimpleJob]
[HtmlExporter]
public class TemplateEngineBenchmarks
{
    [Params(1024, 2048, 4096, 8192)]
    public int TemplateSize { get; set; }

    [Params(10, 100, 1000)]
    public int FileCount { get; set; }

    public string Template { get; set; } = "";

    [GlobalSetup]
    public void Setup()
    {
        Template = new string('-', TemplateSize) + "{{Field}}";
    }

    class SimpleModel
    {
        public string Field { get; set; } = "Value";
    }

    [Benchmark]
    public void BenchmarkSimpleReplacement()
    {
        SimpleModel model = new SimpleModel();
        StringBuilder buffer = new();
        using var writer = new StringWriter(buffer);
        TemplateEngine<SimpleModel> engine = new TemplateEngine<SimpleModel>(true);
        for (int i = 0; i < FileCount; i++)
        {
            engine.Render(Template, writer, model);
            buffer.Clear();
        }
    }
}

using System.Text;

namespace Bookgen.Lib.JsInterop;

public sealed class JavascriptBuilder
{
    private readonly StringBuilder _builder;

    public JavascriptBuilder()
    {
        _builder = new StringBuilder(1024);
    }

    public override string ToString()
    {
        return _builder.ToString();
    }

    public JavascriptBuilder DeclareArray(string name, IEnumerable<string> items)
    {
        _builder.Append($"const {name} = [");
        _builder.AppendJoin(", ", items.Select(item => $"'{item}'"));
        _builder.AppendLine("];");
        return this;
    }

    public JavascriptBuilder DeclareConst(string name, string value)
    {
        _builder.AppendLine($"const {name} = {value};");
        return this;
    }

    public JavascriptBuilder DocumentWriteLine(string line)
    {
        _builder.AppendLine($"document.writeln('{line}');");
        return this;
    }

    public JavascriptBuilder Block(string code)
    {
        _builder.AppendLine(code);
        return this;
    }
}

using System.Text;

namespace Bookgen.Lib;

public sealed class MarkdownBuilder
{
    private readonly StringBuilder _builder;

    public MarkdownBuilder()
    {
        _builder = new StringBuilder();
    }

    public override string ToString()
        => _builder.ToString();

    public MarkdownBuilder Heading(int level, string text)
    {
        if (level < 1 || level > 6)
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be between 1 and 6.");
        _builder.AppendLine($"{new string('#', level)} {text}");
        return this;
    }

    public MarkdownBuilder Paragraph(string text)
    {
        _builder.AppendLine(text);
        _builder.AppendLine();
        return this;
    }

    public MarkdownBuilder List(IEnumerable<string> items)
    {
        foreach (var item in items)
        {
            _builder.AppendLine($"- {item}");
        }
        _builder.AppendLine();
        return this;
    }

    public MarkdownBuilder CodeBlock(string code, string? language = null)
    {
        if (language != null)
        {
            _builder.AppendLine($"```{language}");
        }
        else
        {
            _builder.AppendLine("```");
        }
        _builder.AppendLine(code);
        _builder.AppendLine("```");
        return this;
    }

    public MarkdownBuilder Link(string text, string url)
    {
        _builder.AppendLine($"[{text}]({url})");
        return this;
    }

    public MarkdownBuilder Image(string altText, string url)
    {
        _builder.AppendLine($"![{altText}]({url})");
        return this;
    }

    public MarkdownBuilder HorizontalRule()
    {
        _builder.AppendLine("---");
        return this;
    }
}

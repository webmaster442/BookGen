using System.Diagnostics;
using System.Text;

using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

namespace Bookgen.Lib.Pipeline;

internal sealed class TocRenderer
{
    private readonly StringBuilder _buffer;
    private readonly TableOfContentsConfiguration _configuration;

    public TocRenderer(TableOfContentsConfiguration configuration)
    {
        _buffer = new StringBuilder(4096);
        _configuration = configuration;
    }

    public override string ToString()
        => _buffer.ToString();

    private static string ToHtml(ContainerElement element)
    {
        return Enum.GetName<ContainerElement>(element)?.ToLower()
            ?? throw new UnreachableException();
    }

    private static string ToHtml(ItemContainer itemContainer)
    {
        return itemContainer switch
        {
            ItemContainer.UnorderedList => "li",
            ItemContainer.OrderedList => "li",
            ItemContainer.Details => "",
            ItemContainer.Paragraph => "p",
            ItemContainer.Span => "span",
            _ => throw new UnreachableException()
        };
    }

    public void BeginContainer(params (string attribute, string attributeValue)[] additonalProps)
    {
        _buffer
        .Append('<')
            .Append(ToHtml(_configuration.ContainerElement));

        if (!string.IsNullOrEmpty(_configuration.ContainerId))
        {
            _buffer
                .Append(" id=\"")
                .Append(_configuration.ContainerId)
                .Append('"');
        }

        foreach (var (attribute, attributeValue) in additonalProps)
        {
            _buffer
                .Append(' ')
                .Append(attribute)
                .Append("=\"")
                .Append(attributeValue)
                .Append('"');
        }

        if (!string.IsNullOrEmpty(_configuration.ContainerClass))
        {
            _buffer
                .Append(" class=\"")
                .Append(_configuration.ContainerClass)
                .Append('"');
        }

        _buffer.Append('>').AppendLine();
    }

    public void BeginChapter(string chapter)
    {
        _buffer.Append('<')
            .Append(ToHtml(_configuration.ChapterContainer))
            .Append('>')
            .Append("<h1>")
            .Append(chapter)
            .AppendLine("</h1>");
    }

    public void BeginOuterItemContainer()
    {
        switch (_configuration.ItemContainer)
        {
            case ItemContainer.UnorderedList:
                _buffer.AppendLine("<ul>");
                break;
            case ItemContainer.OrderedList:
                _buffer.AppendLine("<ol>");
                break;
            case ItemContainer.Details:
                _buffer.AppendLine("<details>");
                break;
        }
    }

    public string Add(string file, IWritableFileSystem output, string title, string host)
    {
        string linkTarget = Path.ChangeExtension(Path.Combine(output.Scope, file), ".html");
        linkTarget = Path.GetRelativePath(output.Scope, linkTarget).Replace("\\", "/");

        string link = $"{host}{linkTarget}";

        _buffer
            .Append('<')
            .Append(ToHtml(_configuration.ItemContainer))
            .Append('>')
            .Append($"<a href=\"{link}\">{title}</a>")
            .Append("</")
            .Append(ToHtml(_configuration.ItemContainer))
            .Append('>')
            .AppendLine();

        return link;
    }

    public string AddEpubLink(string file, string title)
    {
        string linkTarget = Path.ChangeExtension(file, ".xhtml");
        _buffer
            .Append('<')
            .Append(ToHtml(_configuration.ItemContainer))
            .Append('>')
            .Append($"<a href=\"{linkTarget}\">{title}</a>")
            .Append("</")
            .Append(ToHtml(_configuration.ItemContainer))
            .Append('>')
            .AppendLine();

        return linkTarget;
    }

    public void EndOuterItemContainer()
    {
        switch (_configuration.ItemContainer)
        {
            case ItemContainer.UnorderedList:
                _buffer.AppendLine("</ul>");
                break;
            case ItemContainer.OrderedList:
                _buffer.AppendLine("</ol>");
                break;
            case ItemContainer.Details:
                _buffer.AppendLine("</details>");
                break;
        }
    }

    public void EndChapter()
    {
        _buffer.Append("</")
            .Append(ToHtml(_configuration.ChapterContainer))
            .Append('>')
            .AppendLine();
    }

    public void EndContainer()
    {
        _buffer
            .Append("</")
            .Append(ToHtml(_configuration.ContainerElement))
            .Append('>')
            .AppendLine();
    }
}

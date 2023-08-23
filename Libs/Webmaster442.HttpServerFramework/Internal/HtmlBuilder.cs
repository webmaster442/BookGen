// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2023 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Text;

namespace Webmaster442.HttpServerFramework.Internal;

internal sealed class HtmlBuilder
{
    private readonly string _title;
    private readonly StringBuilder _content;

    public HtmlBuilder(string title)
    {
        _title = title;
        _content = new StringBuilder(4096);
        CreateHtmlDocHeaders();
    }

    private void CreateHtmlDocHeaders()
    {
        _content.AppendLine("<!doctype html>");
        _content.AppendLine("<html lang=\"en\">");
        _content.AppendLine("<head><meta charset=\"utf-8\">");
        _content.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
        _content.AppendLine("<title>");
        _content.AppendLine(_title);
        _content.AppendLine("</title></head><body>");
    }

    private void AppendElement(string element, string text)
    {
        _content.AppendLine($"<{element}>");
        _content.Append(text);
        _content.AppendLine($"</{element}>");
    }

    public HtmlBuilder AppendParagraph(string text)
    {
        AppendElement("p", text);
        return this;
    }

    public HtmlBuilder AppendHeader(int level, string text)
    {
        if (level < 1 || level > 6)
            throw new ArgumentOutOfRangeException(nameof(level), "level must be between 1 and 6");

        AppendElement($"h{level}", text);
        return this;
    }

    public HtmlBuilder AppendHr()
    {
        _content.Append("<hr/>");
        return this;
    }

    public HtmlBuilder AppendLineBreak()
    {
        _content.Append("<br/>");
        return this;
    }

    public HtmlBuilder AppendImage(string imgUrl, string altText = "")
    {
        _content.Append($"<img src=\"{imgUrl}\" alt=\"{altText}\"/>");
        return this;
    }

    public HtmlBuilder AppendPre(string text)
    {
        AppendElement("pre", text);
        return this;
    }

    public HtmlBuilder AppendBeginElement(Element element, string id="")
    {
        if (string.IsNullOrEmpty(id))
            _content.Append($"<{element.ToString().ToLower()}>");
        else
            _content.Append($"<{element.ToString().ToLower()} id=\"{id}\">");

        return this;
    }

    public HtmlBuilder AppendEndElement(Element element)
    {
        _content.Append($"</{element.ToString().ToLower()}>");
        return this;
    }

    public HtmlBuilder AppendLink(string url, string title)
    {
        _content.Append($"<a href=\"{url}\">{title}</a>");
        return this;
    }

    public HtmlBuilder UnorderedList<T>(IEnumerable<T> enumerable, Func<T, string> itemSelector)
    {
        _content.AppendLine("<ul>");
        foreach (var item in enumerable)
        {
            _content.Append("<li>");
            _content.Append(itemSelector.Invoke(item));
            _content.Append("</li>");
        }
        _content.AppendLine("</ul>");
        return this;
    }

    public HtmlBuilder AppendCss(string css)
    {
        _content.AppendLine("<style type=\"text/css\">");
        _content.AppendLine(css);
        _content.AppendLine("</style>");
        return this;
    }

    private void CloseHtml()
    {
        _content.AppendLine("</body></html>");
    }

    public override string ToString()
    {
        CloseHtml();
        return _content.ToString();
    }
}

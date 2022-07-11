// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Text;

namespace Webmaster442.HttpServerFramework.Internal;

internal class HtmlBuilder
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
        _content.AppendLine("<html>");
        _content.AppendLine("<head><meta charset=\"utf-8\">");
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

    public void AppendParagraph(string text)
    {
        AppendElement("p", text);
    }

    public void AppendHeader(int level, string text)
    {
        if (level < 1 || level > 6)
            throw new ArgumentOutOfRangeException(nameof(level), "level must be between 1 and 6");

        AppendElement($"h{level}", text);
    }

    internal void AppendHr()
    {
        _content.Append("<hr/>");
    }

    public void AppendPre(string text)
    {
        AppendElement("pre", text);
    }

    public void UnorderedList<T>(IEnumerable<T> enumerable, Func<T, string> itemSelector)
    {
        _content.AppendLine("<ul>");
        foreach (var item in enumerable)
        {
            _content.Append("<li>");
            _content.Append(itemSelector.Invoke(item));
            _content.Append("</li>");
        }
        _content.AppendLine("</ul>");
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

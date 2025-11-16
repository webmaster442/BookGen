//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace Bookgen.Lib.Pipeline;

internal sealed class EpubTocRenderer
{
    private readonly StringBuilder _buffer;

    public EpubTocRenderer()
    {
        _buffer = new StringBuilder(4096);
    }

    public override string ToString() => _buffer.ToString();

    internal EpubTocRenderer AddItem(string title, string link)
    {
        _buffer.Append("<li>")
            .Append("<a href=\"")
            .Append(link)
            .Append("\">")
            .Append(title)
            .Append("</a>")
            .AppendLine("</li>");
        return this;
    }

    public void BeginNav()
    {
        _buffer.AppendLine("<nav id=\"toc\" epub:type=\"toc\">");
    }

    public void EndNav()
    {
        _buffer.AppendLine("</nav>");
    }

    internal void AddTitle(string bookTitle)
    {
        _buffer.AppendLine("<h1 class=\"title\">")
            .Append(bookTitle)
            .AppendLine("</h1>");
    }

    public void BeginOl(bool display = true)
    {
        if (display)
            _buffer.AppendLine("<ol class=\"toc\">");
        else
            _buffer.AppendLine("<ol style=\"list-style: none;\">");
    }

    public void EndOl()
    {
        _buffer.AppendLine("</ol>");
    }

    public void BeginChapter(string title)
    {
        _buffer.Append("<li><span>").Append(title).Append("</span>");
    }

    public void EndChapter()
    {
        _buffer.AppendLine("</li>");
    }
}

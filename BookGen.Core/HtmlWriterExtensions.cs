//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace BookGen.Core
{
    public enum HtmlElement
    {
        H1,
        H2,
        H3,
        H4,
        H5,
        Ul,
        Li,
        Div,
        Table,
    }

    public static class HtmlWriterExtensions
    {
        public static StringBuilder WriteElement(this StringBuilder stringBuilder, HtmlElement element, string id, params string[] cssclasses)
        {
            var css = string.Join(' ', cssclasses);
            stringBuilder.AppendFormat("<{0}", element.ToString().ToLower());
            if (!string.IsNullOrEmpty(id))
                stringBuilder.AppendFormat(" id=\"{0}\"", id);
            if (!string.IsNullOrEmpty(css))
                stringBuilder.AppendFormat(" class=\"{0}\"", css);
            stringBuilder.Append(">");

            return stringBuilder;
        }

        public static StringBuilder WriteHeader(this StringBuilder stringBuilder, int level, string format, params object[] args)
        {
            stringBuilder.AppendFormat("<h{0}>", level);
            stringBuilder.AppendFormat(format, args);
            stringBuilder.AppendFormat("</h{0}>", level);
            return stringBuilder;
        }

        public static StringBuilder WriteParagraph(this StringBuilder stringBuilder, string format, params object[] args)
        {
            stringBuilder.Append("<p>");
            stringBuilder.AppendFormat(format, args);
            stringBuilder.Append("</p>");
            return stringBuilder;
        }
        
        public static StringBuilder WriteHref(this StringBuilder stringBuilder, string link, string displaystring, params string[] cssclasses)
        {
            var css = string.Join(' ', cssclasses);

            if (!string.IsNullOrEmpty(css))
                stringBuilder.AppendFormat("<a class=\"{0}\" href=\"{1}\">{2}</a>", css, link, displaystring);
            else
                stringBuilder.AppendFormat("<a href=\"{0}\">{1}</a>", link, displaystring);

            return stringBuilder;
        }

        public static StringBuilder WriteElement(this StringBuilder stringBuilder, HtmlElement element)
        {
            return WriteElement(stringBuilder, element, string.Empty);
        }

        public static StringBuilder CloseElement(this StringBuilder stringBuilder, HtmlElement element)
        {
            stringBuilder.AppendFormat("</{0}>", element.ToString().ToLower());
            return stringBuilder;
        }

        public static StringBuilder WriteTableRow(this StringBuilder stringBuilder, params object[] rowcontents)
        {
            stringBuilder.Append("<tr>");
            foreach (var item in rowcontents)
            {
                stringBuilder.AppendFormat("<td>{0}</td>", item);
            }
            stringBuilder.Append("</tr>");
            return stringBuilder;
        }

        public static StringBuilder WriteTableHeader(this StringBuilder stringBuilder, params object[] rowcontents)
        {
            stringBuilder.Append("<tr>");
            foreach (var item in rowcontents)
            {
                stringBuilder.AppendFormat("<th>{0}</th>", item);
            }
            stringBuilder.Append("</tr>");
            return stringBuilder;
        }
    }
}

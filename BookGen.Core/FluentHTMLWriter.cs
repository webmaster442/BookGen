//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace BookGen.Core
{
    public sealed class FluentHtmlWriter
    {
        private readonly StringBuilder _buffer;

        public static class Tags
        {
            public const string Div = "div";
            public const string Span = "span";
            public const string H1 = "h1";
            public const string H2 = "h2";
            public const string H3 = "h3";
            public const string H4 = "h4";
            public const string H5 = "h5";
            public const string H6 = "h6";
            public const string Br = "br";
            public const string Pre = "Pre";
            public const string Code = "Code";
            public const string Paragraph = "p";
            public const string Link = "a";
            public const string Image = "img";
        }

        public FluentHtmlWriter()
        {
            _buffer = new StringBuilder(4096);
        }

        public FluentHtmlWriter(string initialContent)
        {
            _buffer = new StringBuilder(initialContent);
        }

        public FluentHtmlWriter WriteJavaScript(string content)
        {
            return WriteElement("script", content, ("type", "text/javascript"));
        }

        public FluentHtmlWriter WriteStyle(string content)
        {
            return WriteElement("style", content, ("type", "text/css"));
        }

        public FluentHtmlWriter WriteElement(string tag, string content, params (string name, string value)[] attributeList)
        {
            if (attributeList == null || attributeList.Length == 0)
            {
                if (string.IsNullOrEmpty(content))
                    _buffer.AppendFormat("<{0}/>\r\n", tag);
                else
                    _buffer.AppendFormat("<{0}>{1}</{0}>\r\n", tag, content);

                return this;
            }

            StringBuilder attributes = new StringBuilder();
            for (int i=0; i<attributeList?.Length; i++)
            {
                (string name, string value) = attributeList[i];
                bool last = i == attributeList?.Length - 1;
                attributes.AppendFormat("{0}=\"{1}\"", name, value);
                if (!last)
                    attributes.Append(" ");
            }

            _buffer.AppendFormat("<{0} {1}>{2}</{0}>\r\n", tag, attributes, content);

            return this;
        }

        public FluentHtmlWriter WriteElement(string tag)
        {
            return WriteElement(tag, string.Empty);
        }

        public override string ToString()
        {
            return _buffer.ToString();
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace BookGen.AssemblyDocumenter.Internals
{
    internal class MarkdownDocument
    {
        private readonly StringBuilder _buffer;

        public MarkdownDocument()
        {
            _buffer = new StringBuilder(4096);
        }

        public void Paragraph(string format, params object[] parameters)
        {
            _buffer.AppendFormat(format+"\r\n\r\n", parameters);
        }

        public void WriteLine(string format, params object[] parameters)
        {
            _buffer.AppendFormat(format + "\r\n", parameters);
        }

        public void Heading(int level, string format, params object[] parameters)
        {
            StringBuilder head = new StringBuilder();
            for (int i=0; i<level; i++)
            {
                head.Append('#');
            }
            head.Append(' ');
            _buffer.AppendFormat(head.ToString()+ format + "\r\n\r\n", parameters);
        }

        public override string ToString()
        {
            return _buffer.ToString();
        }
    }
}

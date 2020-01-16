//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BookGen.Framework.Editor
{
    public class FileTreeRenderer
    {
        private string Walk(string folder)
        {
            var buffer = new StringBuilder();
            var folders = new Stack<string>(Directory.GetDirectories(folder));

            while (folders.Count > 0)
            {
                string current = folders.Pop();
                var files = Directory.GetFiles(current, "*.*");

                buffer.Append("<details>\n");
                buffer.AppendFormat("<summary>{0}</summary>\n", current);

                RenderFiles(buffer, files);

                var subDirectories = Directory.GetDirectories(current);
                foreach (var directory in subDirectories)
                {
                    folders.Push(directory);
                }
                buffer.Append("</details>\n");

            }
            return buffer.ToString();
        }

        private void RenderFiles(StringBuilder buffer, string[] files)
        {
            buffer.Append("<ul>\n");
            foreach (var file in files)
            {
                buffer.AppendFormat("<li>{0}</li>\n", file);
            }
            buffer.Append("</ul>\n");
        }

        public byte[] Render(string workdir)
        {
            return Encoding.UTF8.GetBytes(Walk(workdir));
        }
    }
}

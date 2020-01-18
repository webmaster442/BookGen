//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BookGen.Framework.Editor
{
    public static class FileTreeRenderer
    {
        public static string Render(string folder)
        {
            var buffer = new StringBuilder();
            var folders = new Stack<string>(Directory.GetDirectories(folder));

            buffer.AppendFormat("<h3>{0}</h3>\n", folder);

            while (folders.Count > 0)
            {
                string current = folders.Pop();
                var files = Directory.GetFiles(current, "*.*");

                buffer.Append("<details open>\n");
                buffer.AppendFormat("<summary>{0}</summary>\n", GetName(current, folder));

                RenderFiles(buffer, files, current, folder);

                var subDirectories = Directory.GetDirectories(current);
                foreach (var directory in subDirectories)
                {
                    folders.Push(directory);
                }
                buffer.Append("</details>\n");

            }
            return buffer.ToString();
        }

        private static string GetName(string item, string rootfolder)
        {
            return item.Substring(rootfolder.Length);
        }

        private static void RenderFiles(StringBuilder buffer, string[] files, string currentFolder, string rootFolder)
        {
            if (files.Length < 1) return;

            buffer.Append("<ul>\n");
            foreach (var file in files)
            {
                string filepath = GetName(file, currentFolder);
                string linkpath = GetName(file, rootFolder);
                buffer.AppendFormat("<li>{0}", filepath);
                if (IsMarkdownFile(file))
                {
                    string param = Convert.ToBase64String(Encoding.UTF8.GetBytes(linkpath));
                    buffer.AppendFormat(" <a target=\"_blank\" href=\"/Edit.html?file={0}\">[Edit]</a>\n", param);
                    buffer.AppendFormat(" <a target=\"_blank\" href=\"/Preview.html?file={0}\">[Preview]</a>\n", param);
                }
                else
                {
                    buffer.AppendFormat(" <a href=\"{0}\">[Open]</a>\n", linkpath);
                }
                buffer.Append("</li>\n");
            }
            buffer.Append("</ul>\n");
        }

        private static bool IsMarkdownFile(string file)
        {
            return string.Equals(Path.GetExtension(file), ".md", StringComparison.OrdinalIgnoreCase); 
        }
    }
}

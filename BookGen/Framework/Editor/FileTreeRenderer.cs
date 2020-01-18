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

            string[] files = Directory.GetFiles(folder, "*.*");

            RenderFiles(buffer, files, folder, folder);

            while (folders.Count > 0)
            {
                string current = folders.Pop();
                files = Directory.GetFiles(current, "*.*");

                RenderFiles(buffer, files, current, folder);

                string[] subDirectories = Directory.GetDirectories(current);
                foreach (var directory in subDirectories)
                {
                    folders.Push(directory);
                }

            }
            return buffer.ToString();
        }

        private static string GetName(string item, string rootfolder)
        {
            string name = item.Substring(rootfolder.Length);
            if (name.Length > 0)
                return name;
            else
                return "\\";
        }

        private static void RenderFiles(StringBuilder buffer, string[] files, string currentFolder, string rootFolder)
        {
            if (files.Length < 1) return;
            buffer.Append("<details open>\n");
            buffer.AppendFormat("<summary>{0}</summary>\n", GetName(currentFolder, rootFolder));
            buffer.Append("<ul>\n");
            foreach (var file in files)
            {
                string filepath = GetName(file, currentFolder);
                string linkpath = GetName(file, rootFolder);
                buffer.AppendFormat("<li>{0}", filepath);
                if (IsMarkdownFile(file))
                {
                    string param = Convert.ToBase64String(Encoding.UTF8.GetBytes(linkpath));
                    buffer.AppendFormat(" <a target=\"_blank\" href=\"/editor.html?file={0}\">[Edit]</a>\n", Uri.EscapeDataString(param));
                    buffer.AppendFormat(" <a target=\"_blank\" href=\"/preview.html?file={0}\">[Preview]</a>\n", Uri.EscapeDataString(param));
                }
                else
                {
                    buffer.AppendFormat(" <a href=\"{0}\">[Open]</a>\n", linkpath);
                }
                buffer.Append("</li>\n");
            }
            buffer.Append("</ul>\n");
            buffer.Append("</details>\n");
        }

        private static bool IsMarkdownFile(string file)
        {
            return string.Equals(Path.GetExtension(file), ".md", StringComparison.OrdinalIgnoreCase); 
        }
    }
}

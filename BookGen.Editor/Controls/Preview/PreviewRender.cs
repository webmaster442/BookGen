//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Markdown;
using BookGen.Editor.Infrastructure;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Editor.Controls
{
    internal static class PreviewRender
    {

        public static Task<string> RenderPreviewForMd(string md)
        {
            return Task.Run(() => RenderPreviewForMdJob(md));
        }

        private static string RenderPreviewForMdJob(string md)
        {
            StringBuilder buffer = new StringBuilder();

            RenderHtmlHeaderWithCss(buffer);

            var html = MarkdownRenderers.Markdown2Preview(md, new FsPath(EditorSessionManager.CurrentSession.WorkDirectory));

            buffer.AppendLine(html);

            buffer.AppendLine("</body></html>");

            return buffer.ToString();
        }

        private static void RenderHtmlHeaderWithCss(StringBuilder buffer)
        {
            buffer.AppendLine("<!DOCTYPE html>");
            buffer.AppendLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            buffer.AppendLine("<head>");
            buffer.AppendLine("<meta charset=\"utf-8\"/>");
            buffer.AppendLine("<style type=\"text/css\">");
            buffer.AppendLine("");
            buffer.AppendLine("</style>");
            buffer.AppendLine("</head><body>");
        }
    }
}

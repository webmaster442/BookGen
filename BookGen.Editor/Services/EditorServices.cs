//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;
using System;
using System.IO;
using System.Text;

namespace BookGen.Editor.Services
{
    internal static class EditorServices
    {
        private static MarkdownPipeline _previewPipeline;

        static EditorServices()
        {
            _previewPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }

        public static void LaunchEditorFor(string file)
        {
            EditorWindow editor = new EditorWindow();
            editor.Show();
        }

        public static string RenderPreview(string inputMd)
        {
            StringBuilder preview = new StringBuilder();

            preview.Append("<html><head></head><body>");
            preview.Append(Markdown.ToHtml(inputMd, _previewPipeline));
            preview.Append("</body></html>");

            return preview.ToString() ;
        }
    }
}

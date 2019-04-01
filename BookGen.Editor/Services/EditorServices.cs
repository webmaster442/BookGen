//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig;

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
            return Markdown.ToHtml(inputMd, _previewPipeline);
        }
    }
}

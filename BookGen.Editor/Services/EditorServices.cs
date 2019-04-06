//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using Markdig;
using System.IO;
using System.Web;
using System.Xml;

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
            EditorWindow editor = new EditorWindow(new FsPath(file));
            editor.Show();
        }

        public static string RenderPreview(string inputMd, FsPath _file)
        {
            var html = "<div>" + Markdown.ToHtml(inputMd, _previewPipeline) + "</div>";

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(html);

            var images = xml.SelectNodes("//img");

            foreach (XmlNode image in images)
            {
                var src = HttpUtility.UrlDecode(image.Attributes["src"].InnerText);
                var full = new FsPath(src).GetAbsolutePathTo(_file).ToString();
                image.Attributes["src"].InnerText = full;
            }

            using (var stringWriter = new StringWriter())
            {
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    xml.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    html = stringWriter.ToString();
                }
            }

            return html;

        }
    }
}

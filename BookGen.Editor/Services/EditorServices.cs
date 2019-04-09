//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public static IHighlightingDefinition LoadHighlightingDefinition()
        {
            var resourceName = "BookGen.Editor.MardkownSyntax.xshd";
            var type = typeof(EditorServices);
            using (var stream = type.Assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new XmlTextReader(stream))
                {
                    return HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
        }

        public static string RenderPreview(string inputMd, FsPath _file)
        {
            var html = "<div>" + Markdown.ToHtml(inputMd, _previewPipeline) + "</div>";

            if (_file == null) return html;

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

        public static Dictionary<int, string> GetWords(string line)
        {
            var ret = new Dictionary<int, string>();
            StringBuilder word = new StringBuilder();
            int wordstart = -1;
            for (int i=0; i<line.Length; i++)
            {
                if ((line[i] == '\t' 
                    || line[i] == ' ') && word.Length > 0)
                {
                    if (word.ToString().Trim().Length > 0)
                    {
                        ret.Add(wordstart, word.ToString());
                        word.Clear();
                        wordstart = -1;
                    }
                }
                else
                {
                    if (wordstart == -1) wordstart = i;
                    word.Append(line[i]);
                }
            }
            if (wordstart != -1 && word.Length > 0)
            {
                ret.Add(wordstart, word.ToString());
            }
            return ret;
        }

        public static bool HunspellConfigured()
        {
            if (!File.Exists(Properties.Settings.Default.Editor_AffFile))
                return false;

            if (!File.Exists(Properties.Settings.Default.Editor_DictFile))
                return false;

            return true;
        }
    }
}
//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Markdown;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace BookGen.Editor.Services
{
    internal static class EditorServices
    {

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
            return MarkdownRenderers.Markdown2PreviewHtml(inputMd, _file);
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
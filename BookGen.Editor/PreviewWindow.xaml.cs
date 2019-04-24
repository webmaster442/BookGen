//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.Services;
using BookGen.Editor.View;
using System.Windows;

namespace BookGen.Editor
{
    internal enum PreviewControl
    {
        Text,
        Web,
        None,
    }

    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        public PreviewWindow(string file)
        {
            InitializeComponent();
            var extension = System.IO.Path.GetExtension(file);
            PreviewControl control = Identify(extension);
            CreateControlAndLoad(control, file, extension);

        }

        private static PreviewControl Identify(string extension)
        {
            switch (extension)
            {
                case ".js":
                case ".css":
                case ".less":
                case ".ts":
                case ".txt":
                case ".csv":
                case ".gitignore":
                case ".gitattributes":
                case ".json":
                case ".cs":
                case ".c":
                case ".cpp":
                case ".h":
                    return PreviewControl.Text;
                case ".md":
                case ".htm":
                case ".html":
                case ".xml":
                    return PreviewControl.Web;
                default:
                    return PreviewControl.None;
            }
        }

        public static bool CanOpen(string file)
        {
            var extension = System.IO.Path.GetExtension(file);
            PreviewControl control = Identify(extension);
            return control != PreviewControl.None;
        }

        private void CreateControlAndLoad(PreviewControl control, string file, string extension)
        {
            switch(control)
            {
                case PreviewControl.Text:
                    CreateTextContol(file);
                    break;
                case PreviewControl.Web:
                    CreateWebContol(file, extension);
                    break;
                default:
                    return;
            }
        }

        private void CreateWebContol(string file, string extension)
        {
            FsPath fs = new FsPath(file);
            var browser = new HTMLView();
            if (extension == ".md")
            {
                var partial = EditorServices.RenderPreview(fs.ReadFile(), fs);
                browser.RenderPartialHtml(partial);
            }
            else
            {
                var full = fs.ReadFile();
                browser.RenderHTML(full);
            }
            Content = browser;
        }

        private void CreateTextContol(string file)
        {
            var editor = new EditorWrapper
            {
                IsReadOnly = true,
                ShowLineNumbers = true,
                ScrollBelowDocument = false
            };
            editor.Load(file);
            Content = editor;
        }
    }
}

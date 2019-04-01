//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Services;
using System.Windows;

namespace BookGen.Editor
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow
    {
        public EditorWindow()
        {
            InitializeComponent();
        }

        private void Backstage_IsOpenChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            string mdHtml = EditorServices.RenderPreview(Editor.Text);
            HtmlView.RenderPartialHtml(mdHtml);
        }
    }
}

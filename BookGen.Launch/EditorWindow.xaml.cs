//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Launch.Code;
using ICSharpCode.AvalonEdit.Document;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Launch
{
    public partial class EditorWindow : Window, IEditorDialog
    {
        public IDocument Document => Editor.Document;

        public EditorWindow()
        {
            InitializeComponent();
        }

        private void Chrome_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
                Owner.Left = Left - ((Owner.Width - Width) / 2);
                Owner.Top = Top - ((Owner.Height - Height) / 2);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

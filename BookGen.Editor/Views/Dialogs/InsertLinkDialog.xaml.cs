//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace BookGen.Editor.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for InsertLinkDialog.xaml
    /// </summary>
    public partial class InsertLinkDialog : Window
    {
        public InsertLinkDialog()
        {
            InitializeComponent();
        }

        public string LinkText
        {
            get { return TbLinkText.Text; }
        }

        public string Link
        {
            get { return TbLink.Text; }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            DialogResult = DialogCommons.HandleCloseButtons(e);
        }
    }
}

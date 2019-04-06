//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;

namespace BookGen.Editor.Dialogs
{
    /// <summary>
    /// Interaction logic for InsertLink.xaml
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
    }
}

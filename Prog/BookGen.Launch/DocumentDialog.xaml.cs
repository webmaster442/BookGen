//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;

namespace BookGen.Launch
{
    /// <summary>
    /// Interaction logic for DocumentDialog.xaml
    /// </summary>
    public partial class DocumentDialog : Window
    {
        public DocumentDialog(Window parent, string title, string documentContent)
        {
            InitializeComponent();
            Width = parent.ActualWidth * 0.9;
            Height = parent.ActualHeight * 0.9;
            Title = title;
            Browser.HtmlText = documentContent;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}

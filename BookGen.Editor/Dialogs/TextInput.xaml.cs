//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;

namespace BookGen.Editor.Dialogs
{
    /// <summary>
    /// Interaction logic for TextInput.xaml
    /// </summary>
    public partial class TextInput : Window
    {
        public TextInput(string title)
        {
            InitializeComponent();
            Title = title;
        }

        public string Inputstring
        {
            get { return TbInput.Text; }
            set { TbInput.Text = value; }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;
using System.Windows.Input;

namespace BookGen.Editor.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for TextInput.xaml
    /// </summary>
    public partial class TextInputDialog : Window
    {
        public TextInputDialog(string title)
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool? result = DialogCommons.HandleCloseButtons(e);
            if (result.HasValue)
            {
                DialogResult = result.Value;
            }
        }
    }
}

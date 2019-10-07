//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookGen.Editor.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for GotoLineDialog.xaml
    /// </summary>
    public partial class GotoLineDialog : Window
    {
        public GotoLineDialog(int maxlines, int currentline)
        {
            InitializeComponent();
            Slider.Minimum = 1;
            Slider.Maximum = maxlines;
            Slider.Value = currentline;
        }

        public int Line
        {
            get { return (int)Slider.Value; }
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

//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows;

namespace BookGen.Launch
{
    /// <summary>
    /// Interaction logic for MessageBox.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        public MessageDialog()
        {
            DialogText = string.Empty;
            InitializeComponent();
        }

        public MessageBoxImage Image
        {
            get { return (MessageBoxImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(MessageBoxImage), typeof(MessageDialog), new PropertyMetadata(MessageBoxImage.None));

        public MessageBoxButton Buttons
        {
            get { return (MessageBoxButton)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        public static readonly DependencyProperty ButtonsProperty =
            DependencyProperty.Register("Buttons", typeof(MessageBoxButton), typeof(MessageDialog), new PropertyMetadata(MessageBoxButton.OK));

        public MessageBoxResult ClickedButton { get; private set; }

        public string DialogText { get; set; }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            ClickedButton = MessageBoxResult.OK;
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClickedButton = MessageBoxResult.Cancel;
            DialogResult = true;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            ClickedButton = MessageBoxResult.Yes;
            DialogResult = true;
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            ClickedButton = MessageBoxResult.No;
            DialogResult = true;
        }

    }
}

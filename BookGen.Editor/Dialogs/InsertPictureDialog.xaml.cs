//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookGen.Editor.Dialogs
{
    /// <summary>
    /// Interaction logic for InsertPictureDialog.xaml
    /// </summary>
    public partial class InsertPictureDialog : Window
    {
        public InsertPictureDialog()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LocalImages.ItemsSource = new ObservableCollection<string>(FileSystemServices.GetImagesInWorkDir());
        }

        public string Url
        {
            get { return TbUrl.Text; }
        }

        public string Alt
        {
            get { return TbAlt.Text; }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}

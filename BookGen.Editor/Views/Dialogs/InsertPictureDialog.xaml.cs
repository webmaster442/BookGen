//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using BookGen.Editor.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookGen.Editor.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for InsertPictureDialog.xaml
    /// </summary>
    internal partial class InsertPictureDialog : Window
    {
        private ObservableCollection<string> _files;
        private readonly IFileSystemServices _fileSystemServices;
        private readonly FsPath _editedFile;

        public InsertPictureDialog(FsPath editedFile, IFileSystemServices fileSystemServices)
        {
            InitializeComponent();
            _fileSystemServices = fileSystemServices;
            _editedFile = editedFile;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _files = new ObservableCollection<string>(_fileSystemServices.GetImagesInWorkDir(EditorSessionManager.CurrentSession.WorkDirectory));
            LocalImages.ItemsSource = _files;
        }

        public string Url
        {
            get { return TbUrl.Text; }
        }

        public string Alt
        {
            get { return TbAlt.Text; }
        }

        public bool IsFigure
        {
            get { return CbInsertAsFigure.IsChecked ?? false; }
        }

        private void BtnInsert_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void LocalImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LocalImages == null || LocalImages.SelectedIndex < 0) return;

            if (LocalImages.ItemsSource is ObservableCollection<string> currentItems)
            {
                FsPath selected = new FsPath(currentItems[LocalImages.SelectedIndex]);
                TbUrl.Text = selected.GetRelativePathRelativeTo(_editedFile).ToString();
            }
        }

        private void TbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TbFilter.Text))
            {
                LocalImages.ItemsSource = _files;
                return;
            }


            var filter = from item in _files
                         where
                            item.Contains(TbFilter.Text)
                         select item;


            LocalImages.ItemsSource = new ObservableCollection<string>(filter);
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

using System;
using System.IO;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using Microsoft.Win32;

namespace BookGen.FormulaEdit
{
    /// <summary>
    /// Interaction logic for ExportDialog.xaml
    /// </summary>
    public partial class ExportDialog : CustomDialog
    {
        private readonly MetroWindow _owner;

        public bool DialogResult { get; private set; }

        public ExportDialog(MetroWindow owner)
        {
            InitializeComponent();
            _owner = owner;
            TbFolderPath.Text = AppContext.BaseDirectory;
        }

        private async void BtnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
            await _owner.HideMetroDialogAsync(this);
        }

        private async void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
            await _owner.HideMetroDialogAsync(this);
        }

        private void BtnBrowse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenFolderDialog ofd = new OpenFolderDialog();
            if (Directory.Exists(TbFolderPath.Text))
                ofd.FolderName = TbFolderPath.Text;
            if (ofd.ShowDialog() == true)
            {
                TbFolderPath.Text = ofd.FolderName;
            }
        }
    }
}

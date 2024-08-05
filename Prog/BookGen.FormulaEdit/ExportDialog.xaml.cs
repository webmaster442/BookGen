//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

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
        public bool IsComplete { get; internal set; }

        public ExportDialog(MetroWindow owner, string startLocation)
        {
            InitializeComponent();
            _owner = owner;
            TbFolderPath.Text = startLocation;
        }

        private async void BtnOk_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
            await _owner.HideMetroDialogAsync(this);
            IsComplete = true;
        }

        private async void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
            await _owner.HideMetroDialogAsync(this);
            IsComplete = true;
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

//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Wpf;
using BookGen.Launch.Code;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Launch
{
    internal class EditorViewModel : ViewModelBase
    {
        private bool _exportRaw;
        private bool _exportSyntaxHighlight;
        private string _fileName;
        private bool _isDirty;
        private int _tabIndex;

        public bool ExportRaw
        {
            get => _exportRaw;
            set => Set(ref _exportRaw, value);
        }

        public bool ExportSyntaxHighlight
        {
            get => _exportSyntaxHighlight;
            set => Set(ref _exportSyntaxHighlight, value);
        }

        public bool IsDirty
        {
            get => _isDirty;
            set => Set(ref _isDirty, value);
        }

        public string FileName
        {
            get => _fileName;
            set => Set(ref _fileName, value);
        }

        public int TabIndex
        {
            get => _tabIndex;
            set
            {
                Set(ref _tabIndex, value);
                RefreshPreviewIfNeeded();
            }
        }

        public ICommand ExportFile { get; }
        public ICommand ExportClipboard { get; }
        public ICommand Open { get; }
        public ICommand Save { get; }
        public ICommand SaveAs { get; }
        public IEditorDialog View { get; }

        public string PreviewHtml { get; private set; }

        public EditorViewModel(IEditorDialog view)
        {
            ExportFile = new DelegateCommand(OnExportFile);
            ExportClipboard = new DelegateCommand(OnExportClipboard);
            Open = new DelegateCommand(OnOpen);
            Save = new DelegateCommand(OnSave);
            SaveAs = new DelegateCommand(OnSaveAs);
            _fileName = string.Empty;
            View = view;
            PreviewHtml = string.Empty;
        }

        private bool TryExport(string text, bool raw, bool syntaxhighlight, out string Output)
        {
            try
            {
                string? fn = Path.GetTempFileName();
                File.WriteAllText(fn, text);
                StringBuilder sb = new();
                sb.Append($"md2html -i \"{fn}\" -o con");
                if (raw)
                    sb.Append(" -r");
                if (!syntaxhighlight)
                    sb.Append(" -ns");

                string? arguments = sb.ToString();

                using var process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WorkingDirectory = AppContext.BaseDirectory;
                process.StartInfo.FileName = "bookgen.exe";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                process.Start();
                string st = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                if (File.Exists(fn))
                    File.Delete(fn);

                Output = st;
                return true;
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Output = string.Empty;
            return false;
        }

        private void RefreshPreviewIfNeeded()
        {
            if (TabIndex == 1
                && TryExport(View.Document.Text, false, true, out string html))
            {
                PreviewHtml = html;
                RaisePropertyChanged(nameof(PreviewHtml));
            }
        }

        private void OnSaveAs(object? obj)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Markdown (*.md)|*.md",
                Title = "Save file..."
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(sfd.FileName, View.Document.Text);
                    FileName = sfd.FileName;
                    IsDirty = false;
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnSave(object? obj)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                OnSaveAs(obj);
            }
            else
            {
                try
                {
                    File.WriteAllText(FileName, View.Document.Text);
                    IsDirty = false;
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnOpen(object? obj)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Markdown (*.md)|*.md",
                Title = "Open file..."
            };
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    View.Document.Text = File.ReadAllText(ofd.FileName);
                    FileName = ofd.FileName;
                    IsDirty = false;
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OnExportClipboard(object? obj)
        {
            if (TryExport(View.Document.Text, ExportRaw, ExportSyntaxHighlight, out string text))
            {
                Clipboard.SetText(text);
                MessageBoxEx.Show("Export successfull", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnExportFile(object? obj)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Filter = "Markdown (*.md)|*.md",
                Title = "Save file..."
            };

            if (saveFileDialog.ShowDialog() == true
                && TryExport(View.Document.Text, ExportRaw, ExportSyntaxHighlight, out string text))
            {
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, text);
                    MessageBoxEx.Show("Export successfull", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (IOException)
                {
                    MessageBoxEx.Show("File write failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

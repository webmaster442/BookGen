//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Wpf;
using BookGen.Launch.Code;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Win32;
using System;
using System.ComponentModel;
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

        public ICommand ExportFile { get; }
        public ICommand ExportClipboard { get; }
        public ICommand Open { get; }
        public ICommand Save { get; }
        public ICommand SaveAs { get; }

        public EditorViewModel()
        {
            ExportFile = new DelegateCommand(OnExportFile);
            ExportClipboard = new DelegateCommand(OnExportClipboard);
            Open = new DelegateCommand(OnOpen);
            Save = new DelegateCommand(OnSave);
            SaveAs = new DelegateCommand(OnSaveAs);
            _fileName = string.Empty;
        }

        private void OnSaveAs(object? obj)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "Markdown (*.md)|*.md",
                Title = "Save file..."
            };
            if (sfd.ShowDialog() == true
                && obj is IDocument doc)
            {
                try
                {
                    File.WriteAllText(sfd.FileName, doc.Text);
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
                    if (obj is IDocument doc)
                    {
                        File.WriteAllText(FileName, doc.Text);
                        IsDirty = false;
                    }
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
            if (ofd.ShowDialog() == true
                && obj is IDocument doc)
            {
                try
                {
                    doc.Text = File.ReadAllText(ofd.FileName);
                    FileName = ofd.FileName;
                    IsDirty = false;
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool TryExport(out string Output)
        {
            try
            {
                if (TryGetArguments(out string arguments))
                {
                    Process process = new Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.WorkingDirectory = AppContext.BaseDirectory;
                    process.StartInfo.FileName = "bookgen.exe";
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
                    process.Start();
                    string st = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    Output =  st;
                    return true;
                }
            }
            catch (Win32Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Output = string.Empty;
            return false;
        }

        private bool TryGetArguments(out string arguments)
        {
            if (string.IsNullOrEmpty(FileName))
            {
                MessageBoxEx.Show("Please save the file first", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                arguments = string.Empty;
                return false;
            }

            StringBuilder sb = new();
            sb.Append($"md2html -i \"{FileName}\" -o con");
            if (ExportRaw)
                sb.Append(" -r");
            if (!ExportSyntaxHighlight)
                sb.Append(" -ns");
            arguments = sb.ToString();
            return true;
        }

        private void OnExportClipboard(object? obj)
        {
            if (TryExport(out string text))
            {
                Clipboard.SetText(text);
                MessageBoxEx.Show("Export successfull", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnExportFile(object? obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "Markdown (*.md)|*.md",
                Title = "Save file..."
            };

            if (saveFileDialog.ShowDialog() == true
                && TryExport(out string text))
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

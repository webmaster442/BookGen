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
                    File.WriteAllText(FileName, string.Empty);
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
            if (ofd.ShowDialog() == true
                && obj is IDocument doc)
            {
                try
                {
                    doc.Text = File.ReadAllText(ofd.FileName);
                    IsDirty = false;
                }
                catch (Exception ex)
                {
                    MessageBoxEx.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private string Export()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.FileName = "bookgen.exe";
                process.StartInfo.Arguments = GetArguments();
                process.Start();
                return process.StandardOutput.ReadToEnd();
            }
            catch (Win32Exception ex)
            {
                MessageBoxEx.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        private string GetArguments()
        {
            StringBuilder sb = new();
            sb.Append($"-i {FileName} -o con");
            if (ExportRaw)
                sb.Append(" -r");
            if (!ExportSyntaxHighlight)
                sb.Append(" -ns");
            return sb.ToString();
        }

        private void OnExportClipboard(object? obj)
        {
            var text = Export();
            Clipboard.SetText(text);
            MessageBoxEx.Show("Export successfull", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnExportFile(object? obj)
        {
            var text = Export();

        }
    }
}

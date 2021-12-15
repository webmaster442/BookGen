//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Wpf;
using System;
using System.Windows.Input;

namespace BookGen.Launch
{
    internal class EditorViewModel : ViewModelBase
    {
        private bool _exportRaw;
        private bool _exportSyntaxHighlight;

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
        }

        private void OnSaveAs(object? obj)
        {
            throw new NotImplementedException();
        }

        private void OnSave(object? obj)
        {
            throw new NotImplementedException();
        }

        private void OnOpen(object? obj)
        {
        }

        private void OnExportClipboard(object? obj)
        {
            throw new NotImplementedException();
        }

        private void OnExportFile(object? obj)
        {
            throw new NotImplementedException();
        }
    }
}

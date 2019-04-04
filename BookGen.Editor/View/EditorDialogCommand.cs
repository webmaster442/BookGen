//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Windows.Input;

namespace BookGen.Editor.View
{
    internal enum DialogType
    {
        InsertPicture,
        InsertLink
    }

    internal class EditorDialogCommand : ICommand
    {
        private readonly EditorWrapper _editor;
        private readonly DialogType _dialogtype;

        public EditorDialogCommand(EditorWrapper editor, DialogType dialogtype)
        {
            _editor = editor;
            _dialogtype = dialogtype;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            switch (_dialogtype)
            {
                case DialogType.InsertPicture:
                    var InsertPic = new Dialogs.InsertPictureDialog();
                    if (InsertPic.ShowDialog() == true)
                    {
                    }
                    break;
            }
        }
    }
}

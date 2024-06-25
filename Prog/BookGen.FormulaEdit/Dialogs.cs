//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using Microsoft.Win32;

namespace BookGen.FormulaEdit;

internal sealed class Dialogs : IDialogs
{
    private readonly MetroWindow _metroWindow;
    private readonly MetroDialogSettings _dialogsettings;

    public Dialogs(MetroWindow metroWindow)
    {
        _metroWindow = metroWindow;
        _dialogsettings = new MetroDialogSettings
        {
            ColorScheme = MetroDialogColorScheme.Inverted,
            DefaultButtonFocus = MessageDialogResult.Affirmative,
        };
    }

    public async Task<bool> Confirm(string message)
    {
        var resullt = await _metroWindow.ShowMessageAsync("Confirm", message, MessageDialogStyle.AffirmativeAndNegative, _dialogsettings);
        return resullt == MessageDialogResult.Affirmative;
    }

    public async Task Error(Exception ex)
    {
        await _metroWindow.ShowMessageAsync("Error", ex.Message, MessageDialogStyle.Affirmative, _dialogsettings);
    }

    public string? OpenFile()
    {
        var ofd = new OpenFileDialog
        {
            Filter = "Formulas|*.formulas",
            Title = "Open formulas file",
            CheckFileExists = true,
            CheckPathExists = true
        };
        if (ofd.ShowDialog() == true)
        {
            return ofd.FileName;
        }
        return null;
    }

    public string? SaveFile(string extension)
    {
        static string GetFilter(string extension)
            => $"{extension}|*.{extension}";

        var sfd = new SaveFileDialog
        {
            Filter = GetFilter(extension),
            Title = "Save formulas file",
            OverwritePrompt = true,
        };

        if (sfd.ShowDialog() == true)
        {
            return sfd.FileName;
        }
        return null;
    }

    public async Task<(string baseName, string folder)?> ExportDialog()
    {
        var dialog = new ExportDialog(_metroWindow);
        await _metroWindow.ShowMetroDialogAsync(dialog);
        return dialog.DialogResult ? (dialog.TbBaseName.Text, dialog.TbFolderPath.Text) : null;
    }
}

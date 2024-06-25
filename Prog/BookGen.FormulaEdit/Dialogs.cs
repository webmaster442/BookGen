using System;
using System.Windows;

using Microsoft.Win32;

namespace BookGen.FormulaEdit;

internal sealed class Dialogs : IDialogs
{
    public bool Confirm(string message)
    {
        return MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
    }

    public void Error(Exception ex)
    {
        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
}

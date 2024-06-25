using System;
using System.Threading.Tasks;

namespace BookGen.FormulaEdit;

internal interface IDialogs
{
    string? OpenFile();
    string? SaveFile(string extension);
    Task Error(Exception ex);
    Task<bool> Confirm(string message);
    Task<(string baseName, string folder)?> ExportDialog();
}

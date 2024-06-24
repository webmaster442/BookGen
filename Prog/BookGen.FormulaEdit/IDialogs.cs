using System;
using System.Windows;

namespace BookGen.FormulaEdit;

internal interface IDialogs
{
    string? OpenFile();
    string? SaveFile();
    void Error(Exception ex);
    bool Confirm(string message);
}

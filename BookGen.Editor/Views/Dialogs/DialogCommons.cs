//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Windows.Input;

namespace BookGen.Editor.Views.Dialogs
{
    internal static class DialogCommons
    {
        internal static bool HandleCloseButtons(KeyEventArgs e)
        {
            if (e.Key == Key.Return
                && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                e.Handled = true;
                return true;
            }
            else if (e.Key == Key.Escape)
            {
                
                e.Handled = true;
                return false;
            }
            else
            {
                e.Handled = false;
                return false;
            }
        }
    }
}

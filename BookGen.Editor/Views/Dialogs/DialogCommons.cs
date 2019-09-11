//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookGen.Editor.Views.Dialogs
{
    internal static class DialogCommons
    {
        public static async Task<bool> ShowMessage(string title, string content, bool cancelable = false)
        {
            bool result = false;
            if (App.Current.MainWindow is MetroWindow mw)
            {
                var dialogResult = await mw.ShowMessageAsync(title, content, cancelable ? MessageDialogStyle.AffirmativeAndNegative : MessageDialogStyle.Affirmative).ConfigureAwait(false);
                return dialogResult == MessageDialogResult.Affirmative;
            }
            return result;
        }

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

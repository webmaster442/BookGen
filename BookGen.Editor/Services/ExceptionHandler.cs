//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;

namespace BookGen.Editor.Services
{
    internal static class ExceptionHandler
    {
        public static void HandleException(Exception ex)
        {
#if DEBUG
            Debugger.Break();
#endif
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void SafeRun(Action action)
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
    }
}

//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.ServiceContracts;
using System;
using System.Diagnostics;
using System.Windows;

namespace BookGen.Editor.Services
{
    internal class ExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception ex)
        {
#if DEBUG
            Debugger.Break();
#endif
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void SafeRun(Action action)
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

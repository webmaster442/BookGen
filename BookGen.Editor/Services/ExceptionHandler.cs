//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.ServiceContracts;
using BookGen.Editor.Views.Dialogs;
using System;
using System.Diagnostics;

namespace BookGen.Editor.Services
{
    internal class ExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception ex)
        {
#if DEBUG
            Debugger.Break();
#endif
            DialogCommons.ShowMessage("Error", ex.Message, false);
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

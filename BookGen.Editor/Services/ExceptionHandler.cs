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
            // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CS4014
            DialogCommons.ShowMessage("Error", ex.Message, false);
#pragma warning restore CS4014
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

//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen.Editor.ServiceContracts
{
    internal interface IExceptionHandler
    {
        void HandleException(Exception ex);
        void SafeRun(Action action);
    }
}
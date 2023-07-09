//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Bookgen.Win
{
    public static class ExceptionHandler
    {
        public static void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex) 
            {
#if DEBUG
                Debug.WriteLine(ex);
#endif
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

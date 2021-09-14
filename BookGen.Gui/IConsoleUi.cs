//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui
{
    public interface IConsoleUi
    {
        /// <summary>
        /// Suspend UI Execution
        /// </summary>
        void SuspendUi();
        /// <summary>
        /// Resume the UI Execution
        /// </summary>
        void ResumeUi();
        /// <summary>
        /// Exit application
        /// </summary>
        void ExitApp();
        /// <summary>
        /// Refresh the current view. Pulls data from Controller
        /// </summary>
        void RefreshCurrentView();
    }
}

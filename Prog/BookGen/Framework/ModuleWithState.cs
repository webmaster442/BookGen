//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO;

namespace BookGen.Framework
{
    internal abstract class ModuleWithState : ModuleBase
    {
        protected ProgramState CurrentState { get; }

        protected ModuleWithState(ProgramState currentState)
        {
            CurrentState = currentState;
        }

        public virtual void Abort()
        {
            //empty behaviour by default
        }

        public bool ShouldSkipLockCheck { get; set; }

        protected void CheckLockFileExistsAndExitWhenNeeded(string folder)
        {
            if (ShouldSkipLockCheck) return;

            if (FolderLock.TryCheckLockExistance(folder, out string lockfile))
            {
#if DEBUG
                System.Diagnostics.Process[]? running = System.Diagnostics.Process.GetProcessesByName(nameof(BookGen));
                if (running.Length == 1) //only this current instance is running
                {
                    CurrentState.Log.Warning("Lockfile was found, but no other bookgen is running. Removing lock...");
                    File.Delete(lockfile);
                    return;
                }
#endif
                CurrentState.Log.Critical("An other bookgen process is using this folder. Exiting...");
                Environment.Exit((int)ExitCode.FolderLocked);
            }
        }
    }
}

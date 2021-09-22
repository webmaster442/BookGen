//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Framework;
using BookGen.Utilities;
using System.Runtime.InteropServices;

namespace BookGen.Modules
{
    internal class UpdateModule : ModuleWithState
    {
        public UpdateModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Update";

        public override bool Execute(string[] arguments)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                CurrentState.Log.Warning("Auto update feature only available on windows");
                return false;
            }

            var updater = new Updater(CurrentState.Log,
                                      CurrentState.BuildDate,
                                      CurrentState.ProgramDirectory);

            var release = updater.GetLatestRelease();

            CurrentState.Log.Info("Current version: {0}", CurrentState.BuildDate);
            CurrentState.Log.Info("Update version: {0}", release?.Version ?? "unknown");

            if (release !=null 
                && updater.IsUpdateNewerThanCurrentVersion(release))
            {
                CurrentState.Log.Info("Preparing to launch update script...");
                updater.LaunchUpdateScript(release);
            }
            else
            {
                CurrentState.Log.Info("Allready up to date");
            }

            return true;
        }

        public override string GetHelp()
        {
            return "Upde program to latest stable release";
        }
    }
}

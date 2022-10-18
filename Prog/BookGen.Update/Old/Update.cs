//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

/*
using BookGen.Framework;

namespace BookGen.Modules
{
    internal class UpdateModule : ModuleWithState
    {
        public UpdateModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Update";

        public override SupportedOs SupportedOs => SupportedOs.Windows;

        public override ModuleRunResult Execute(string[] arguments)
        {
            var updater = new Updater(CurrentState.Log,
                                      CurrentState.BuildDateUtc,
                                      CurrentState.ProgramDirectory);

            Release? release = updater.GetLatestRelease();

            CurrentState.Log.Info("Current version: {0}", CurrentState.BuildDateUtc);
            CurrentState.Log.Info("Update version: {0}", release?.Version ?? "unknown");

            if (release != null
                && updater.IsUpdateNewerThanCurrentVersion(release))
            {
                CurrentState.Log.Info("Preparing to launch update script...");
                updater.LaunchUpdateScript(release);
            }
            else
            {
                CurrentState.Log.Info("Allready up to date");
            }

            return ModuleRunResult.Succes;
        }

        public override string GetHelp()
        {
            return "Upde program to latest stable release";
        }
    }
}
*/
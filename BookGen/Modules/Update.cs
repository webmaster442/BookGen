//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Utilities;

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
            var updater = new Updater(CurrentState.Log,
                                      CurrentState.BuildDate,
                                      CurrentState.ProgramDirectory);

            var updateVersion = updater.GetLatestVersion();

            CurrentState.Log.Info("Current version: {0}", CurrentState.BuildDate);
            CurrentState.Log.Info("Update version: {0}", updateVersion?.ToString() ?? "unknown");

            if (updater.IsUpdateNewerThanCurrentVersion(updateVersion))
            {
                CurrentState.Log.Info("Preparing to launch update script...");
                updater.LaunchUpdateScript();
            }

            return true;
        }
    }
}

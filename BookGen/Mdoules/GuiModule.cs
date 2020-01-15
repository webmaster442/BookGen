//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Gui;

namespace BookGen.Mdoules
{
    internal class GuiModule : ModuleBase
    {
        private ConsoleMenu? _ui;

        public GuiModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Gui";

        private GuiParameters GetGuiParameters(ArgumentParser arguments)
        {
            var guiParams = new GuiParameters
            {
                Verbose = arguments.GetSwitch("v", "verbose")
            };

            var dir = arguments.GetSwitchWithValue("d", "dir");

            if (!string.IsNullOrEmpty(dir))
                guiParams.WorkDir = dir;

            return guiParams;
        }

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            var parameters = GetGuiParameters(tokenizedArguments);

            CurrentState.Gui = true;
            CurrentState.GeneratorRunner = Program.CreateRunner(parameters.Verbose, parameters.WorkDir);
            _ui = new ConsoleMenu(CurrentState.GeneratorRunner);
            _ui.Run();

            return true;
        }

        public override void Abort()
        {
            if (_ui != null) _ui.ShouldRun = false;
        }
    }
}

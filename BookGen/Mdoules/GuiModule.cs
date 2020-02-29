//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.ConsoleUi;
using BookGen.Core;
using BookGen.Domain.ArgumentParsing;

namespace BookGen.Mdoules
{
    internal class GuiModule : ModuleBase
    {
        private readonly Gui.ConsoleUi uiRunner;

        public GuiModule(ProgramState currentState) : base(currentState)
        {
            uiRunner = new Gui.ConsoleUi();
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

            System.IO.Stream? Ui = typeof(GuiModule).Assembly.GetManifestResourceStream("BookGen.ConsoleUi.MainView.xml");
            var vm = new MainViewModel(CurrentState.GeneratorRunner);

            if (Ui != null)
            {
                uiRunner.Run(Ui, vm);
                return true;
            }
            return false;
        }

        public override void Abort()
        {
            if (uiRunner != null)
                uiRunner.SuspendUi();
        }
    }
}

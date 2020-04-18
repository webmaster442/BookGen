//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Utilities;
using System;

namespace BookGen.Mdoules
{
    internal class EditorModule : ModuleBase
    {
        public EditorModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Editor";

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            string workdir = Environment.CurrentDirectory;

            var dir = tokenizedArguments.GetSwitchWithValue("d", "dir");

            if (!string.IsNullOrEmpty(dir))
                workdir = dir;

            GeneratorRunner runner = Program.CreateRunner(false, workdir);

            if (runner.Initialize(false))
            {
                runner.DoEditor();
                return true;
            }

            return false;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(EditorModule));
        }
    }
}

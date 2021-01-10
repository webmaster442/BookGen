//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System;

namespace BookGen.Modules
{
    internal class EditorModule : ModuleWithState
    {
        public EditorModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Editor";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-d",
                                            "--dir",
                                             "-v", 
                                             "--verbose");
            }
        }

        public override bool Execute(string[] arguments)
        {
            BookGenArgumentBase args = new BookGenArgumentBase();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            GeneratorRunner runner = Program.CreateRunner(args.Verbose, args.Directory);

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

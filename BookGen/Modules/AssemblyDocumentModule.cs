//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;

namespace BookGen.Modules
{
    internal class AssemblyDocumentModule : StateModuleBase
    {
        public AssemblyDocumentModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "AssemblyDocument";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem("AssemblyDocument",
                                            "-a",
                                            "--assembly",
                                            "-x",
                                            "--xml",
                                            "-o",
                                            "--output");
            }
        }

        public override bool Execute(string[] arguments)
        {
            var parameters = new AssemblyDocumentParameters();

            if (!ArgumentParser.ParseArguments(arguments, parameters))
            {
                return false;
            }

            var documenter = new AssemblyDocumenter.AssemblyDocumenter(CurrentState.Log);

            documenter.Document(parameters.AssemblyPath, parameters.XmlPath, parameters.OutputDirectory);

            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(AssemblyDocumentModule));
        }
    }
}

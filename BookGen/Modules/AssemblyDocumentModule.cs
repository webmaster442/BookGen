//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System.Net.Http.Headers;

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

            ArgumentParser argumentParser = new ArgumentParser();
            if (!argumentParser.ParseArguments(arguments, parameters))
            {
                return false;
            }

            ILog log = new ConsoleLog();

            var documenter = new AssemblyDocumenter.AssemblyDocumenter(log);

            documenter.Document(parameters.AssemblyPath, parameters.XmlPath, parameters.OutputDirectory);

            return true;
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(AssemblyDocumentModule));
        }
    }
}

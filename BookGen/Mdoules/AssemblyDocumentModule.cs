//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Utilities;

namespace BookGen.Mdoules
{
    internal class AssemblyDocumentModule : ModuleBase
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

        private bool GetParameters(ArgumentParser arguments, out AssemblyDocumentParameters parameters)
        {
            parameters = new AssemblyDocumentParameters();
            parameters.AssemblyPath = new FsPath(arguments.GetSwitchWithValue("-a", "--assembly"));
            parameters.XmlPath = new FsPath(arguments.GetSwitchWithValue("-x", "--xml"));
            parameters.OutputDirectory = new FsPath(arguments.GetSwitchWithValue("-o", "--output"));

            return
                parameters.AssemblyPath.IsExisting
                && parameters.XmlPath.IsExisting
                && !FsPath.IsEmptyPath(parameters.OutputDirectory);
        }

        public override bool Execute(ArgumentParser tokenizedArguments)
        {
            if (!GetParameters(tokenizedArguments, out AssemblyDocumentParameters parameters))
                return false;

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

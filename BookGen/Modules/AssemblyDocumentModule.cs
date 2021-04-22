//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Documenter;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Ui.ArgumentParser;

namespace BookGen.Modules
{
    internal class AssemblyDocumentModule : ModuleWithState
    {
        public AssemblyDocumentModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "AssemblyDocument";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
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

            FolderLock.ExitIfFolderIsLocked(parameters.OutputDirectory.ToString(), CurrentState.Log);

            using (var l = new FolderLock(parameters.OutputDirectory.ToString()))
            {
                var documenter = new AssemblyDocumenter(CurrentState.Log);

                documenter.Document(parameters.AssemblyPath, parameters.XmlPath, parameters.OutputDirectory);
            }

            return true;
        }
    }
}

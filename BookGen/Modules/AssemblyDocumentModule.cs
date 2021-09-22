//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Documenter;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using System.Collections.ObjectModel;
using XmlDocMarkdown.Core;

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
                                            "-o",
                                            "--output",
                                            "-s",
                                            "--singlepage");
            }
        }

        public override bool Execute(string[] arguments)
        {
            var parameters = new AssemblyDocumentArguments();

            if (!ArgumentParser.ParseArguments(arguments, parameters))
            {
                return false;
            }

            FolderLock.ExitIfFolderIsLocked(parameters.OutputDirectory.ToString(), CurrentState.Log);

            using (var l = new FolderLock(parameters.OutputDirectory.ToString()))
            {
                if (parameters.SinglePage)
                {
                    var filename = parameters.OutputDirectory
                        .Combine(parameters.AssemblyPath.Filename)
                        .Combine("_doc.md");
                    
                    var documenter = new SinglePageAssemblyDocumenter(CurrentState.Log);

                    documenter.Document(parameters.AssemblyPath, filename);
                }
                else
                {
                    var result = XmlDocMarkdownGenerator.Generate(parameters.AssemblyPath.ToString(),
                                                 parameters.OutputDirectory.ToString(),
                                                 new XmlDocMarkdownSettings
                                                 {
                                                     IsQuiet = true,
                                                     ShouldClean = true,
                                                     VisibilityLevel = XmlDocVisibilityLevel.Protected,
                                                     SkipUnbrowsable = true,
                                                 });
                    Logdetails(result.Messages);
                }
            }
            return true;
        }

        private void Logdetails(Collection<string> messages)
        {
            foreach (var msg in messages)
            {
                CurrentState.Log.Detail(msg);
            }
        }
    }
}

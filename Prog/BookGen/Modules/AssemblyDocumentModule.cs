//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.AssemblyDocumenter;
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

        public override ModuleRunResult Execute(string[] arguments)
        {
            var parameters = new AssemblyDocumentArguments();

            if (!ArgumentParser.ParseArguments(arguments, parameters))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CheckLockFileExistsAndExitWhenNeeded(parameters.OutputDirectory.ToString());

            using (var l = new FolderLock(parameters.OutputDirectory.ToString()))
            {
                if (parameters.SinglePage)
                {
                    Interfaces.FsPath? filename = parameters.OutputDirectory
                        .Combine(parameters.AssemblyPath.Filename)
                        .Combine("_doc.md");

                    Interfaces.FsPath? xmlfile = parameters.AssemblyPath.ChangeExtension(".xml");

                    if (XmlDocValidator.ValidateXml(xmlfile, CurrentState.Log))
                    {

                        var documenter = new XmlDocumenter(xmlfile);

                        string? result = documenter.ToMarkdown();

                        filename.WriteFile(CurrentState.Log, result);
                        return ModuleRunResult.Succes;
                    }
                    return ModuleRunResult.GeneralError;
                }
                else
                {
                    XmlDocMarkdownResult? result = XmlDocMarkdownGenerator.Generate(parameters.AssemblyPath.ToString(),
                                                 parameters.OutputDirectory.ToString(),
                                                 new XmlDocMarkdownSettings
                                                 {
                                                     IsQuiet = true,
                                                     ShouldClean = true,
                                                     VisibilityLevel = XmlDocVisibilityLevel.Protected,
                                                     SkipUnbrowsable = true,
                                                 });
                    Logdetails(result.Messages);
                    return ModuleRunResult.Succes;
                }
            }
        }

        private void Logdetails(Collection<string> messages)
        {
            foreach (string? msg in messages)
            {
                CurrentState.Log.Detail(msg);
            }
        }
    }
}

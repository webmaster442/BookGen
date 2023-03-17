﻿using BookGen.AssemblyDocumenter;
using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.CommandArguments;
using BookGen.Framework;
using BookGen.Infrastructure;
using System.Collections.ObjectModel;
using XmlDocMarkdown.Core;

namespace BookGen.Commands
{
    [CommandName("assemblydocument")]
    internal class AssemblyDocumentCommand : Command<AssemblyDocumentArguments>
    {
        private readonly ILog _log;

        public AssemblyDocumentCommand(ILog log)
        {
            _log = log;
        }

        private void Logdetails(Collection<string> messages)
        {
            foreach (string? msg in messages)
            {
                _log.Detail(msg);
            }
        }

        public override int Execute(AssemblyDocumentArguments arguments, string[] context)
        {
            _log.CheckLockFileExistsAndExitWhenNeeded(arguments.OutputDirectory.ToString());

            using (var l = new FolderLock(arguments.OutputDirectory.ToString()))
            {
                if (arguments.SinglePage)
                {
                    Interfaces.FsPath? filename = arguments.OutputDirectory
                        .Combine(arguments.AssemblyPath.Filename)
                        .Combine("_doc.md");

                    Interfaces.FsPath? xmlfile = arguments.AssemblyPath.ChangeExtension(".xml");

                    if (XmlDocValidator.ValidateXml(xmlfile, _log))
                    {

                        var documenter = new XmlDocumenter(xmlfile);

                        string? result = documenter.ToMarkdown();

                        filename.WriteFile(_log, result);
                        return Constants.Succes;
                    }
                    return Constants.GeneralError;
                }
                else
                {
                    XmlDocMarkdownResult? result = XmlDocMarkdownGenerator.Generate(arguments.AssemblyPath.ToString(),
                                                 arguments.OutputDirectory.ToString(),
                                                 new XmlDocMarkdownSettings
                                                 {
                                                     IsQuiet = true,
                                                     ShouldClean = true,
                                                     VisibilityLevel = XmlDocVisibilityLevel.Protected,
                                                     SkipUnbrowsable = true,
                                                 });
                    Logdetails(result.Messages);
                    return Constants.Succes;
                }
            }

        }
    }
}

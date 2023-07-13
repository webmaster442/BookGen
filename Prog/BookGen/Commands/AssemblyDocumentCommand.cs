//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.ObjectModel;

using BookGen.AssemblyDocumenter;
using BookGen.CommandArguments;
using BookGen.Infrastructure;

using XmlDocMarkdown.Core;

namespace BookGen.Commands;

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

        if (arguments.SinglePage)
        {
            Interfaces.FsPath? filename = arguments.OutputDirectory
                .Combine(arguments.AssemblyPath.Filename)
                .Combine("_doc.md");

            Interfaces.FsPath? xmlfile = arguments.AssemblyPath.ChangeExtension(".xml");

            if (XmlDocValidator.ValidateXml(xmlfile, _log))
            {

                var documenter = new XmlDocumenter(xmlfile, arguments.AssemblyPath);

                string? result = documenter.ToMarkdown(new ConverterSettings
                {
                    ShouldSkipInternal = true,
                    ShouldSkipNonBrowsable = true,
                });

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

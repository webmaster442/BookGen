//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using DocumentFormat.OpenXml.Wordprocessing;

using HtmlToOpenXml;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("html2openxml")]
internal sealed class Html2OpenXmlCommand : AsyncCommand<Html2OpenXmlCommand.Html2OpenXmlArguments>
{
    private readonly ILogger _logger;
    private readonly IAssetSource _assetSource;
    private readonly IWritableFileSystem _fileSystem;

    internal sealed class Html2OpenXmlArguments : ArgumentsBase
    {
        [Switch("i", "input", true)]
        public string InputFile { get; set; }

        [Switch("o", "output", true)]
        public string OutputFile { get; set; }

        public Html2OpenXmlArguments()
        {
            InputFile = string.Empty;
            OutputFile = string.Empty;
        }

        public override ValidationResult Validate(IValidationContext context)
        {
            if (!context.FileSystem.FileExists(InputFile))
                return ValidationResult.Error($"File doesn't exist: {InputFile}");

            if (string.IsNullOrEmpty(OutputFile))
                return ValidationResult.Error("Output file not specified");

            var extension = Path.GetExtension(InputFile).ToLower();

            if (extension != ".htm" && extension != ".html")
                return ValidationResult.Error("Input file isn't html");

            return ValidationResult.Ok();
        }

        public override void ModifyAfterValidation()
        {
            var extension = Path.GetExtension(InputFile).ToLower();

            if (extension != ".docx")
                OutputFile = Path.ChangeExtension(OutputFile, ".docx");
        }
    }

    public Html2OpenXmlCommand(ILogger logger, IAssetSource assetSource, IWritableFileSystem fileSystem)
    {
        _logger = logger;
        _assetSource = assetSource;
        _fileSystem = fileSystem;
    }


    public override async Task<int> ExecuteAsync(Html2OpenXmlArguments arguments, IReadOnlyList<string> context)
    {
        using Stream generated = _fileSystem.CreateWriteStream(arguments.OutputFile);
        using (Stream template = _assetSource.GetBinaryAssetStream(BundledAssets.WordTemplate))
        {
            await template.CopyToAsync(generated);
        }

        generated.Seek(0, SeekOrigin.Begin);
        using (WordprocessingDocument package = WordprocessingDocument.Create(generated, WordprocessingDocumentType.Document))
        {
            MainDocumentPart? mainPart = package.MainDocumentPart;
            if (mainPart == null)
            {
                mainPart = package.AddMainDocumentPart();
                new Document(new Body()).Save(mainPart);
            }

            HtmlConverter converter = new(mainPart: mainPart, webRequester: null);
            converter.RenderPreAsTable = true;

            string html = await _fileSystem.ReadAllTextAsync(arguments.InputFile);
            await converter.ParseBody(html);

            if (!AssertThatOpenXmlDocumentIsValid(package))
            {
                return ExitCodes.GeneralError;
            }
        }

        return ExitCodes.Success;
    }

    private bool AssertThatOpenXmlDocumentIsValid(WordprocessingDocument package)
    {
        var validator = new OpenXmlValidator(FileFormatVersions.Office2021);
        IEnumerable<ValidationErrorInfo> errors = validator.Validate(package);
        if (errors.Any())
        {
            foreach (var error in errors)
            {
                _logger.LogError("OpenXml validation error: {Description} at {Path}", error.Description, error.Path!.XPath);
            }
            return false;
        }
        return true;
    }
}

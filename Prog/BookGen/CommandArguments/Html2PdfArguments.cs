namespace BookGen.CommandArguments;
internal class Html2PdfArguments : ArgumentsBase
{
    [Switch("i", "input")]
    public FsPath InputFile { get; set; }

    [Switch("o", "output")]
    public FsPath OutputFile { get; set; }

    public Html2PdfArguments() 
    {
        InputFile = FsPath.Empty;
        OutputFile = FsPath.Empty;
    }

    public override ValidationResult Validate()
    {
        if (!InputFile.IsExisting)
            return ValidationResult.Error($"File doesn't exist: {InputFile}");

        if (InputFile.Extension != ".htm" && InputFile.Extension != ".html")
            return ValidationResult.Error("Input file isn't html");

        return ValidationResult.Ok();
    }

    public override void ModifyAfterValidation()
    {
        if (OutputFile.Extension != ".pdf")
            OutputFile = OutputFile.ChangeExtension(".pdf");
    }
}

//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class Html2PngArguments : ArgumentsBase
{
    [Switch("i", "input")]
    public FsPath InputFile { get; set; }

    [Switch("o", "output")]
    public FsPath OutputFile { get; set; }

    [Switch("w", "width")]
    public int Width { get; set; }

    [Switch("h", "height")]
    public int Height { get; set; }

    public Html2PngArguments()
    {
        InputFile = FsPath.Empty;
        OutputFile = FsPath.Empty;
        Width = 1920;
        Height = 1080;
    }

    public override ValidationResult Validate()
    {
        if (!InputFile.IsExisting)
            return ValidationResult.Error($"File doesn't exist: {InputFile}");

        if (InputFile.Extension != ".htm" && InputFile.Extension != ".html")
            return ValidationResult.Error("Input file isn't html");

        if (Width < 10)
            return ValidationResult.Error("Width must be at least 10px");

        if (Height < 10)
            return ValidationResult.Error("Height must be at least 10px");

        return ValidationResult.Ok();
    }

    public override void ModifyAfterValidation()
    {
        if (OutputFile.Extension != ".png")
            OutputFile = OutputFile.ChangeExtension(".png");
    }
}
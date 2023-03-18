namespace BookGen.CommandArguments;

internal sealed class ImgConvertArguments : ArgumentsBase
{
    [Switch("i", "input")]
    public FsPath Input { get; set; }

    [Switch("o", "output")]
    public FsPath Output { get; set; }

    [Switch("q", "quality")]
    public int Quality { get; set; }

    [Switch("w", "width")]
    public int? Width { get; set; }

    [Switch("h", "height")]
    public int? Height { get; set; }

    [Switch("f", "format")]
    public string Format { get; set; }

    public ImgConvertArguments()
    {
        Input = FsPath.Empty;
        Output = FsPath.Empty;
        Format = string.Empty;
        Quality = 90;
    }

    public override ValidationResult Validate()
    {
        ValidationResult result = new();

        if (FsPath.IsEmptyPath(Input))
            result.AddIssue("Input must be specified");

        if (FsPath.IsEmptyPath(Output))
            result.AddIssue("Output must be specified");

        if (string.IsNullOrEmpty(Format))
            result.AddIssue("format must be specified");

        if (Input.IsWildCard && !Output.IsDirectory)
            result.AddIssue("For multiple files output must be a directory");
        else if (!Input.IsExisting)
            result.AddIssue("Input file doesn't exist");

        if (Width.HasValue && Width.Value < 0)
            result.AddIssue("Width must be positive");

        if (Height.HasValue && Height.Value < 0)
            result.AddIssue("Height must be positive");

        return result;
    }
}

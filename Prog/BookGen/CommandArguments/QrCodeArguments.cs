namespace BookGen.CommandArguments;

internal sealed class QrCodeArguments : ArgumentsBase
{
    [Switch("o", "output")]
    public FsPath Output { get; set; }

    [Switch("s", "size")]
    public int Size { get; set; }

    [Switch("d", "data")]
    public string Data { get; set; }

    public QrCodeArguments()
    {
        Output = FsPath.Empty;
        Size = 200;
        Data = string.Empty;
    }

    public override ValidationResult Validate()
    {
        ValidationResult result = new();
        if (string.IsNullOrEmpty(Data))
            result.AddIssue("Data can't be empty");

        if (Data.Length < 1 && Data.Length > 900)
            result.AddIssue("Data must be at least 1 chars and max 900 chars");

        if (FsPath.IsEmptyPath(Output))
            result.AddIssue("Output can't be empty");

        if (Size < 10 || Size > 1000)
            result.AddIssue("Size must be bigger than 10px and maximum 1000 pixels");

        if (Output.Extension != ".png" || Output.Extension != ".svg")
            result.AddIssue("Output extension must be .png or .svg");

        return result;
    }
}

namespace BookGen.CommandArguments;
internal class DownloadArguments : BookGenArgumentBase
{
    [Argument(0)]
    public string Url { get; set; }

    public DownloadArguments() 
    {
        Url = string.Empty;
    }

    public override ValidationResult Validate()
    {
        return Uri.TryCreate(Url, UriKind.RelativeOrAbsolute, out _)
            ? ValidationResult.Ok()
            : ValidationResult.Error($"Given url is not a valid url: {Url}");
    }
}

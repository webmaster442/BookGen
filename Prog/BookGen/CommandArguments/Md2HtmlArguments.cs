//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class Md2HtmlArguments : ArgumentsBase
{
    [Switch("i", "input")]
    public FsPath[] InputFiles { get; set; }

    [Switch("o", "output")]
    public FsPath OutputFile { get; set; }

    [Switch("c", "css")]
    public FsPath Css { get; set; }

    [Switch("tf", "template")]
    public FsPath Template { get; set; }

    [Switch("ns", "no-syntax")]
    public bool NoSyntax { get; set; }

    [Switch("r", "raw")]
    public bool RawHtml { get; set; }

    [Switch("nc", "no-css")]
    public bool NoCss { get; set; }

    [Switch("s", "svg")]
    public bool SvgPassthrough { get; set; }

    [Switch("t", "title")]
    public string Title { get; set; }


    public Md2HtmlArguments()
    {
        Css = FsPath.Empty;
        Template = FsPath.Empty;
        Title = "Markdown document";
        InputFiles = [];
        OutputFile = FsPath.Empty;
    }

    public override ValidationResult Validate()
    {
        ValidationResult result = new();

        if (Template != FsPath.Empty)
        {
            if (!Template.IsExisting)
                result.AddIssue("Template file doesn't exist");
        }

        if (Css != FsPath.Empty)
        {
            if (!Css.IsExisting)
                result.AddIssue("css file doesn't exist");
        }

        if (FsPath.IsEmptyPath(OutputFile))
            result.AddIssue("Output file must be specified");

        if (!InputFiles.Any())
            result.AddIssue("An Input file must be specified");

        foreach (var inputfile in InputFiles)
        {
            if (!inputfile.IsExisting)
                result.AddIssue($"Input file: {inputfile} doesn't exist");
        }

        if (string.IsNullOrWhiteSpace(Title))
            result.AddIssue("Title can't be only whitespaces or empty");

        return result;
    }
}

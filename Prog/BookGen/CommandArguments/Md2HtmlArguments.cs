//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal sealed class Md2HtmlArguments : InputOutputArguments
{
    [Switch("c", "css")]
    public FsPath Css { get; set; }

    [Switch("ns", "no-syntax")]
    public bool NoSyntax { get; set; }

    [Switch("r", "raw")]
    public bool RawHtml { get; set; }

    [Switch("s", "svg")]
    public bool SvgPassthrough { get; set; }

    [Switch("t", "title")]
    public string Title { get; set; }


    public Md2HtmlArguments()
    {
        Css = FsPath.Empty;
        Title = "Markdown document";
    }

    public override ValidationResult Validate()
    {
        ValidationResult result = new();
        if (Css != FsPath.Empty)
        {
            if (!Css.IsExisting)
                result.AddIssue("css file doesn't exist");
        }

        if (!InputFile.IsExisting)
            result.AddIssue("Input file doesn't exist");

        if (string.IsNullOrWhiteSpace(Title))
            result.AddIssue("Title can't be only whitespaces or empty");

        return result;
    }
}

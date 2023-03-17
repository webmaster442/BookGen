using BookGen.Cli;
using BookGen.Interfaces;

namespace BookGen.CommandArguments
{
    internal sealed class Md2HtmlArguments : InputOutputArguments
    {
        [Switch("c", "css")]
        public FsPath Css { get; set; }

        [Switch("ns", "no-syntax")]
        public bool NoSyntax { get; set; }

        [Switch("r", "raw")]
        public bool RawHtml { get; set; }


        public Md2HtmlArguments()
        {
            Css = FsPath.Empty;
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

            return result;
        }
    }
}

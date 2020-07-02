//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Ui.ArgumentParser;

namespace BookGen.Domain.ArgumentParsing
{
    internal class Md2HtmlParameters : ArgumentsBase
    {
        [Switch("-i", "--input", true)]
        public FsPath InputFile { get; set; }

        [Switch("-o", "--output", true)]
        public FsPath OutputFile { get; set; }

        [Switch("-c", "--css")]
        public FsPath Css { get; set; }

        [Switch("-ns", "--no-syntax")]
        public bool NoSyntax { get; set; }

        [Switch("-r", "--raw")]
        public bool RawHtml { get; set; }


        public Md2HtmlParameters()
        {
            Css = FsPath.Empty;
            InputFile = FsPath.Empty;
            OutputFile = FsPath.Empty;
        }

        public override bool Validate()
        {
            if (Css != FsPath.Empty)
            {
                return
                    Css.IsExisting
                    && InputFile.IsExisting;
            }

            return
                InputFile.IsExisting;
        }
    }
}

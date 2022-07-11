//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.Domain.ArgumentParsing
{
    public sealed class Md2HtmlArguments : InputOutputArguments
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

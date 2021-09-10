//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Ui.ArgumentParser;

namespace BookGen.Domain.ArgumentParsing
{
    internal class ImgConvertArguments : ArgumentsBase
    {
        [Switch("i", "input", true)]
        public FsPath Input { get; set; }

        [Switch("o", "output", true)]
        public FsPath Output { get; set; }

        [Switch("q", "quality", false)]
        public int Quality { get; set; }

        [Switch("w", "width", false)]
        public int? Width { get; set; }

        [Switch("h", "height", false)]
        public int? Height { get; set; }

        public ImgConvertArguments()
        {
            Input = FsPath.Empty;
            Output = FsPath.Empty;
            Quality = 90;
        }

        public override bool Validate()
        {
            if (Input.IsWildCard())
            {
                return Output.IsDirectory;
            }
            return Input.IsExisting 
                && Output.IsExisting;
        }
    }
}

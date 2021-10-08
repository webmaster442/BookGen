//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Gui.ArgumentParser;

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

        [Switch("f", "format", false)]
        public string Format { get; set; }

        public ImgConvertArguments()
        {
            Input = FsPath.Empty;
            Output = FsPath.Empty;
            Format = string.Empty;
            Quality = 90;
        }

        public override bool Validate()
        {
            if (Input.IsWildCard())
            {

                return Output.IsDirectory
                    && !string.IsNullOrEmpty(Format);
            }
            return Input.IsExisting;
        }
    }
}

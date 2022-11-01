//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.Domain.ArgumentParsing
{
    public class QrCodeArguments : ArgumentsBase
    {
        [Switch("o", "output", true)]
        public FsPath Output { get; set; }

        [Switch("s", "size")]
        public int Size { get; set; }

        public QrCodeArguments()
        {
            Output = FsPath.Empty;
            Size = 200;
        }

        public override bool Validate()
        {
            return
                !FsPath.IsEmptyPath(Output)
                && Size > 10
                && Size < 1000
                && (Output.Extension == ".png" || Output.Extension == ".svg");
        }
    }
}

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

        [Switch("d", "data")]
        public string Data { get; set; }

        public QrCodeArguments()
        {
            Output = FsPath.Empty;
            Size = 200;
            Data = string.Empty;
        }

        public override bool Validate()
        {
            return
                !string.IsNullOrEmpty(Data)
                && Data.Length > 1
                && Data.Length < 900
                && !FsPath.IsEmptyPath(Output)
                && Size > 10
                && Size < 1000
                && (Output.Extension == ".png" || Output.Extension == ".svg");
        }
    }
}

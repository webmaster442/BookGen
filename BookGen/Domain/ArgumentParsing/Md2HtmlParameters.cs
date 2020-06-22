//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;

namespace BookGen.Domain.ArgumentParsing
{
    internal class Md2HtmlParameters
    {
        public FsPath InputFile { get; }
        public FsPath OutputFile { get; }
        public FsPath Css { get; }
        public bool NoSyntax { get; }
        public bool RawHtml { get; }

        public Md2HtmlParameters(string inputFile, string outputFile, string css, bool nosyntax, bool raw)
        {
            InputFile = new FsPath(inputFile);
            OutputFile = new FsPath(outputFile);
            Css = new FsPath(css);
            RawHtml = raw;
            NoSyntax = nosyntax;
        }
    }
}

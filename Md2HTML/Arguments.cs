using BookGen.Core;

namespace Md2HTML
{
    internal class Arguments
    {
        public FsPath InputFile { get; }
        public FsPath OutputFile { get; }
        public FsPath Css { get;}

        public Arguments(string inputFile, string outputFile, string css)
        {
            InputFile = new FsPath(inputFile);
            OutputFile = new FsPath(outputFile);
            Css = new FsPath(css);
        }
    }
}

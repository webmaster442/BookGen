//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Ui.ArgumentParser;
using BookGen.Utilities;
using System.Linq;
using System.Threading.Tasks;

namespace BookGen.Modules
{
    internal class ImgConvert : ModuleWithState
    {
        protected ImgConvert(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "ImgConvert";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-i",
                                            "--input",
                                            "-o",
                                            "--output",
                                            "-q",
                                            "--quality",
                                            "-f",
                                            "--format",
                                            "-w",
                                            "--width",
                                            "-h",
                                            "--height");
            }
        }


        public override bool Execute(string[] arguments)
        {
            var args = new ImgConvertArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return false;
            }

            if (args.Input.IsWildCard())
            {
                var files = args.Input.GetAllFiles(false).Where(x => ImageUtils.IsImage(x));
                Parallel.ForEach(files, file =>
                {
                    var output = args.Output.Combine(file.Filename);
                    ImageUtils.ConvertImageFile(CurrentState.Log, file, output, args.Quality, args.Width, args.Height, args.Format);
                });

                return true;
            }

            return ImageUtils.ConvertImageFile(CurrentState.Log, args.Input, args.Output, args.Quality, args.Width, args.Height);
        }
    }
}

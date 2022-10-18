//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Interfaces;

namespace BookGen.Modules
{
    internal class Math2SvgModule : ModuleWithState
    {
        public Math2SvgModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Math2Svg";

        public override ModuleRunResult Execute(string[] arguments)
        {
            var args = new InputOutputArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Log.LogLevel = Api.LogLevel.Info;

            IList<string>? input = args.InputFile.ReadFileLines(CurrentState.Log);

            Process(input, args.InputFile.Filename, args.OutputFile);

            return ModuleRunResult.Succes;
        }

        private void Process(IList<string> input, string filename, FsPath outDirectory)
        {
            using (var client = new BookGenHttpClient())
            {
                int counter = 0;
                foreach (string? line in input)
                {
                    if (!line.StartsWith("\\"))
                    {
                        CurrentState.Log.Warning("Not a formula (not starting with \\), Skipping line: {0}", line);
                        continue;
                    }

                    CurrentState.Log.Info("Downloading from https://math.vercel.app...");

                    var result = client.TryDownload($"https://math.vercel.app?from={line}",
                                                     out string? svg);

                    if (!string.IsNullOrEmpty(svg)
                        && (int)result >= 200 
                        && (int)result <= 300)
                    {
                        FsPath output = outDirectory.Combine(filename + $"-{counter}.svg");
                        output.WriteFile(CurrentState.Log, svg);
                        ++counter;
                    }
                    else
                    {
                        CurrentState.Log.Warning("Download failed. Error: {0}", result);
                    }
                }
            }
        }
    }
}

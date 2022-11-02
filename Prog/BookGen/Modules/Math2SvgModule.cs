//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.Shell;
using BookGen.DomainServices.WebServices;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Infrastructure;
using BookGen.Interfaces;
using System.Net;

namespace BookGen.Modules
{
    internal class Math2SvgModule : ModuleWithState, IAsyncModule
    {
        public Math2SvgModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "Math2Svg";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-i",
                                            "--input",
                                            "-o",
                                            "--output");
            }
        }

        public override ModuleRunResult Execute(string[] arguments)
        {
            return ModuleRunResult.AsyncModuleCalledInSyncMode;
        }

        public async Task<ModuleRunResult> ExecuteAsync(string[] arguments)
        {
            var args = new InputOutputArguments();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            CurrentState.Log.LogLevel = Api.LogLevel.Info;
            IList<string>? input = args.InputFile.ReadFileLines(CurrentState.Log);

            UrlParameterBuilder builder = new(MathVercelParams.ApiUrl);
            using (var client = new BookGenHttpClient())
            {
                for (int i=0; i<input.Count; i++)
                {
                    if (!input[i].StartsWith("\\"))
                    {
                        CurrentState.Log.Warning("Not a formula (not starting with \\), Skipping line: {0}", input[i]);
                        continue;
                    }
                    CurrentState.Log.Info("Downloading from {0}...", MathVercelParams.ApiUrl);

                    builder.AddParameter(MathVercelParams.FromPram, input[i]);
                    Uri uri = builder.Build();

                    (HttpStatusCode code, string resultString) = await client.TryDownload(uri);

                    if (BookGenHttpClient.IsSuccessfullRequest(code))
                    {
                        FsPath output = args.OutputFile.Combine(args.InputFile.Filename + $"-{i}.svg");
                        output.WriteFile(CurrentState.Log, resultString);
                    }
                    else
                    {
                        CurrentState.Log.Warning("Download failed. Error: {0}", code);
                        return ModuleRunResult.GeneralError;
                    }
                }
            }
            return ModuleRunResult.Succes;
        }
    }
}

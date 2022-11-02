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
using System.Net;

namespace BookGen.Modules
{
    internal class QrCodeModule : ModuleWithState, IAsyncModule
    {
        public QrCodeModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "QrCode";

        public override AutoCompleteItem AutoCompleteInfo
        {
            get
            {
                return new AutoCompleteItem(ModuleCommand,
                                            "-d",
                                            "--data",
                                            "-o",
                                            "--output",
                                            "-s",
                                            "--size");
            }
        }

        public override ModuleRunResult Execute(string[] arguments)
        {
            return ModuleRunResult.AsyncModuleCalledInSyncMode;
        }

        public async Task<ModuleRunResult> ExecuteAsync(string[] arguments)
        {
            QrCodeArguments args = new();
            if (!ArgumentParser.ParseArguments(arguments, args))
            {
                return ModuleRunResult.ArgumentsError;
            }

            UrlParameterBuilder builder = new UrlParameterBuilder(GoQrMeParams.ApiUrl);
            builder.AddParameter(GoQrMeParams.DataParam, args.Data);
            builder.AddParameter(GoQrMeParams.SizeParam, $"{args.Size}x{args.Size}");
            builder.AddParameter(GoQrMeParams.FormatParam, args.Output.Extension);

            var uri = builder.Build();

            using (var client = new BookGenHttpClient())
            {
                CurrentState.Log.Info("Downloading from {0}...", GoQrMeParams.ApiUrl);
                HttpStatusCode result = await client.DownloadToFile(uri, args.Output, CurrentState.Log);
               
                if (!BookGenHttpClient.IsSuccessfullRequest(result))
                {
                    CurrentState.Log.Warning("Download failed. Error: {0}", result);
                    return ModuleRunResult.GeneralError;
                }

                return ModuleRunResult.Succes;
            }
        }
    }
}

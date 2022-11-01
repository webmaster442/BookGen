using BookGen.Domain.ArgumentParsing;
using BookGen.DomainServices.WebServices;
using BookGen.Framework;
using BookGen.Gui.ArgumentParser;
using BookGen.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Modules
{
    internal class QrCodeModule : ModuleWithState, IAsyncModule
    {
        public QrCodeModule(ProgramState currentState) : base(currentState)
        {
        }

        public override string ModuleCommand => "QrCode";

        public override ModuleRunResult Execute(string[] arguments)
        {
            return ModuleRunResult.GeneralError;
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
                bool result = await client.DownloadToFile(uri, args.Output);
                return result ? ModuleRunResult.Succes : ModuleRunResult.GeneralError;
            }
        }
    }
}

using BookGen.Cli;
using BookGen.CommandArguments;
using BookGen.DomainServices.WebServices;
using BookGen.Interfaces;
using System.Net;

namespace BookGen.Commands
{
    internal class Math2SvgCommand : AsyncCommand<InputOutputArguments>
    {
        private readonly ILog _log;

        public Math2SvgCommand(ILog log) 
        {
            _log = log;
        }

        public override async Task<int> Execute(InputOutputArguments arguments, string[] context)
        {
            _log.LogLevel = Api.LogLevel.Info;
            IList<string>? input = arguments.InputFile.ReadFileLines(_log);

            UrlParameterBuilder builder = new(MathVercelParams.ApiUrl);
            using (var client = new BookGenHttpClient())
            {
                for (int i = 0; i < input.Count; i++)
                {
                    if (!input[i].StartsWith("\\"))
                    {
                        _log.Warning("Not a formula (not starting with \\), Skipping line: {0}", input[i]);
                        continue;
                    }
                    _log.Info("Downloading from {0}...", MathVercelParams.ApiUrl);

                    builder.AddParameter(MathVercelParams.FromPram, input[i]);
                    Uri uri = builder.Build();

                    (HttpStatusCode code, string resultString) = await client.TryDownload(uri);

                    if (BookGenHttpClient.IsSuccessfullRequest(code))
                    {
                        FsPath output = arguments.OutputFile.Combine(arguments.InputFile.Filename + $"-{i}.svg");
                        output.WriteFile(_log, resultString);
                    }
                    else
                    {
                        _log.Warning("Download failed. Error: {0}", code);
                        return Constants.GeneralError;
                    }
                }
            }
            return Constants.Succes;
        }
    }
}

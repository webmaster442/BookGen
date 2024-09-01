//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;

using BookGen.CommandArguments;
using BookGen.DomainServices.WebServices;

namespace BookGen.Commands;

[CommandName("math2svg")]
internal class Math2SvgCommand : AsyncCommand<InputOutputArguments>
{
    private readonly ILogger _log;

    public Math2SvgCommand(ILogger log)
    {
        _log = log;
    }

    public override async Task<int> Execute(InputOutputArguments arguments, string[] context)
    {
        IList<string>? input = arguments.InputFile.ReadFileLines(_log);

        UrlParameterBuilder builder = new(MathVercelParams.ApiUrl);
        using (var client = new BookGenHttpClient())
        {
            for (int i = 0; i < input.Count; i++)
            {
                if (!input[i].StartsWith("\\"))
                {
                    _log.LogWarning("Not a formula (not starting with \\), Skipping line: {line}", input[i]);
                    continue;
                }
                _log.LogInformation("Downloading from {url}...", MathVercelParams.ApiUrl);

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
                    _log.LogWarning("Download failed. Error: {error}", code);
                    return Constants.GeneralError;
                }
            }
        }
        return Constants.Succes;
    }
}

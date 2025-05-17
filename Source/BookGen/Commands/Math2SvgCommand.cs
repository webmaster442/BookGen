//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Web;
using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace BookGen.Commands;

[CommandName("math2svg")]
internal class Math2SvgCommand : AsyncCommand<InputOutputArguments>
{
    private readonly ILogger _log;
    private readonly IWritableFileSystem _fileSystem;

    public Math2SvgCommand(ILogger log, IWritableFileSystem fileSystem)
    {
        _log = log;
        _fileSystem = fileSystem;
    }

    public override async Task<int> Execute(InputOutputArguments arguments, string[] context)
    {
        var input = File.ReadAllLines(arguments.InputFile);

        UrlParameterBuilder builder = new(MathVercelParams.ApiUrl);
        using (var client = new BookGenHttpClient())
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!input[i].StartsWith('\\'))
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
                    var output = Path.Combine(arguments.OutputFile, Path.GetFileName(arguments.InputFile) + $"-{i}.svg");

                    _fileSystem.WriteAllText(output, resultString);
                }
                else
                {
                    _log.LogWarning("Download failed. Error: {error}", code);
                    return ExitCodes.GeneralError;
                }
            }
        }
        return ExitCodes.Succes;
    }
}

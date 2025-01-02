using BookGen.CommandArguments;

namespace BookGen.Commands;

[CommandName("minify")]
internal class MinifyCommand : Command<InputOutputArguments>
{
    private readonly ILogger _logger;

    public MinifyCommand(ILogger logger)
    {
        _logger = logger;
    }

    public override int Execute(InputOutputArguments arguments, string[] context)
    {
        if (Minify.TryMinify(arguments.InputFile, _logger, out string? result))
        {
            arguments.OutputFile.WriteFile(_logger, result);
            return Constants.Succes;
        }

        return Constants.GeneralError;
    }
}

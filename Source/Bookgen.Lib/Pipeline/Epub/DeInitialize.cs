using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class DeInitialize : PipeLineStep<EpubState>
{
    public DeInitialize(EpubState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        State.Deinitialize();

        logger.LogInformation("Rember to validate your generated e-book, with a tool such as: https://pagina.gmbh/startseite/leistungen/publishing-softwareloesungen/epub-checker/");

        return Task.FromResult(StepResult.Success);
    }
}
using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline;

public interface IPipeLineStep
{
    Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken);
}

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline;
public interface IPipeLineStep
{
    Task<StepResult> ExecuteAsync(IEnvironment environment, ILogger logger, CancellationToken cancellationToken);
}

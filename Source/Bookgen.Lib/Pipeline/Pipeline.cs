using System.Collections;

using Bookgen.Lib.Pipeline.StaticWebsite;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline;

public sealed class Pipeline : IEnumerable<IPipeLineStep>
{
    private readonly List<IPipeLineStep> _steps;

    public Pipeline()
    {
        _steps = new List<IPipeLineStep>();
    }

    public void Add(IPipeLineStep step)
        => _steps.Add(step);

    public IEnumerator<IPipeLineStep> GetEnumerator()
        => _steps.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _steps.GetEnumerator();

    public async Task<bool> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        foreach (var step in _steps)
        {
            var result = await step.ExecuteAsync(environment, logger, cancellationToken);
            if (result == StepResult.Failure)
            {
                return false;
            }
        }
        return true;
    }

    public static Pipeline CreateWebPipeLine()
    {
        return new Pipeline
        {
            new CopyAssets(),
            new ExtractTemplateAssets(),
            new CreateEmptyIndexPagesForFolders(),
        };
    }
}
using System.IO.Compression;
using System.Text;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class CreateMimeType : PipeLineStep<EpubState>
{
    public CreateMimeType(EpubState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating mimetype...");
        State.EpubFile.Add("mimetype", "application/epub+zip", Encoding.UTF8, CompressionLevel.NoCompression);
        return Task.FromResult(StepResult.Success);
    }
}

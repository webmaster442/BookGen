using System.Text;

using Bookgen.Lib.Domain.Epub;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class CreateEpubCoverAndStyle : PipeLineStep<EpubState>
{
    public CreateEpubCoverAndStyle(EpubState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var css = environment.GetAsset("bookgen.epub.min.css");
        State.EpubFile.Add("EPUB/content/bookgen.epub.min.css", css, Encoding.UTF8);
        State.PackageItems.Add(new PackageItem
        {
            Href = "content/bookgen.epub.min.css",
            Id = "id-css",
            Mediatype = "text/css",
        });

        return Task.FromResult(StepResult.Success);
    }
}

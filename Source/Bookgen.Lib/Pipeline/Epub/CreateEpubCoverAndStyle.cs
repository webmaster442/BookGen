using System.Text;

using Bookgen.Lib.Domain.Epub;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class CreateEpubCoverAndStyle : PipeLineStep<EpubState>
{
    public CreateEpubCoverAndStyle(EpubState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var coverfile = await environment.Source.GetCoverFileName(environment.TableOfContents, logger);
        if (coverfile != null)
        {
            byte[] coverdata = Utils.ConvertToPng(coverfile, 1200, 1200);
            State.EpubFile.Add("EPUB/cover.png", coverdata);
            State.PackageItems.Add(new PackageItem
            {
                Href = "cover.png",
                Id = "cover",
                Mediatype = "image/png",
                Properties = "cover-image"
            });
        }

        var css = environment.GetAsset("bookgen.epub.min.css");
        State.EpubFile.Add("EPUB/content/bookgen.epub.min.css", css, Encoding.UTF8);
        State.PackageItems.Add(new PackageItem
        {
            Href = "content/bookgen.epub.min.css",
            Id = "id-css",
            Mediatype = "text/css",
        });

        return StepResult.Success;
    }
}

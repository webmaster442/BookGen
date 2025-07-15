using Bookgen.Lib.Domain.Epub;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class CreateContentOpf : PipeLineStep<EpubState>
{
    public CreateContentOpf(EpubState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating EPUB content.opf...");
        var opf = new Package
        {
            Version = "3.0",
            Lang = "en-US",
            Uniqueidentifier = Guid.CreateVersion7().ToString(),
            Prefix = "ibooks: http://vocabulary.itunes.apple.com/rdf/ibooks/vocabulary-extensions-1.0/",
            Metadata = new PackageMetadata
            {
                Title = new Title
                {
                    Id = "epub-title",
                    Value = environment.Configuration.BookTitle
                },
                Date = new Date
                {
                    Id = "epub-date",
                    Value = DateTime.UtcNow
                },
                Identifier = new Identifier
                {
                    Id = "epub-id",
                    Value = $"urn:uuid:{Guid.CreateVersion7()}"
                },
                Language = "en-US",
                Meta =
                [
                    new PackageMetadataMeta
                    {
                        Property = "schema:accessMode",
                        Value = "textual"
                    }
                ]
            },
            Manifest = State.PackageItems,
            Spine = State.Spine,
        };

        State.EpubFile.AddXml("EPUB/content.opf",
                              opf,
                              ("", "http://www.idpf.org/2007/opf"),
                              ("dc", "http://purl.org/dc/elements/1.1/"));

        return Task.FromResult(StepResult.Success);
    }
}

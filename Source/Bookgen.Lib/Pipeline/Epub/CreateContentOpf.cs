using Bookgen.Lib.Domain.Epub;
using Bookgen.Lib.Internals;

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
        var uniqueId = $"id-{IdGenerator.Generate32BitDeterministicId(environment.Configuration.BookTitle)}";
        var opf = new Package
        {
            Version = "3.0",
            Lang = "en-US",
            Uniqueidentifier = uniqueId,
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
                    Id = uniqueId,
                    Value = $"urn:uuid:{State.BookId}"
                },
                Language = "en-US",
                Meta =
                [
                    new PackageMetadataMeta
                    {
                        Property = "schema:accessMode",
                        Value = "textual"
                    },
                    new PackageMetadataMeta
                    {
                        Property = "dcterms:modified",
                        Value = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                    },
                ]
            },
            Manifest = State.PackageItems,
            Spine = State.Spine,
            Guide = new PackageGuide
            {
                Reference = new PackageGuideReference
                {
                    Href = "content/nav.xhtml",
                    Title = environment.Configuration.BookTitle,
                    Type = "toc"
                }
            }
        };

        State.EpubFile.AddXml("EPUB/content.opf",
                              opf,
                              ("", "http://www.idpf.org/2007/opf"),
                              ("dc", "http://purl.org/dc/elements/1.1/"));

        return Task.FromResult(StepResult.Success);
    }
}

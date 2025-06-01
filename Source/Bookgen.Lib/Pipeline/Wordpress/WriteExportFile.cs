
using System.Xml.Serialization;

using Bookgen.Lib.Domain.Wordpress;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Wordpress;

internal sealed class WriteExportFile : PipeLineStep<WpState>
{
    public WriteExportFile(WpState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Writing wordpress xml export...");

        var output = new Rss()
        {
            Version = "2.0",
            Channel = State.CurrentChannel,
        };

        XmlSerializerNamespaces xnames = new();
        xnames.Add("excerpt", "http://wordpress.org/export/1.2/excerpt/");
        xnames.Add("content", "http://purl.org/rss/1.0/modules/content/");
        xnames.Add("wfw", "http://wellformedweb.org/CommentAPI/");
        xnames.Add("dc", "http://purl.org/dc/elements/1.1/");
        xnames.Add("wp", "http://wordpress.org/export/1.2/");

        var xs = new XmlSerializer(typeof(Rss));
        using var fileStream = environment.Output.CreateWriteStream("wodpress-export.xml");

        xs.Serialize(fileStream, output, xnames);

        return Task.FromResult(StepResult.Success);
    }
}

//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.Epub;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class CreateContainer : PipeLineStep<EpubState>
{
    public CreateContainer(EpubState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating META-INF/container.xml...");
        var container = new Container
        {
            Version = "1.0",
            Rootfiles = new ContainerRootfiles
            {
                Rootfile = new ContainerRootfilesRootfile
                {
                    Mediatype = "application/oebps-package+xml",
                    Fullpath = "EPUB/content.opf"
                }
            }
        };
        State.EpubFile.AddXml("META-INF/container.xml",
                              container,
                              ("", "urn:oasis:names:tc:opendocument:xmlns:container"));

        return Task.FromResult(StepResult.Success);
    }
}
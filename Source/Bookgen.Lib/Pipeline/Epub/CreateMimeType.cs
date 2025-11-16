//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.IO.Compression;
using System.Text;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class CreateMimeType : PipeLineStep<EpubState>
{
    public CreateMimeType(EpubState state) : base(state)
    {
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        logger.LogInformation("Creating mimetype...");
        await State.EpubFile.AddAsync("mimetype", "application/epub+zip", Encoding.ASCII, CompressionLevel.NoCompression);
        return StepResult.Success;
    }
}
